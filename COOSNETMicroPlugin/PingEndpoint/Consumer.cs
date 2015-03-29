using Org.Coos.Messaging.Impl;
using System;
using Microsoft.SPOT;
using System.Runtime.CompilerServices;

namespace Org.Coos.Messaging.Ping
{
    public class Consumer : DefaultConsumer
    {
        public Consumer(IEndpoint endpoint)
            : base(endpoint)
        {
        }

        /// <summary>
        /// Method is called from processMessage in DefaultEndpoint, consumer starts further processing (if started from 
        /// default endpoint it runs on a dedicated thread)
        /// </summary>
        /// <param name="exchange"></param>
        //[MethodImpl(MethodImplOptions.Synchronized)] - adding thread safety if needed
        public override void process(IExchange exchange)
        {
            IMessage msg = exchange.getInBoundMessage();
            Debug.Print("Ping endpoint consumer got msg.: " + msg.Name);
            Debug.Print("Message body: " + msg.getBodyAsString());
            //throw new System.NotImplementedException();
        }
    }
}