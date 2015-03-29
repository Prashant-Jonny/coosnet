using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Collections;

using Org.Coos.Messaging;
using Org.Coos.Messaging.Transport;
using Org.Coos.Messaging.Util;

namespace COOS_Router
{
    public class NETContainer : ICOContainer
    {

        private readonly ILog logger = LogFactory.getLog(typeof(NETContainer).FullName);

        Assembly coosMessagingAssembly;

        COOS coosInstance;

        public NETContainer()
        {
            Thread.CurrentThread.Name = "NETContainer thread";
            coosMessagingAssembly = Assembly.LoadFile(@"C:\Users\henning\Documents\Visual Studio 2010\Projects\COOS Router\COOS Messaging\bin\Debug\COOS Messaging.dll");

            
        }

        public object getObject(string name)
        {
            return null;
        }

        public Type loadClass(string typeName)
        {
            // Will get types from currently executing assembley or msorlib.dll
            Type type = Type.GetType(typeName, false, true);


            // If this type is null, try COOS messaging assembley
            if (type == null) 
              type = coosMessagingAssembly.GetType(typeName);


            
            if (type == null)
                logger.error("Can not find type: " + typeName);

            return type;
        }

        public MemoryStream getResource(string resourceName)
        {
            string stringResource;
            MemoryStream ms;

            System.Resources.ResourceSet resourceSet = ResourceCOOS.ResourceManager.GetResourceSet(Thread.CurrentThread.CurrentCulture, true, false);

            // Try getting resource as string
            try
            {
              stringResource = ResourceCOOS.ResourceManager.GetString(resourceName);

               
                Encoding encoding = Encoding.UTF8;
                ms = new MemoryStream(encoding.GetBytes(stringResource));
                logger.info("Using embedded configuration: "+resourceName);
                return ms;
            }
            catch (Exception e)
            {
                logger.error("Error, problems with getting string resource : " + resourceName);
                return null;
            }

            
        }


        public void start()
        {
            

            try
            {
                // Read embedded configuration
                coosInstance = COOSFactory.createCOOS(getResource("coos"), this);
               Plugin[] plugins =  PluginFactory.createPlugins(getResource("pluginPong"), this);

             
                coosInstance.start();
                coosInstance.getRouter().setLoggingEnabled(true);

              
                // Connect plugins
                foreach (Plugin plugin in plugins)
                    plugin.connect();
                
            }
            catch (Exception e)
            {
               logger.info("Could not start COOS instance : " + e.Message+" "+e.TargetSite.ToString());
            }

            // Dont close console window.
            Console.ReadKey(false);

        }

        public void stop()
           
        {
            if (coosInstance != null)
                coosInstance.stop();
        }

    }
}
