using System;
using Microsoft.SPOT;
using Org.Coos.Messaging.Impl;

namespace TestStartup
{
    public class Program
    {
        public static void Main()
        {
            DefaultMessage defmsg = new DefaultMessage();
           

            defmsg.setSenderEndpointUri("coos://pong");
            defmsg.setReceiverEndpointUri("coos://myartifact");

            defmsg.serialize();


        }

    }
}
