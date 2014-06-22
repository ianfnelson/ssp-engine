using FluentAssertions;
using NUnit.Framework;
using SspEngine.Checks;
using SspEngine.DomainModel;

namespace SspEngine.Tests.Checks
{
    [TestFixture]
    public class PostcodeToGeoCoordinateServiceFixture
    {
        [TestCase("YO8 3UW", 53.812604D, -1.097173D)]
        [TestCase("W11 1JA", 51.516117D, -0.204534D)]
        public void GetCoordinatesForPostcodeTests(string postcodeString, double expectedLatitude, double expectedLongitude)
        {
            // Arrange
            var postcode = Postcode.Parse(postcodeString);
            var sut = new PostcodeToGeoCoordinateService();

            // Act
            var coordinate = sut.GetCoordinatesForPostcode(postcode);

            // Assert
            coordinate.Latitude.Should().BeApproximately(expectedLatitude, 0.000001D);
            coordinate.Longitude.Should().BeApproximately(expectedLongitude, 0.000001D);
        }
    }
}