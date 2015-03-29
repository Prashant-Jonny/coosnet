using java.lang;
using System.Collections.Generic;


namespace Org.Coos.Messaging.Impl
{
    /// <summary>
    /// Callback processing in default endpoint
    /// </summary>
    public class CallbackTask : IRunnable
    {
        private Dictionary<string,IAsyncCallback> callbacks;
        string xId;
        IExchange exchange;

        public CallbackTask(Dictionary<string, IAsyncCallback> callbacks, string xId, IExchange exchange)
        {
            this.callbacks = callbacks;
            this.xId = xId;
            this.exchange = exchange;

        }

        public void run()
        {

            // this is an async return
            IAsyncCallback callback = (IAsyncCallback)callbacks[xId];
            callbacks.Remove(xId);
            callback.processExchange(exchange);

        }


        
    }
}
