#define LOGGING
#define COCONTAINER
#define NET
/**
 * COOS - Connected Objects Operating System (www.connectedobjects.org).
 *
 * Copyright (C) 2009 Telenor ASA and Tellu AS. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 * You may also contact one of the following for additional information:
 * Telenor ASA, Snaroyveien 30, N-1331 Fornebu, Norway (www.telenor.no)
 * Tellu AS, Hagalokkveien 13, N-1383 Asker, Norway (www.tellu.no)
 */


using System.Collections.Generic;
using System;



using Org.Coos.Messaging.Util;


namespace Org.Coos.Messaging.Impl
{
    //import java.util.Enumeration;
    //import java.util.Hashtable;


    //import org.coos.messaging.COContainer;
    //import org.coos.messaging.Processor;
    //import org.coos.messaging.util.Log;
    //import org.coos.messaging.util.LogFactory;


    /**
     * @author Knut Eilif Husa, Tellu AS
     */
    public abstract class DefaultProcessor : IProcessor
    {

        #region Shared
        private bool shared = false;
        public virtual bool isShared()
        {
            return shared;
        }

        public virtual bool Shared
        {
            get { return shared; }
            set { this.shared = value; }
        }
        #endregion


        #region Name
        protected string name;
        public virtual string Name
        {
            get { return name; }
            set { this.name = value; }
        }

        public virtual void setName(string name)
        {
            this.name = name;
        }

        public virtual string getName()
        {
            return this.name;
        }

        #endregion


        protected Dictionary<string,string> properties = new Dictionary<string,string>();

#if COCONTAINER
         protected ICOContainer coContainer;
#endif

#if LOGGING
        private  ILog logger = LogFactory.getLog(typeof(DefaultProcessor).FullName);
#endif


        public virtual void setProperties(Dictionary<string,string> addedProperties)

       {
            
            foreach (string key in  addedProperties.Keys)
                this.properties.Add(key, addedProperties[key]);


        } 
        
        public Dictionary<string,string> Properties
        {
            get { return this.properties; }
        }
        public virtual Dictionary<string,string> getProperties()
        {
            return this.properties;
        }




       
        public virtual void setProperty(string key, string value)
        {
            this.properties.Add(key, value);
#if LOGGING
            logger.info("New property "+key+"="+value);
#endif
        }

        public virtual string getProperty(string key)
        {
            string propertyValue;

            if (this.properties.ContainsKey(key))


                propertyValue = (string) this.properties[key];
             else
               propertyValue = null;


            return propertyValue;

        }

        public virtual string getProperty(string key, string defaultValue)
        {
            // JAVA        String value = (String) this.properties.get(key);

            //        if (value == null)
            //            return defaultValue;

            //        return value;

            if (!this.properties.ContainsKey(key))

                return defaultValue;
            else
                return (string)this.properties[key];
        }

        public virtual void setShared(bool shared)
        {
            this.shared = shared;
        }

#if COCONTAINER
        public void setCoContainer(ICOContainer coContainer) {
            this.coContainer = coContainer;
        }

        public ICOContainer getCoContainer() {
            return this.coContainer;
        }
#endif
        public abstract void processMessage(IMessage msg);
       


        public virtual IProcessor copy()
        {

            IProcessor copyProcessor = null;

            if (shared)
            {
                copyProcessor = this;
            }
            else
            {
                IProcessor processor = null;


                
                // JAVA processor = (IProcessor)this.getClass().newInstance();

                processor = Activator.CreateInstance(this.GetType()) as IProcessor;
               
                processor.setProperties(properties);
                processor.setShared(shared);
                processor.setName(name);
#if COCONTAINER
                    processor.setCoContainer(coContainer);
#endif

                copyProcessor = processor;
            }

            return copyProcessor;
        }

    }
}