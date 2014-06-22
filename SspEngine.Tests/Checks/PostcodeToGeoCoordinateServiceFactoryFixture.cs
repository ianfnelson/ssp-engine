using FluentAssertions;
using NUnit.Framework;
using SspEngine.Checks;

namespace SspEngine.Tests.Checks
{
    [TestFixture]
    public class PostcodeToGeoCoordinateServiceFactoryFixture
    {
        [Test]
        public void Create_CreatesAnInstanceOfTheService()
        {
            // Arrange
            var sut = new PostcodeToGeoCoordinateServiceFactory();

            // Act
            var service = sut.Create();

            // Assert
            service.Should().BeAssignableTo<IPostcodeToGeoCoordinateService>();
        }

        [Test]
        public void Create_ReturnsADifferentInstanceOfTheServiceWithEachInvocation()
        {
            // Arrange
            var sut = new PostcodeToGeoCoordinateServiceFactory();

            // Act
            var service1 = sut.Create();
            var service2 = sut.Create();

            // Assert
            service1.Should().BeAssignableTo<IPostcodeToGeoCoordinateService>();
            service2.Should().BeAssignableTo<IPostcodeToGeoCoordinateService>();
            service1.Should().NotBeSameAs(service2);
        }
    }
}