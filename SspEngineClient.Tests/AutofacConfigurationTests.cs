using System.Linq;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using SspEngine;
using SspEngine.Checks;

namespace SspEngineClient.Tests
{
    [TestFixture]
    public class AutofacConfigurationTests
    {
        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            _container = AutofacConfiguration.Configure();
        }

        [Test]
        public void ConfigureAutofac_FollowingConfiguration_WeCanResolveCore()
        {
            // Arrange

            // Act
            var core = _container.Resolve<ICore>();

            // Assert
            core.Should().NotBeNull();
        }

        [Test]
        public void ConfigureAutofac_FollowingConfiguration_ResolvedEngineHasExpectedChecks()
        {
            // Arrange

            // Act
            var engine = _container.Resolve<IEngine>() as Engine;

            // Assert
            engine.Checks.First().Should().BeAssignableTo<OccupationCheck>();
            engine.Checks.Last().Should().BeAssignableTo<VehicleKeptCheck>();
        }
    }
}