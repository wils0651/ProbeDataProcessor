namespace ProbeDataProcessor.Contracts
{
    public interface IJob
    {
        /// <summary>
        /// Run the job.
        /// </summary>
        /// <returns></returns>
        public Task RunAsync();
    }
}
