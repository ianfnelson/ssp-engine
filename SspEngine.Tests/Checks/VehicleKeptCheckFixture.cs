using System;
using System.Device.Location;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;
using SspEngine.Checks;
using SspEngine.DomainModel;

namespace SspEngine.Tests.Checks
{
    [TestFixture]
    public class VehicleKeptCheckFixture
    {
        private IFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoRhinoMockCustomization());

            _fixture.Customize<Postcode>(x => x.FromFactory(() => Postcode.Parse("YO8 3UW")));
        }

        [TearDown]
        public void TearDown()
        {
            AppSettingsProvider.ResetToDefault();
        }

        [TestCase(20D, 19.99D)]
        [TestCase(20D, 10.0D)]
        [TestCase(8D, 6D)]
        public void InterpretDistance_BelowAcceptBelowMetresSetting_ReturnsAccept(double acceptBelowMetres,
            double calculatedDistance)
        {
            // Arrange
            var mockAppSettingsProvider = MockRepository.GenerateMock<AppSettingsProvider>();
            mockAppSettingsProvider.Expect(x => x.VehicleKeptCheck_AcceptBelowMetres).Return(acceptBelowMetres);
            AppSettingsProvider.Current = mockAppSettingsProvider;

            var mockServiceFactory = MockRepository.GenerateMock<IPostcodeToGeoCoordinateServiceFactory>();
            var sut = new VehicleKeptCheck(mockServiceFactory);

            // Act
            var result = sut.InterpretDistance(calculatedDistance);

            // Assert
            result.Should().Be(RatingResult.Accept);

        }

        [TestCase(20D, 40D, 20D)]
        [TestCase(20D, 40D, 30D)]
        [TestCase(20D, 40D, 39.99D)]
        [TestCase(8D, 10D, 9D)]
        public void InterpretDistance_BetweenAcceptBelowMetresAndReferBelowMetresSettings_ReturnsRefer(double acceptBelowMetres, double referBelowMetres, double calculatedDistance)
        {
            // Arrange
            var mockAppSettingsProvider = MockRepository.GenerateMock<AppSettingsProvider>();
            mockAppSettingsProvider.Expect(x => x.VehicleKeptCheck_AcceptBelowMetres).Return(acceptBelowMetres);
            mockAppSettingsProvider.Expect(x => x.VehicleKeptCheck_ReferBelowMetres).Return(referBelowMetres);
            AppSettingsProvider.Current = mockAppSettingsProvider;

            var mockServiceFactory = MockRepository.GenerateMock<IPostcodeToGeoCoordinateServiceFactory>();
            var sut = new VehicleKeptCheck(mockServiceFactory);

            // Act
            var result = sut.InterpretDistance(calculatedDistance);

            // Assert
            result.Should().Be(RatingResult.Refer);

        }

        [TestCase(40D, 40D)]
        [TestCase(40D, 50D)]
        [TestCase(10D, 12D)]
        public void InterpretDistance_AboveOrEqualReferBelowMetresSettings_ReturnsDecliner(double referBelowMetres, double calculatedDistance)
        {
            // Arrange
            var mockAppSettingsProvider = MockRepository.GenerateMock<AppSettingsProvider>();
            mockAppSettingsProvider.Expect(x => x.VehicleKeptCheck_ReferBelowMetres).Return(referBelowMetres);
            AppSettingsProvider.Current = mockAppSettingsProvider;

            var mockServiceFactory = MockRepository.GenerateMock<IPostcodeToGeoCoordinateServiceFactory>();
            var sut = new VehicleKeptCheck(mockServiceFactory);

            // Act
            var result = sut.InterpretDistance(calculatedDistance);

            // Assert
            result.Should().Be(RatingResult.Decline);
        }

        [Test]
        public void GetCoordinatePair_CallsDifferentInstanceOfServiceForEachCoordinate()
        {
            // Arrange
            var addressPostcode = Postcode.Parse("YO8 3UW");
            var keptPostcode = Postcode.Parse("YO8 3UT");

            var addressGeoCoordindate = new GeoCoordinate(53.811009D, -1.100275D);
            var keptGeoCoordinate = new GeoCoordinate(53.812604D, -1.097173D);

            var risk = _fixture.Create<Risk>();
            risk.Address.Postcode = addressPostcode;
            risk.KeptPostcode = keptPostcode;

            var mockService1 = MockRepository.GenerateMock<IPostcodeToGeoCoordinateService>();
            mockService1.Expect(x => x.GetCoordinatesForPostcode(addressPostcode))
                .Return(addressGeoCoordindate);
            var mockService2 = MockRepository.GenerateMock<IPostcodeToGeoCoordinateService>();
            mockService2.Expect(x => x.GetCoordinatesForPostcode(keptPostcode))
                .Return(keptGeoCoordinate);

            var mockServiceFactory = MockRepository.GenerateMock<IPostcodeToGeoCoordinateServiceFactory>();
            mockServiceFactory.Expect(x => x.Create()).Return(mockService1).Repeat.Once();
            mockServiceFactory.Expect(x => x.Create()).Return(mockService2).Repeat.Once();

            var sut = new VehicleKeptCheck(mockServiceFactory);

            // Act
            var result = sut.GetCoordinatePair(risk);

            // Assert
            mockService1.VerifyAllExpectations();
            mockService2.VerifyAllExpectations();
            mockServiceFactory.VerifyAllExpectations();

            result.Item1.Should().Be(addressGeoCoordindate);
            result.Item2.Should().Be(keptGeoCoordinate);
        }

        [TestCase(53.811009D, -1.100275D, 53.812604D, -1.097173D, 270.292046203046D)]
        public void RunCheck_GetsCoordinatePairCalculatesDistanceBetweenAndInterpretsResult(double addressLatitude, double addressLongitude, double keptLatitude, double keptLongitude, double expectedDistance)
        {
            // Arrange
            var risk = _fixture.Create<Risk>();

            var keptGeo = new GeoCoordinate(keptLatitude, keptLongitude);
            var addressGeo = new GeoCoordinate(addressLatitude, addressLongitude);
            
            var mockServiceFactory = MockRepository.GenerateMock<IPostcodeToGeoCoordinateServiceFactory>();

            var sut = MockRepository.GeneratePartialMock<VehicleKeptCheck>(mockServiceFactory);

            var randomResult = _fixture.Create<RatingResult>();

            sut.Expect(x => x.InterpretDistance(0D)).IgnoreArguments().Return(randomResult);
            sut.Expect(x => x.GetCoordinatePair(risk))
                .Return(new Tuple<GeoCoordinate, GeoCoordinate>(addressGeo, keptGeo));

            // Act
            var result = sut.RunCheck(risk);

            // Assert
            var distanceParam = (double)sut.GetArgumentsForCallsMadeOn(x => x.InterpretDistance(0D))[0][0];

            distanceParam.Should().BeApproximately(expectedDistance, 0.0001D);

            sut.VerifyAllExpectations();
            result.Should().Be(randomResult);
        }
    }
}