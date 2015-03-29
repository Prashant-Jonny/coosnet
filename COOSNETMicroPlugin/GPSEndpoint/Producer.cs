using Org.Coos.Messaging.Impl;
using Org.Coos.Messaging;
using Org.Coos.Messaging.Util;

using System.Threading;

using System.Collections;
using System;


using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using Hks.Itprojects.GPS;


namespace Org.Coos.Messaging.GPS
{
    public class Producer : DefaultProducer
    {
        Thread loadThread;
        bool running = true;
        IMessage msg;
        IGPSSensor gpsSensor;

       protected readonly  Org.Coos.Messaging.Util.ILog  log = LogFactory.getLog(typeof(Producer).FullName) as Org.Coos.Messaging.Util.ILog;

        public Producer(IEndpoint endpoint)
            : base(endpoint)
        {
            
           
        }

        // A producer is a service

        public override void start()
        {
           // base.start();
           log.info("GPS simulator endpoint producer starting load test.");

            loadThread = new Thread (loadTest);
            loadThread.Start();
          
        }

        private void loadTest()
        {
           
              msg = new DefaultMessage("GPS simulated position");
               
               
                // GPS Sensor data


             gpsSensor = new GPSSensor(500, 3000);
               
                gpsSensor.NewPosition += new PositionEventHandler(sendPosition);


                //while (running)
                //{
                //    for (int i = 0; i < 50; i++)
                //    {
                        
                //        Position p = gpsSensor.readPosition();
               //         sendPosition(this,new PositionArgs(p));

                //    }
                //   Thread.Sleep(10000);


                //  //  sendMessageRobust("coos://thePong/pongActor@Pong", msg);
                //}
            
        }

        /// <summary>
        /// NewPosition event handler, sends position to server endpoint
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pArgs"></param>
        private void sendPosition(object sender, PositionArgs pArgs)
        {
            Hashtable positionProperties = preparePositionMessage(pArgs.Position);

            msg.setBody(positionProperties);

           IExchange exchange =  sendMessage("coos://thePong/pongActor@Pong", msg);
            
        }

        private  Hashtable preparePositionMessage(Position p)
        {
           

            Hashtable ht = new Hashtable();

            //double[] dPos = new double[2];
            //dPos[0] = p.Latitude;
            //dPos[1] = p.Longitude;

            ht.Add("Device ID", (int)Microsoft.SPOT.Hardware.SystemInfo.SystemID.SKU);
            //ht.Add("GPS position", dPos);
            ht.Add("GPS latitude",p.Latitude);
            ht.Add("GPS longitude",p.Longitude);
            ht.Add("GPS timestamp", p.TimeStamp.Ticks);
            ht.Add("Battery", (int)Battery.StateOfCharge());

            return ht;

            
        }

        public override void stop()
        {
            base.stop();

            running = false;

            gpsSensor.stopTimer(); 
            // Another method maybe let gpsSensor implement IService and register with endpoint,
            // but that will make the code dependent upon COOS
           

        }
    }
}