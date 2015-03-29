using java.lang;
using Org.Coos.Module;
using Org.Coos.Messaging.Util;

namespace Org.Coos.Messaging.Impl
{
    public class ConsumerTask : IRunnable
    {
        private IConsumer consumer;
        private IExchange exchange;
        private string uri;
        private ILog log;

        public ConsumerTask(IConsumer consumer, IExchange exchange, ILog log, string uri)
        {
            this.consumer = consumer;
            this.exchange = exchange;
            this.log = log;
            this.uri = uri;
        }

        public void run()
        {

            // Todo might insert e semaphore here to control
            // concurrent access to consumer
            log.debug(
                "Endpoint: " + uri + ", Processing incoming exchange: " +
                exchange);

            consumer.process(exchange);
        }
         
    }

}