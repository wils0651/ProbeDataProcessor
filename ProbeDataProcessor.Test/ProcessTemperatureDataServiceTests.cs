using NSubstitute;
using ProbeDataProcessor.Contracts;
using ProbeDataProcessor.Models;
using ProbeDataProcessor.Services;

namespace ProbeDataProcessor.Test
{
    public class ProcessTemperatureDataServiceTests
    {
        private readonly IProbeRepository _probeRepository = Substitute.For<IProbeRepository>();
        private readonly IProbeDataRepository _probeDataRepository = Substitute.For<IProbeDataRepository>();
        private readonly ITemperatureStatisticRepository _temperatureStatisticRepository = Substitute.For<ITemperatureStatisticRepository>();

        private readonly IProcessTemperatureDataService _processTemperatureDataService;

        public ProcessTemperatureDataServiceTests()
        {
            _processTemperatureDataService = new ProcessTemperatureDataService(
                _probeRepository, _probeDataRepository, _temperatureStatisticRepository);
        }

        [Fact]
        public async Task GetProbeIdsAsync_ValidData_Success()
        {
            // Arrange
            var probes = new List<Probe>
            {
                new() { ProbeId = 1, ProbeName = "probe 1" },
                new() { ProbeId = 2, ProbeName = "probe 2" }
            };

            _probeRepository.ListProbes().Returns(probes);

            // Act
            var result = await _processTemperatureDataService.GetProbeIdsAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(1, result);
            Assert.Contains(2, result);
        }

        [Fact]
        public async Task GetProbeDataByDateAsync_ValidData_Success()
        {
            // Arrange
            const int probeId = 1;

            var probeData = new List<ProbeData>()
            {
                new() {ProbeId = probeId, CreatedDate = new DateTime(2025,2,19,12,12,12), Temperature = 70.0m },
                new() {ProbeId = probeId, CreatedDate = new DateTime(2025,2,18,12,12,12), Temperature = 70.0m },
                new() {ProbeId = probeId, CreatedDate = new DateTime(2025,2,19,12,12,12), Temperature = 60.0m }
            };

            _probeDataRepository.GetAllPastProbeData(probeId).Returns(probeData);

            // Act
            var result = await _processTemperatureDataService.GetProbeDataByDateAsync(probeId);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }
    }
}