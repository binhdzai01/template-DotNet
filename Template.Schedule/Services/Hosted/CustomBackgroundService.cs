namespace Template.Schedule.Services.Hosted
{
    public abstract class CustomBackgroundService<T> : IHostedService, IDisposable
    {
        protected ILogger<T> Logger;
        protected IServiceProvider ServiceProvider;
        private Timer _timer;
        private volatile bool _isProcessing;

        protected CustomBackgroundService(ILogger<T> logger, IServiceProvider serviceProvider)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoJob, null, TimeSpan.Zero, TimeSpanInSecond);
            return Task.CompletedTask;
        }

        protected abstract TimeSpan TimeSpanInSecond { get; set; }

        private void DoJob(object state)
        {
            if (_isProcessing)
            {
                return;
            }

            try
            {
                _isProcessing = true;
                InternalDoJob();
            }
            finally
            {
                _isProcessing = false;
            }
        }

        protected abstract void InternalDoJob();

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
