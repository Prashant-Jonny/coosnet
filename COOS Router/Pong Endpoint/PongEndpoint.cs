using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Org.Coos.Messaging;
using Org.Coos.Messaging.Impl;

namespace Org.Coos.Messaging.Plugin.Pong
{
    public class PongEndpoint : DefaultEndpoint
    {

         public PongEndpoint()
            : base()
        {
            setName("thePong");

        }

        /// <summary>
        /// Method is called during startup in DefaultEndpoint (poststart-method). At the moment pr. 04 nov 2010
        /// IProducer interface is empty, so there is no methods accessible, unless casting for ex. to IService
        /// </summary>
        /// <returns>IProducer</returns>
        public override IProducer createProducer()
        {
           // return base.createProducer();
            IProducer producer = new Producer(this);
           
            (producer as IService).start();

           // Register service with endpoint, so that it can be stopped from endpoint
            Services.Add(producer as IService);

            return producer;
        }


        public override IConsumer createConsumer()
        {
            return new Consumer(this);
        }

        
    }
}
