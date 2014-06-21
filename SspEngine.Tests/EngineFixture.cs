using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;
using SspEngine.DomainModel;

namespace SspEngine.Tests
{
    [TestFixture]
    public class EngineFixture
    {
        private IFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoRhinoMockCustomization());

            _fixture.Customize<Postcode>(x => x.FromFactory(() => Postcode.Parse("YO8 3UW")));
        }

        [Test]
        public void Constructor_ChecksPassed_PopulatesChecksPropertyWithChecksOrderedByOrdinality()
        {
            // Arrange
            var checks = _fixture.CreateMany<ICheck>().ToArray();

            // Act
            var sut = new Engine(checks);

            // Assert
            sut.Checks.ShouldAllBeEquivalentTo(checks.OrderBy(c => c.Ordinality));
        }

        [Test]
        public void RunChecks_AllChecksReturnAccept_AcceptReturned()
        {
            // Arrange
            var risk = _fixture.Create<Risk>();

            var checks = _fixture.CreateMany<ICheck>().ToArray();

            // Set all our checks to return Accept
            foreach (var check in checks)
            {
                check.Expect(c => c.RunCheck(risk)).Return(RatingResult.Accept);
            }

            var sut = new Engine(checks);

            // Act
            var result = sut.RunChecks(risk);

            // Assert
            result.Should().Be(RatingResult.Accept);
            foreach (var check in checks)
            {
                check.VerifyAllExpectations();
            }
        }

        [Test]
        public void RunChecks_OneCheckReturnsRefer_ReferReturned()
        {
            // Arrange
            var risk = _fixture.Create<Risk>();

            var checks = _fixture.CreateMany<ICheck>().ToArray();

            // Set all our checks to return Accept...
            foreach (var check in checks)
            {
                check.Expect(c => c.RunCheck(risk)).Return(RatingResult.Accept);
            }
            // ... Except the second, which we'll have return Refer
            checks[1].BackToRecord(BackToRecordOptions.All);
            checks[1].Replay();
            checks[1].Expect(c => c.RunCheck(risk)).Return(RatingResult.Refer).Repeat.Any();

            var sut = new Engine(checks);

            // Act
            var result = sut.RunChecks(risk);

            // Assert
            result.Should().Be(RatingResult.Refer);
            foreach (var check in checks)
            {
                check.VerifyAllExpectations();
            }
        }

        [Test]
        public void RunChecks_OneCheckReturnsDecline_DeclineReturned()
        {
            // Arrange
            var risk = _fixture.Create<Risk>();

            var checks = _fixture.CreateMany<ICheck>(3).ToArray();

            // A mixture of Accept, Refer and Decline..
            checks[0].Expect(c => c.RunCheck(risk)).Return(RatingResult.Accept);
            checks[1].Expect(c => c.RunCheck(risk)).Return(RatingResult.Refer);
            checks[2].Expect(c => c.RunCheck(risk)).Return(RatingResult.Decline);

            var sut = new Engine(checks);

            // Act
            var result = sut.RunChecks(risk);

            // Assert
            result.Should().Be(RatingResult.Decline);
        }

        [Test]
        public void RunChecks_OneCheckReturnsDecline_RemainingChecksNotRun()
        {
            // Arrange
            var risk = _fixture.Create<Risk>();

            var checks = _fixture.CreateMany<ICheck>(3)
                .OrderBy(c => c.Ordinality).ToArray();
  
            // We'll set the second one of three to return a decline...
            checks[0].Expect(c => c.RunCheck(risk)).Return(RatingResult.Accept);
            checks[1].Expect(c => c.RunCheck(risk)).Return(RatingResult.Decline);

            // ... So the third check should never be run.
            checks[2].Expect(c => c.RunCheck(risk)).Return(RatingResult.Refer).Repeat.Never();

            var sut = new Engine(checks);

            // Act
            var result = sut.RunChecks(risk);

            // Assert
            foreach (var check in checks)
            {
                check.VerifyAllExpectations();
            }

        }
    }
}