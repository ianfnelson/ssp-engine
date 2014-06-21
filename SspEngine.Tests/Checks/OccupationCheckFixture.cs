using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using SspEngine.Checks;
using SspEngine.DomainModel;

namespace SspEngine.Tests.Checks
{
    [TestFixture]
    public class OccupationCheckFixture
    {
        private IFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoRhinoMockCustomization());

            _fixture.Customize<Postcode>(x => x.FromFactory(() => Postcode.Parse("YO8 3UW")));
        }

        [TestCase("Chef", RatingResult.Refer)]
        [TestCase("Footballer", RatingResult.Accept)]
        [TestCase("IT Contractor", RatingResult.Decline)]   // ho ho
        public void OccupationsInTheLookupData_ReturnsExpectedResult(string occupation, RatingResult expectedResult)
        {
            var risk = _fixture.Build<Risk>().With(p => p.Occupation, occupation).Create();

            var sut = new OccupationCheck();

            var result = sut.RunCheck(risk);

            result.Should().Be(expectedResult);
        }

        [TestCase("CHEF", RatingResult.Refer)]
        [TestCase("FOOTballer", RatingResult.Accept)]
        [TestCase("it ContractoR", RatingResult.Decline)]  
        public void OccupationsInTheLookupData_LookupsAreCaseInsensitive(string occupation, RatingResult expectedResult)
        {
            var risk = _fixture.Build<Risk>().With(p => p.Occupation, occupation).Create();

            var sut = new OccupationCheck();

            var result = sut.RunCheck(risk);

            result.Should().Be(expectedResult);
        }

        [TestCase("Goatherd")]
        [TestCase("Henchman")]
        [TestCase("Pargetter")]
        [TestCase("IT Recruiter")]  // ho ho
        public void OccupationsNotInTheLookupData_Declined(string occupation)
        {
            var risk = _fixture.Build<Risk>().With(p => p.Occupation, occupation).Create();

            var sut = new OccupationCheck();

            var result = sut.RunCheck(risk);

            result.Should().Be(RatingResult.Decline);
        }

    }
}