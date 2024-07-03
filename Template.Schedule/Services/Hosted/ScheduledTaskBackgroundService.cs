namespace Template.Schedule.Services.Hosted
{
    public class ScheduledTaskBackgroundService : CustomBackgroundService<ScheduledTaskBackgroundService>
    {
        public ScheduledTaskBackgroundService(ILogger<ScheduledTaskBackgroundService> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {

        }

        protected override TimeSpan TimeSpanInSecond { get; set; } = TimeSpan.FromSeconds(10);

        protected override async void InternalDoJob()
        {
            Logger.LogError("ScheduledTaskBackgroundService.DoJob");

        }
    }
}
