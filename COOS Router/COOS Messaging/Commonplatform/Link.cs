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

#define LOGGING
#define KEEP_JAVA_GET_SET

using Org.Coos.Messaging.Impl;
using Org.Coos.Messaging.Util;

//using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;

namespace Org.Coos.Messaging
{

    /*
import java.util.Enumeration;
import java.util.Hashtable;
import java.util.Vector;

import org.coos.messaging.impl.DefaultChannel;
import org.coos.messaging.impl.DefaultProcessor;
import org.coos.messaging.util.Log;
import org.coos.messaging.util.LogFactory;
import org.coos.messaging.util.UuidGenerator;
    */
     
    /// <summary>A link defines the processing used on a message</summary>
    /// <author>Knut Eilif Husa, Tellu AS</author>
    public class Link : DefaultProcessor, IChannelProcessor
    
{
       public static string DEFAULT_QOS_CLASS = "defaultQos";
        public static string ALIASES = "aliases";

        private static UuidGenerator linkUuid = new UuidGenerator("link");

        /// <summary>
        /// The uuid of the destination node of this link
        /// </summary>
         private string destinationUuid;
         /// <summary> Returns and sets the destination UUID, i.e. the destination of this Link
         /// </summary>
         /// <returns> the destination UUID
         /// </returns>
         /// <param name="destinationUuid">
         /// </param>
         public string DestinationUuid
         {
             get
             {
                 return destinationUuid;
             }

             set
             {
                 this.destinationUuid = value;
             }

         }

         #region Aliases
         /// <summary>
         /// Aliases of the the destination node of this Link
         /// </summary>
         private LinkedList<string> aliases = new LinkedList<string>();
         /// <summary> Get the aliases     
         /// </summary>
         /// <returns> Vector containing all aliases
         /// </returns>
         public LinkedList<string> Alises
         {
             get
             {
                 return aliases;
             }

         }
#endregion

         #region FilterProcessors
         /// <summary>
         /// The processors that runs on the messages
        /// </summary>
        private List<IProcessor> filterProcessors = new List<IProcessor>();
        /// <summary> Get a filter linkProcessor
        /// </summary>
        /// <param name="processor">the filter linkProcessors on this link
        /// </param>
        public List<IProcessor> FilterProcessors
        {
            get
            {
                return filterProcessors;
            }

        }
#endregion

        #region LinkProcessor
        /// <summary>
        /// The processor that this link calls after all the filters have been executed
         /// </summary>
         private IProcessor linkProcessor;
         /// <summary> Get the linkProcessor that the link ends into,
         /// 
         /// </summary>
         /// <returns> the link end linkProcessor
         /// </returns>
         public IProcessor LinkProcessor
         {
             get
             {
                 return linkProcessor;
             }

         }
        #endregion
         /// <summary> Set the linkProcessor that the link ends into,
         /// 
         /// </summary>
         /// <param name="processor">the link end linkProcessor
         /// </param>
         public IProcessor ChainedProcessor
         {
             set
             {
                 this.linkProcessor = value;
             }

         }

        /// <summary>
         ///The costmap of this link
        /// </summary>
         private ConcurrentDictionary<string,int> costMap = new ConcurrentDictionary<string,int>();

        /// <summary>
         /// The original costmap of this link
        /// </summary>
         private Dictionary<string,int> origCostMap;

         #region Channel
         /// <summary>
         /// The owning channel of this link.
        /// </summary>
         private IChannel channel;
         public IChannel Channel
         {
             get { return channel; }
             set { this.channel = value; }
         }

#endregion

         #region LinkId
         /// <summary>
         /// The uuid of this link
        /// </summary>
         private string linkId;

         /// <summary> Returns the unique link id </summary>
         /// <returns> the link id</returns>
         public string LinkId
         {
             get
             {
                 return linkId;
             }

         }
#endregion

   
         private bool inLink = true;
        

        

        #region CostMap
        /// <summary> Returns the CostMap of this link
        /// 
        /// </summary>
        /// <returns> the costMap
        /// </returns>
        public ConcurrentDictionary<string,int> CostMap
        {
            get
            {
                return costMap;
            }

        }
        #endregion

        #region Cost
        /// <summary> Gets the default (if present) cost of a link
        /// </summary>
        /// <returns> the cost
        /// </returns>
        public int Cost
        {
            get { 
                if (costMap.ContainsKey(DEFAULT_QOS_CLASS)) 
                   return (int)costMap[DEFAULT_QOS_CLASS];
              else
                return 0;
            }
            
        }
        #endregion

#if LOGGING
        protected ILog log = LogFactory.getLog("Link");
#endif

         public Link() {
             linkId = linkUuid.generateId();
            // costMap.Add(DEFAULT_QOS_CLASS, (int)0);
         }

         public Link(IChannel channel) : this()
         {
            
             costMap.TryAdd(DEFAULT_QOS_CLASS, (int)0);
             this.channel = channel;
         }

         public Link(int cost) : this() {
           
          
          costMap.TryAdd(DEFAULT_QOS_CLASS,cost);
         }

         public Link(string uuid, int cost) : this() {
          
             this.destinationUuid = uuid;
           
             costMap.TryAdd(DEFAULT_QOS_CLASS, cost);
         }
#if KEEP_JAVA_GET_SET
        #region InLink
         public bool InLink
         {
             get { return inLink; }
             set { this.inLink = value; }
         }
        
         public void setInLink(bool inLink)
         {
             this.inLink = inLink;
         }

         public bool isInLink()
         {
             return inLink;
         }
#endregion

         #region OutLink
         public bool OutLink
         {
             get { return !inLink; }
             set { this.inLink = !value; }
         }
          
        public void setOutLink(bool outLink)
         {
             this.inLink = !outLink;
         }

         public bool isOutLink()
         {
             return !inLink;
         }
#endregion
#endif
         /// <summary> Add linkProcessor that act as filter, i.e. a linkProcessor that can
         /// inspect the message and pass it on to the next linkProcessor Pattern is
         /// pipes and filter 
         /// </summary>
         /// <param name="processor">the filter linkProcessor to be added
         /// </param>
         public void addFilterProcessor(IProcessor processor)
         {

             if (processor != null)
             {
                
                 filterProcessors.Add(processor);
             }
         }

         /// <summary> remove a filter linkProcessor
         /// </summary>
         /// <param name="processor">the filter linkProcessor to be removed
         /// </param>
	     public void removeFilterProcessor(IProcessor processor)
         {
             
             filterProcessors.Remove(processor);
         }

#if KEEP_JAVA_GET_SET
         public List<IProcessor> getFilterProcessors()
         {
             return filterProcessors;
         }

         /// <summary> Get the linkProcessor that the link ends into,
         /// 
         /// </summary>
         /// <returns> the link end linkProcessor
         /// </returns>
         public IProcessor getLinkedProcessor()
         {
             return linkProcessor;
         }

         /// <summary> Set the linkProcessor that the link ends into,
         /// 
         /// </summary>
         /// <param name="processor">the link end linkProcessor
         /// </param>
         public void setChainedProcessor(IProcessor processor)
         {
             this.linkProcessor = processor;
         }
#endif
         
        /// <summary>
        /// Add a new alias
        /// </summary>
        /// <param name="alias">new alias</param>
        public void addAlias(string alias)
         {

             lock (aliases)
             {

                 if (!aliases.Contains(alias))
                 {
                     aliases.AddLast(alias);
                 }
             }
         }

         
         ///<summary>Removes an alias</summary>
         ///<param name="alias">alias to be removed</param>
         public void removeAlias(string alias)
         {

             lock (aliases)
             {
                 aliases.Remove(alias);
             }
         }

         ///<summary>Remove all aliases</summary>
         public void removeAllAliases()
         {

             lock (aliases)
             {
                 aliases.Clear();
             }
         }

#if KEEP_JAVA_GET_SET
         /// <summary> Get the aliases     
         /// </summary>
         /// <returns> Vector containing all aliases
         /// </returns>
         public LinkedList<string> getAlises()
         {
             return aliases;
         }

         /// <summary> Returns the unique link id
         /// 
         /// </summary>
         /// <returns> the link id
         /// </returns>
         public string getLinkId()
         {
             return linkId;
         }

         /// <summary> Returns the Channel this Link belongs to
         /// 
         /// </summary>
         /// <returns> the Channel
         /// </returns>
	     public IChannel getChannel()
         {
             return channel;
         }

         /// <summary> Sets the owning channel of this Link</summary>
	     public void setChannel(IChannel channel) {
        //     this.channel = channel;
         }
#endif

         /// <summary> Processing of the message
         /// 
         /// </summary>
         /// <param name="msg">
         /// </param>
	    public override void processMessage(IMessage msg)  {

         if (channel != null) {
            
              msg.getMessageContext().setCurrentChannel(channel);
         }

         msg.getMessageContext().setCurrentLink(this);


             if (inLink) {

                 // Set inbound channel
                 if (channel != null) {
                     msg.getMessageContext().setInBoundChannel(channel);
                     if(!channel.isConnected()){
#if LOGGING
           
                         log.debug("Cannot process message in inLink since channel is not connected, Msg:" + msg);
#endif                         
                         throw new ProcessorException("Cannot process message in InLink since Channel :"+getName()+" is not connected");
                     }
                 }

                 msg.getMessageContext().setInBoundLink(this);
             }
             else
             {

                 // Set inbound channel
                 if (channel != null)
                 {
                     msg.getMessageContext().setOutBoundChannel(channel);
                     if (!channel.isConnected())
                     {
#if LOGGING
                         log.debug("Cannot process message in OutLink since channel is not connected, Msg:" + msg);
#endif
                         throw new ProcessorException("Cannot process message in OutLink since Channel :" + getName() + " is not connected");
                     }
                 }

                 msg.getMessageContext().setOutBoundLink(this);

             }

            // Run message through filter processors

            foreach (IProcessor processor in filterProcessors) {

            //for (int i = 0; i < filterProcessors.Count; i++)
            //{
              //  IProcessor processor = (IProcessor)filterProcessors[i];

                try
                {
                    processor.processMessage(msg);
                }
                catch (ProcessorInterruptException e)
                {
#if LOGGING
                    log.debug("Interrupted processing of message:" + e.Message);
#endif
                    return;
                }
            }

            linkProcessor.processMessage(msg);
         }

        /// <summary> Starting any linkProcessor that is a service
        /// 
        /// </summary>
        /// <throws>  Exception </throws>
	     public void start()  {

             foreach (IProcessor processor in filterProcessors) 
             //for (int i = 0; i < filterProcessors.Count; i++) {
                 //IProcessor processor = (IProcessor) filterProcessors[i];

                 if (processor is IService) 
                     ((IService) processor).start();
                 
             

             //The first time this Link starts we save the costMap in origCostMap.
             //If the Link is started at any later time the original costMap is restored
             if (origCostMap == null) {
                 origCostMap = new Dictionary<string,int>();
                
                 //IEnumerator iekeys = costMap.Keys.GetEnumerator();
                 //IEnumerator ievalues = costMap.Values.GetEnumerator();
                 //bool morekeys;

                 foreach (string key in costMap.Keys)
                     origCostMap.Add(key, (int)costMap[key]);

                 //if (costMap.Keys.Count > 0)
                 //    do
                 //    {

                 //        origCostMap.Add((string)iekeys.Current, (int)ievalues.Current);
                 //        morekeys = iekeys.MoveNext();
                 //        ievalues.MoveNext();
                 //    } while (morekeys);

                 
                 //Enumeration enumer = costMap.keys();
                 
                 

                 //while (enumer.hasMoreElements()) {
                 //    String key = (String) enumer.nextElement();
                 //    origCostMap.put(key, costMap.get(key));
                 //}
             } else {
                 //IEnumerator iekeys = origCostMap.Keys.GetEnumerator();
                 //IEnumerator ievalues = origCostMap.Values.GetEnumerator();
                 //bool morekeys;

                 costMap = new ConcurrentDictionary<string,int>();

                 foreach (string key in origCostMap.Keys)
                     costMap.TryAdd(key, (int)origCostMap[key]);
                     
                 //if (origCostMap.Keys.Count > 0)
                 //    do
                 //    {

                 //        costMap.Add((string)iekeys.Current, (int)ievalues.Current);
                 //        morekeys = iekeys.MoveNext();
                 //        ievalues.MoveNext();
                 //    } while (morekeys);

                 //costMap = new Hashtable();

                 //Enumeration enumer = origCostMap.keys();

                 //while (enumer.hasMoreElements()) {
                 //    String key = (String) enumer.nextElement();
                 //    costMap.put(key, origCostMap.get(key));
                 //}
             }
         }

         /// <summary> stopping any linkProcessor that is a service
         /// 
         /// </summary>
         /// <throws>  Exception </throws>
         public void stop()  {

             foreach (IProcessor processor in filterProcessors)
                 if (processor is IService)
                     ((IService)processor).stop();

             //for (int i = 0; i < filterProcessors.Count; i++) {
             //    IProcessor processor = (IProcessor) filterProcessors[i];

             //    if (processor is IService) {
             //        ((IService) processor).stop();
             //    }
             //}
         }

#if KEEP_JAVA_GET_SET
        /**
         * Gets the default (if present) cost of a link
         *
         * @return the cost
         */
        public int getCost()
        {
            if (costMap.ContainsKey(DEFAULT_QOS_CLASS))
                return (int) costMap[DEFAULT_QOS_CLASS];
            else
                return 0;

            
        }
#endif
        /**
         * Gets the link cost of a link based on a qos class
         *
         * @param qosClass
         *            the qosClass
         * @return the cost
         */
        public int getCost(string qosClass)
        {
        
            if (costMap.ContainsKey(qosClass))
                return (int)costMap[qosClass];
            else
                return 0;
        }

        /**
         * Sets the default cost
         *
         * @param cost
         *            the cost
         */
        public void setCost(int cost)
        {
            costMap.TryAdd(DEFAULT_QOS_CLASS,cost);
        }

        /**
         * Gets the cost based on qosClass
         *
         * @param qosClass
         *            the qosClass
         * @param cost
         *            the cost
         */
        public void setCost(string qosClass, int cost)
        {
            costMap.TryAdd(qosClass, cost);
        }

#if KEEP_JAVA_GET_SET
        /**
         * Returns the CostMap of this link
         *
         * @return the costMap
         */
        public ConcurrentDictionary<string,int> getCostMap()
        {
            return costMap;
        }


        /**
         * Returns the destination UUID, i.e. the destination of this Link
         *
         * @return the destination UUID
         */
        public string getDestinationUuid()
        {
            return destinationUuid;
        }

        /**
         * Sets the destination UUID, i.e. the destination of this Link
         *
         * @param destinationUuid
         */
        public void setDestinationUuid(string destinationUuid)
        {
            this.destinationUuid = destinationUuid;
        }
#endif

        public override string ToString()
        {

            if (linkProcessor != null)
            {
                string s = "Link to " + linkProcessor.ToString() + ", destUUID: " + destinationUuid + ", aliases:";
                foreach (string alias in aliases)
                    s += " " + alias;
                return s;
            }

            return "No link processor";
        }

        /**
         * Returns a copy of the Link
         *
         * @return the Link
         */
        public override IProcessor copy()
        {
            Link copyLink = new Link();

            // copy filter processors

            foreach (IProcessor processor in filterProcessors)
                copyLink.addFilterProcessor(processor.copy());

            //for (int i = 0; i < filterProcessors.size(); i++)
            //{
            //    Processor processor = (Processor)filterProcessors.elementAt(i);
            //    copy.addFilterProcessor(processor.copy());
            //}

            // copy costmap

           // Enumeration enumer = costMap.keys();

            foreach (string key in costMap.Keys)
                copyLink.setCost(key, (int)costMap[key]);

            //while (enumer.hasMoreElements())
            //{
            //    String key = (String)enumer.nextElement();
            //    copy.setCost(new String(key), ((Integer)costMap.get(key)).intValue());
            //}

            return copyLink;
        }


         public int hashCode() {
             const int prime = 31;
             int result = 1;
             result = (prime * result) + ((linkId == null) ? 0 : linkId.GetHashCode());

             return result;
         }


         //public bool equals(object obj)
         //{

         //    if (this == obj)
         //        return true;

         //    if (obj == null)
         //        return false;

            
         //    if (getClass() != obj.getClass())
         //        return false;

         //    Link other = (Link)obj;

         //    if (linkId == null)
         //    {

         //        if (other.linkId != null)
         //            return false;
         //    }
         //    else if (!linkId.equals(other.linkId))
         //        return false;

         //    return true;
         //}


    }
}
