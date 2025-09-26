using NSubstitute;
using ProbeDataProcessor.Contracts;
using ProbeDataProcessor.Models;
using ProbeDataProcessor.Services;

namespace ProbeDataProcessor.Test
{
    public class GarageDistanceDataServiceTests
    {
        private readonly IGarageDistanceSensorDataRepository _garageDistanceSensorDataRepository
            = Substitute.For<IGarageDistanceSensorDataRepository>();

        private readonly IGarageDistanceDataService _garageDistanceDataService;

        public GarageDistanceDataServiceTests()
        {
            _garageDistanceDataService = new GarageDistanceDataService(_garageDistanceSensorDataRepository);
        }

        [Fact]
        public async Task GetGarageDistancesAsync_ValidData_Success()
        {
            // Arrange
            var garageDistances = new List<GarageDistance>
            {
                new(),
                new()
            };

            _garageDistanceSensorDataRepository.GetPastGarageDataAsync().Returns(garageDistances);

            // Act
            var result = await _garageDistanceDataService.GetGarageDistancesAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task DeleteGarageDistances_ValidData_Success()
        {
            // Arrange
            var garageDistances = new List<GarageDistance>
            {
                new(),
                new()
            };

            // Act
            await _garageDistanceDataService.DeleteGarageDistances(garageDistances);
        }
    }
}
