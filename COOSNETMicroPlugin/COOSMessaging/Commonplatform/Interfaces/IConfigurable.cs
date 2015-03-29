 using System.Collections;
  
namespace Org.Coos.Messaging
{

   public interface IConfigurable
    {

        Hashtable getProperties();

        /**
         * Sets the properties of this processor
         *
         * @param properties
         *            the properties
         */
        void setProperties(Hashtable properties);

        void setProperty(string key, string value);

       string getProperty(string key);


    }
}
