using AutoMapper;
using NUnit.Framework;

namespace SspEngineClient.Tests
{
    [TestFixture]
    public class AutoMapperTests
    {
        [Test]
        public void AutoMapper_TestMappings()
        {
            // Arrange

            // Act
            AutoMapperConfiguration.Configure();

            // Assert
            Mapper.AssertConfigurationIsValid();
        }
    }

}
