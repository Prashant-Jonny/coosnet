using System;


using Org.Coos.Messaging;
using Org.Coos.Messaging.Impl;

namespace Org.Coos.Messaging.GPS
{
    public class GPSEndpoint : DefaultEndpoint
    {
        public GPSEndpoint()
            : base()
        {

            setName("GPS.NET-Plugin");
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
            Services.Add(producer);

            return producer;
        }


        public override IConsumer createConsumer()
        {
            return new Consumer(this);
        }

        
        
    }
}
