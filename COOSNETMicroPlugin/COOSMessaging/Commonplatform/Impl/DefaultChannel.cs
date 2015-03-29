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
//package org.coos.messaging.impl;

//import java.util.Vector;

//import org.coos.messaging.Channel;
//import org.coos.messaging.Link;
//import org.coos.messaging.Connectable;
//import org.coos.messaging.Message;
//import org.coos.messaging.Processor;
//import org.coos.messaging.ProcessorException;
//import org.coos.messaging.Transport;

using Org.Coos.Messaging;
using System.Collections;

namespace Org.Coos.Messaging.Impl
{

    
    ///<author>Knut Eilif Husa, Tellu AS</author>
    public abstract class DefaultChannel : DefaultProcessor, IChannel
    {

        // Messages
        /// <summary>
        /// Message name for connecting channel
        /// </summary>
        public static string CONNECT
        {
            get { return "connect"; }
        }

        ///<summary>
        ///routingalgorithm for connecting segment
        ///</summary>

        public static string ROUTING_ALGORITHM
        {
            get { return "routingAlg"; }
        }
        

        /// <summary>
        /// Message name for ack of connecting channel
        /// </summary>
        public static string CONNECT_ACK
        {
            get { return "connectAck"; }
        }

        /// <summary>
        /// Message name for nack of connecting channel
        /// </summary>
        public static string CONNECT_NACK
        {
            get { return "connectNack"; }
        }

        #region Message parameters



        /// <summary>
        /// Message paramater in message CONNECT indicating the endpoint uuid
        /// </summary>
        public static string CONNECT_UUID
        {
            get { return "con_uuid"; }
        }
       

        /// <summary>
        /// Message paramater in message CONNECT indicating the endpoint segment
        /// </summary>
        public static string CONNECT_SEGMENT
        {
            get { return "con_seg"; }
        }

        /// <summary>
        /// Message paramater in message CONNECT_ACK indicating the allocated endpoint uuid
        /// </summary>
        public static string CONNECT_ALLOCATED_UUID
        {
            get { return "alloc_uuid"; }
        }

        /// <summary>
        /// Message paramater in message CONNECT_ACK indicating the router uuid
        /// </summary>
        public static string CONNECT_ROUTER_UUID
        {
            get { return "router_uuid"; }
        }

        #endregion

        /// <summary>
        /// The inLink, direction into the channel/bus
        /// </summary>
        protected Link inLink;

        /// <summary>
        /// The outLink, direction from the channel/bus
        /// </summary>
        protected Link outLink;

        /// <summary>
        /// The Uuid of the connecting party
        /// </summary>
        protected string connectingPartyUuid;

        /// <summary>
        ///  The Linkmanager that connects the links
        /// </summary>
        protected IConnectable connectable;

        /// <summary>
        ///  Indicates if the Channel shall take action to initialize
        /// </summary>
        protected bool init;

        /// <summary>
        /// The transport mechanism connected to the Channel
        /// </summary>
        protected ITransport transport;

        protected bool connected = false;

        protected ArrayList protocols = new ArrayList();

        protected string segment = "";

        private bool defaultGw = false;

        private bool receiveRoutingInfo = true;

        public DefaultChannel()
        {
            inLink = new Link(this);
            outLink = new Link(this);
        }

        #region InLink
        public virtual Link InLink
        {
            get { return inLink; }
            set
            {
                this.inLink = value;
                this.inLink.setChannel(this);
                this.inLink.setInLink(true);
            }
        }
        
        public virtual Link getInLink()
        {
            return inLink;
        }

        public virtual void setInLink(Link inLink)
        {
            this.inLink = inLink;
            this.inLink.setChannel(this);
            this.inLink.setInLink(true);
        }

        #endregion

        #region OutLink

        public virtual Link OutLink
        {
            get { return outLink; }
            set { this.outLink = value;
                  this.outLink.setChannel(this);
                 this.outLink.setOutLink(true);
            
            }
        }
        
        public virtual Link getOutLink()
        {
            return outLink;
        }

        public virtual void setOutLink(Link outLink)
        {
            this.outLink = outLink;
            this.outLink.setChannel(this);
            this.outLink.setOutLink(true);
        }

        #endregion

        public virtual void connect(IConnectable linkManager)
        {
        }
        

        public virtual void disconnect()
        {
            connectable.removeLinkById(outLink.getLinkId());
        }

        #region LinkManager

        public virtual IConnectable LinkManager
        {
            get { return connectable; }
            set { this.connectable = value; }
        }
        
        public virtual IConnectable getLinkManager()
        {
            return connectable;
        }

        public virtual void setLinkManager(IConnectable linkManager)
        {
            this.connectable = linkManager;
        }

        #endregion

        #region Protocols

        public virtual ArrayList Protocols
        {
            get { return protocols; }
            set { this.protocols = value ; }
        }
        
        public virtual ArrayList getProtocols()
        {
            return protocols;
        }

        public virtual void addProtocol(string protocol)
        {
            protocols.Add(protocol);

        }
        
        public virtual void setProtocols(ArrayList protocols)
        {
            this.protocols = protocols;
        }

        #endregion

        #region Transport
        public virtual ITransport Transport
        {
            get { return transport; }
            set { this.transport = value;
               if (this.transport != null)
               {
                   this.transport.setChannel(this);
              }
                }
        }
        
        public virtual ITransport getTransport()
        {
            return transport;
        }

        public virtual void setTransport(ITransport transport)
        {
            this.transport = transport;

            if (this.transport != null)
            {
                this.transport.setChannel(this);
            }
        }

        #endregion

        public override void processMessage(IMessage msg)
        {
            outLink.processMessage(msg);
        }

        #region Init

        public virtual bool Init
        {
            get { return init; }
            set { this.init = value; }
        }
        
        public virtual bool isInit()
        {
            return init;
        }

        public virtual void setInit(bool init)
        {
            this.init = init;
        }
        #endregion

        public override IProcessor copy()
        {
            IChannel channel;
            channel = (IChannel)base.copy();

            channel.setInLink((Link)inLink.copy());
            channel.setOutLink((Link)outLink.copy());
            channel.setInit(init);

            if (transport != null)
            {
                channel.setTransport((ITransport)transport.copy());
            }

            channel.setProtocols(protocols);
            channel.setSegment(segment);
            channel.setReceiveRoutingInfo(receiveRoutingInfo);
            channel.setDefaultGw(defaultGw);

            return channel;
        }

        #region Segment
        public virtual string Segment
        {
            get { return segment; }
            set { this.segment = value; }
        }
        
        public virtual string getSegment()
        {
            return segment;
        }

        public virtual void setSegment(string segment)
        {
            this.segment = segment;
        }
        #endregion


        #region DefaultGw

        public virtual bool DefaultGw
        {
            get { return defaultGw; }
            set { this.defaultGw = value; }
        }
        
        public virtual bool isDefaultGw()
        {
            return defaultGw;
        }

        public virtual void setDefaultGw(bool defaultGw)
        {
            this.defaultGw = defaultGw;
        }

        #endregion


        #region ReceiveRoutingInfo

        public virtual bool ReceiveRoutingInfo
        {
            get { return receiveRoutingInfo; }
            set { this.receiveRoutingInfo = value; }
        }
        
        public virtual bool isReceiveRoutingInfo()
        {
            return receiveRoutingInfo;
        }

        public virtual void setReceiveRoutingInfo(bool receiveRoutingInfo)
        {
            this.receiveRoutingInfo = receiveRoutingInfo;
        }

        #endregion


        #region Connected

        public virtual bool Connected
        {
            get { return connected; }
            set { this.connected = value; }
        }
        
        public virtual bool isConnected()
        {
            return connected;
        }

        public virtual void setConnected(bool connected)
        {
            this.connected = connected;
        }

        #endregion


    }
}
