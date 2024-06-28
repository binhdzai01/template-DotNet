using Newtonsoft.Json;
using StackExchange.Redis;

namespace Template.PubSub.Services
{
    public interface IPubSubService
    {
        void SubscribeInternal();
        long Publish(string channel, object data);
        void PublishSystem(object data);
    }

    public class PubSubService : IPubSubService
    {
        private const string channel = "channel";
        public ISubscriber _subscribe { get; set; }
        public ILogger<PubSubService> _logger { get; set; }
        private readonly IServiceProvider _serviceProvider;

        public PubSubService(RedisConnectionManager redisConnManager, ILogger<PubSubService> logger, IServiceProvider serviceProvider)
        {
            try
            {
                _logger = logger;
                _subscribe = redisConnManager.Connection.GetSubscriber();
            }
            catch
            {

            }
            _serviceProvider = serviceProvider;
        }

        public void SubscribeInternal()
        {
            _logger.LogError($"Subcribe");

            _subscribe.SubscribeAsync(new RedisChannel(channel, RedisChannel.PatternMode.Literal),
                async (channel, message) =>
                {
                    try
                    {
                        await HandleSubscribe(message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error when handle Subcribe : {ex.Message}");
                    }
                });
        }

        public long Publish(string channel, object data)
        {
            var message = JsonConvert.SerializeObject(data);

            _subscribe.PublishAsync(channel, message);

            return 1;
        }

        public void PublishSystem(object data)
        {
            Publish(channel, data);
        }

        private async Task HandleSubscribe(string message)
        {
            _logger.LogError("Received Message from Publish");
            //var msg = JsonConvert.DeserializeObject<PubSubMessageDTO>(message);

            //if (msg == null) return;

            //await UpdateCache(msg);
        }

        //implement handle subcribe
        //private async Task UpdateCache(PubsubMessage msg)
        //{
        //    //implement
        //    throw new ApplicationException
        //}
    }
}
