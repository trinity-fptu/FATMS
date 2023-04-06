using Application.Interfaces;
using Application.Services;
using Domain.Tests;

namespace Application.Tests.Services
{
    public class CurrentTimeTests : SetupTest
    {
        private readonly ICurrentTime _currentTime;

        public CurrentTimeTests()
        {
            _currentTime = new CurrentTime();
        }

        [Fact]
        public void GetCurrentTime_ShouldReturnTimeExactlyToTheMiliSec()
        {
            //Arrange
            var expected = DateTime.UtcNow;

            //Act
            var result = _currentTime.GetCurrentTime();

            //Assert
            TimeSpan diff = result - expected;
            var timeDiffLessThan1MiliSec = diff < TimeSpan.FromMilliseconds(1);
            Assert.True(timeDiffLessThan1MiliSec);
        }
    }
}