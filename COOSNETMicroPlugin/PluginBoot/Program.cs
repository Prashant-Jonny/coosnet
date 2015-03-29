#region Using
using System;
using Microsoft.SPOT;
using System.Threading;
using Microsoft.SPOT.Net.NetworkInformation;
using System.IO;
using System.Collections;
using System.Reflection;
using Microsoft.SPOT.Hardware;

using Org.Coos.Messaging.Ping;
using Org.Coos.Messaging.GPS;

using Org.Coos.Messaging;
using Org.Coos.Messaging.NETMicro.Transport;
using Org.Coos.Messaging.Impl;
using Org.Coos.Messaging.Util;

using Org.Coos.Messaging.NETMicro;
#endregion


namespace Org.Coos.Messaging.PingNETBoot
{
    /// <summary>
    /// This plugin will try to establish contact with a dummy COOS server, than only responds to connect and send connect ack
    /// COOS fake server is running on the loopback interface for testing on local machine.
    /// </summary>
    public class Program
    {
        const string loopbackIPadr = "127.0.0.1";
        const string routerPort = "15656";
        const string routerSSLPort = "15666";
        const string routerUDPPort = "15676";

        const string retryConnect = "true"; // Transport will retry connection to router, if it fails

        const int shutdownTime = 60 * 1000; // Time before shutdown of endpoint

        public static void Main()
        {

            // Useful for seeing memory information 
            Debug.EnableGCMessages(true);
            

           //   networkInterfaces();

           // testSerializationDeserialization();
           
           // testFloatDouble();

           // threadPoolTest();

            
           // Debug.Print("OEM: "+Microsoft.SPOT.Hardware.SystemInfo.OEMString);
           // Debug.Print("BOOT: CPU speed (Hz) : " + Cpu.SystemClock);
           // Debug.Print("CPU slow clock (Hz) - used for precise timing: " + Cpu.SlowClock);

           
            //showBatteryStatus();

           // showAssembliesMemoryInfo();

            //Debug.GC(true); // Gives valuable information about memory

            #region Endpoint
            Debug.Print("BOOT:Trying to boot endpoint...");

           
            // Conceptually this is the highest level endpoint - exchange
          
           // IEndpoint Endpoint = new PingEndpoint();
            
            IEndpoint Endpoint = new GPSEndpoint();
            

            if (Endpoint.getEndpointUuid() != null)
              Endpoint.setEndpointUuid(null); // Most likely null, but make sure its null to get allocated id from router, follows the spirit of DHCP

            // PingEndpoint override CreateProducer that does the below statement
            //PingProducer pingProducer = new PingProducer(pingEndpoint);
            #endregion

            #region Channel
            // Next level is the channel
            PluginChannel Channel = new PluginChannel();
           

            //pingChannel.Name = "PingNETChannel";
            //pingChannel.setName("PingNETChannel");
            #endregion

            #region Transport
            // And finally the lowest level is transport
            ITransport Transport = new TCPTransport();
            //ITransport pingTransport = new SecureTCPTransport();
            //ITransport pingTransport = new UDPTransport();
           
            
            string COOSRouterIPAdr = loopbackIPadr; // Test it on loopback
            //string COOSRouterPort = routerUDPPort;
            
            //string COOSRouterPort = routerSSLPort;
              string COOSRouterPort = routerPort;
            string rootSegment = ".";
           
            //Transport.setName("PingNETTCPTransport");

            #endregion

            #region Create plugin
            Plugin Plugin = null;

            Plugin = createPlugin(Endpoint.getName(),Endpoint, Channel, Transport, COOSRouterIPAdr, COOSRouterPort, rootSegment, retryConnect, Plugin);

            #endregion

            #region Start COOS fake server
            startServer(false);


            // Turn off life cycle reporting
            Endpoint.setProperty(IEndpointPrimitives.PROP_LCM_REGISTRATION_ENABLED, "false");

            #endregion

            #region Connect plugin to server
            // Connect plugin with server
            connectPlugin(COOSRouterIPAdr, COOSRouterPort, Plugin);

            if (Plugin.Connected)
            {
                Debug.Print("BOOT: Plugin is connected to server");
            //    IMessage msg = new DefaultMessage("Ping!");
               
            //    IMessage responseMsg = pingEndpoint.sendMessage(msg, pingEndpoint.getEndpointUri(), ExchangePattern.OutOnly);
            }

            #endregion


            Debug.Print("BOOT: Shutdown of endpoint in " + shutdownTime.ToString() + " millisec.");
            Thread.Sleep(shutdownTime); // wait before shutdown

            Endpoint.shutDownEndpoint();


        }

        private static void startServer(bool fake)
        {
            // Setup simple fake server to process connect request from plugin

            Thread tServer = new Thread(new ThreadStart(COOSFakeRouter.Main));

            if (fake)
                tServer.Start();

            if (tServer.IsAlive)
                Debug.Print("BOOT: Endpoint running fake server for testing purposes");
            else
                Debug.Print("BOOT: Endpoint expecting COOS server");
        }

        private static void showBatteryStatus()
        {
            if (Battery.OnCharger())
            {
                int state = Battery.StateOfCharge();
                string s = String.Empty;
                for (int l = 0; l < state; l++)
                    s += "*";
                Debug.Print("Battery on charger : " + s+" level = "+state.ToString());
            }

            if (Battery.IsFullyCharged())
                Debug.Print("Battery fully charged");

            Debug.Print("Battery temperature : "+Battery.ReadTemperature().ToString()+" Voltage : "+Battery.ReadVoltage().ToString()+" ");


        }

        private static void threadPoolTest()
        {
            ThreadPool tpool = new ThreadPool("Test thread pool", 10);

            for (int i = 0; i < 100; i++)
            {
                Debug.Print("Queing task nr. " + i.ToString());
                tpool.execute(new TestTask());
                Thread.Sleep(10); // Give some time to let threadpool-thread start running
            }
        }

        private static void showAssembliesMemoryInfo()
        {
            Debug.Print("Memory consumption:");

            Assembly[] assms = Reflection.GetAssemblies();
            foreach (Assembly assm in assms)
            {
                Reflection.AssemblyMemoryInfo ami = new Reflection.AssemblyMemoryInfo();
                Reflection.GetAssemblyMemoryInfo(assm, ami);
                string fullname = assm.FullName;
                if (fullname.IndexOf("Microsoft") != 0 && fullname.IndexOf("System") != 0 && fullname.IndexOf("mscorlib") != 0)
                {
                    Debug.Print(assm.FullName + " Ram size:" + ami.RamSize.ToString() + " Rom size: " + ami.RomSize.ToString());

                    //foreach (Type t in assm.GetTypes())
                    //    Debug.Print(" ---> "+t.FullName);
                }
            }
        }

        private static void networkInterfaces()
        {
            // Network interfaces on device

            NetworkInterface[] allInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            // Wireless80211[] allInterfaces = (Wireless80211[])Wireless80211.GetAllNetworkInterfaces();



            // Find first wireless 802.11 network interface

            //for (int idx = 0; idx < allInterfaces.Length; idx++)
            //    if (allInterfaces[idx].NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
            //    {
            //        wirelessInterf = allInterfaces[idx];
            //        break;
            //    }

            //string ssid;

            //if (wirelessInterf != null)
            //    ssid = wirelessInterf.Ssid;
        }

        private static void testSerializationDeserialization()
        {
            // Serialization /deserialization testing
            IMessage msg = new DefaultMessage("Test");
            msg.setBody("string");
            //byte[] serial = msg.serialize();
            msg.setBody((string)null);
            string test = msg.getBodyAsString();

            //Hashtable ht = new Hashtable();
            //ht.Add("book", "tcp ip illustrated volume 1");



            //msg.setBody(ht);
            //byte[] serial = msg.serialize();
            //msg.setBody((Hashtable)null);

            //Hashtable ht2 = msg.getBodyAsProperties();

            //foreach (object o in ht2.Keys)
            //    Debug.Print("Key: " + o.ToString() + "Value: " + ht2[o].ToString());


            // .NET micro framework serialization/deserialization

            //byte[] integer = Reflection.Serialize(32, typeof(Int32));
            //byte[] str = Reflection.Serialize("Test string", typeof(string));

            //int i = (int)Reflection.Deserialize(integer, typeof(Int32));
            //string s = (string)Reflection.Deserialize(str, typeof(string));
        }

        private static void testFloatDouble()
        {
            MemoryStream ms = new MemoryStream();
            DataOutputStream dout = new DataOutputStream(ms);
            DataInputStream dis = new DataInputStream(ms);

            float f = 3.14F;
            dout.writeFloat(f);
            ms.Position = 0;
            float t = dis.readFloat();


            double d = 3.14;
            ms.Position = 0;
            dout.writeDouble(d);
            ms.Position = 0;
            double td = dis.readDouble();
        }

        private static  void connectPlugin(string COOSRouterIPAdr, string COOSRouterPort, Plugin pingPlugin)
        {
            // Connect plugin
            Debug.Print("BOOT:Trying to connect plugin to COOS router at " + COOSRouterIPAdr + ":" + COOSRouterPort);
            try
            {
                pingPlugin.connect();
            }
            catch (System.Exception e)
            {
                Debug.Print("BOOT:Problems with connecting plugin to network and retrieving COOS address for endpoint" + e.Message);
            }
        }

        private static Plugin createPlugin(string name,IEndpoint endpoint, PluginChannel Channel, ITransport Transport, string COOSRouterIPAdr, string COOSRouterPort, string segment, string retryConnect, Plugin Plugin)
        {
            // Create plugin
            Debug.Print("BOOT:Creating plugin and sets up channel and transport");
            try
            {
                Plugin = PluginFactory.createPlugin(name, endpoint, Channel, Transport, segment, COOSRouterIPAdr, COOSRouterPort, retryConnect);
            }
            catch (System.Exception e)
            {
                Debug.Print("BOOT:Problems with creating plugin " + e.Message);
            }
            return Plugin;
        }

    }

    // Abtraction levels pattern followed:

    // Interface - method declaration
    //  Abstract class - some implementation of interface method, allows overrides in inherited classes
    //      Class
    //          more implementation
    //public interface ITest
    //{
    //    void connect();
    //}

    //public abstract class DefaultTest : ITest
    //{
    //    public virtual void connect() { }
    //}

    //public class Test : DefaultTest
    //{
    //    public override void connect()
    //    {
    //        int i = 1;
    //    }
    //}
}
