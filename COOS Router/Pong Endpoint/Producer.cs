using Org.Coos.Messaging.Impl;
using Org.Coos.Messaging;
using Org.Coos.Messaging.Util;

using System.Threading;

using System.Collections;
using System;







namespace Org.Coos.Messaging.Plugin.Pong
{
    public class Producer : DefaultProducer
    {
        Thread loadThread;
        bool running = true;


        protected readonly ILog log = LogFactory.getLog(typeof(Producer).FullName);

        public Producer(IEndpoint endpoint)
            : base(endpoint)
        {

        }

        // A producer is a service

        public override void start()
        {
            // base.start();
            log.info("thePong endpoint producer starting load test.");

            //loadThread = new Thread(loadTest);

            //loadThread.Start();


        }

        private void loadTest()
        {

            IMessage msg = new DefaultMessage("Ping!");
            Hashtable ht = new Hashtable();
            ht.Add("cat", "animal");
            ht.Add("dog", "animal");
            ht.Add("tiger", "animal");
            float f = 3.14F;
            ht.Add("pi float", f);
            double d = 3.14;
            ht.Add("pi double", d);
            ht.Add("integer", (System.Int32)32);
            ht.Add("long", (long)111345345345);
            ArrayList ar = new ArrayList();
            ar.Add("Just");
            ar.Add("testing");
            ar.Add((int)1);
            ar.Add((float)2);
            ar.Add((double)3);
            ht.Add("Test C#arraylist/Java vector", ar);
            double[] da = new double[] { 11.1, 22.2, 33.3 };
            ht.Add("double array", da);
            byte[] ba = new byte[] { 101, 102, 103 };
            ht.Add("byte array", ba);






            msg.setBody(ht);
            //msg.setBody("Hello world!");
            // msg.setBody((long)3);

            while (running)
            {
                for (int i = 0; i < 50; i++)
                {



                    sendMessage("coos://thePong/pongActor@Pong", msg);
                }
                Thread.Sleep(10000);


                //  sendMessageRobust("coos://thePong/pongActor@Pong", msg);
            }


        }



        public override void stop()
        {
            base.stop();

            running = false;

        }
    }
}