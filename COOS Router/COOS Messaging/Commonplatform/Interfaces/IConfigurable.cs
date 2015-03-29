#define NET

#if NETMICROFRAMEWORK
using System.Collections;
#endif

#if NET
using System.Collections.Generic;
#endif

namespace Org.Coos.Messaging
{

   public interface IConfigurable
    {

#if NETMICROFRAMEWORK
        Hashtable getProperties();

        /**
         * Sets the properties of this processor
         *
         * @param properties
         *            the properties
         */
        void setProperties(Hashtable properties);
#endif
#if NET
       Dictionary<string,string> getProperties();
       void setProperties(Dictionary<string,string> properties);
#endif

        void setProperty(string key, string value);

       string getProperty(string key);


    }
}
