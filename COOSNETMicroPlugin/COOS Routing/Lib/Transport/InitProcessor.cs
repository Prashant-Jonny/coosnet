using Org.Coos.Messaging;
using Org.Coos.Messaging.Routing;
using Org.Coos.Messaging.Impl;
using Org.Coos.Messaging.Util;  

using System.Collections;

private class InitProcessor : DefaultProcessor {
        ITransport transport;
        ILinkManager linkManager;

        public InitProcessor(ITransport transport,ILinkManager linkManager) {
            this.transport = transport;
            this.linkManager = linkManager;
        }

       

       
    }