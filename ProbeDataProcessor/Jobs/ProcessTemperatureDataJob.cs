using ProbeDataProcessor.Contracts;

namespace ProbeDataProcessor.Jobs
{
    public class ProcessTemperatureDataJob
    {
        private readonly IProcessTemperatureDataService _processTemperatureDataService;

        public ProcessTemperatureDataJob(IProcessTemperatureDataService processTemperatureDataService)
        {
            this._processTemperatureDataService = processTemperatureDataService;
        }

        public async Task RunAsync()
        {
            // Get the probe ids

            // FOR EACH ID

            // Get the probe data

            // Break into days

            // Process statistics

            // Save statistics

            // delete probe data
        }
    }
}
