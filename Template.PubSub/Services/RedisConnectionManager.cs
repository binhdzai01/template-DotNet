using StackExchange.Redis;

namespace Template.PubSub.Services
{
    public class RedisConnectionManager
    {
        public RedisConnectionManager()
        {
            lock (locker)
            {
                if (lazyConnection == null)
                {
                    lazyConnection = new Lazy<ConnectionMultiplexer>(() => { return ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false"); });
                }
            }
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;
        private static readonly object locker = new();

        public ConnectionMultiplexer Connection
        {
            get { return lazyConnection.Value; }
        }
    }
}
