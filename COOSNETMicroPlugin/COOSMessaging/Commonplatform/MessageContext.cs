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

// Converted get/set-acessors in Java to C# property, hks 15 october 2010

namespace Org.Coos.Messaging
{

   
    ///<summary>The MessageContext contains all information about the processing of the message in this COOS node. It is not serialized and transferred to a remote receiver</summary>
    ///<author>Knut Eilif Husa, Tellu AS</author>
    public class MessageContext
    {
        #region OutBoundLink
        /// <summary>
        /// The outBoundLink holds the Link that processes this message when message is going out of this node (plugin or coos router)
        /// </summary>
        private Link outBoundLink;
        /// <summary>
        /// The outBoundLink holds the Link that processes this message when message is going out of this node (plugin or coos router)
        /// </summary>
        public Link OutBoundLink
        {
            get { return this.outBoundLink; }
            set { this.outBoundLink = value; }
        }

        /// <summary>
        /// The outBoundLink holds the Link that processes this message when message is going out of this node (plugin or coos router)
        /// </summary>
        public Link getOutBoundLink()
        {
            return outBoundLink;
        }
        /// <summary>
        /// The outBoundLink holds the Link that processes this message when message is going out of this node (plugin or coos router)
        /// </summary>
        public void setOutBoundLink(Link outBoundLink)
        {
            this.outBoundLink = outBoundLink;
        }
        #endregion

        #region OutBoundChannel
        private IChannel outBoundChannel;
        /// <summary>
        /// The outBoundChannel holds the Channel that processes this message when message is going out of this node (plugin or coos router)
        /// </summary>
         public IChannel OutBoundChannel
        {
            get { return outBoundChannel; }
            set { this.outBoundChannel = value; }
        }
         /// <summary>
         /// The outBoundChannel holds the Channel that processes this message when message is going out of this node (plugin or coos router)
         /// </summary>
        public IChannel getOutBoundChannel()
        {
            return outBoundChannel;
        }
        /// <summary>
        /// The outBoundChannel holds the Channel that processes this message when message is going out of this node (plugin or coos router)
        /// </summary>
        public void setOutBoundChannel(IChannel outBoundChannel)
        {
            this.outBoundChannel = outBoundChannel;
        }

        #endregion

        #region InBoundLink

        
        private Link inBoundLink;
        /// <summary>
        /// The inBoundLink holds the Link that processes this message when message is going in to this node (plugin or coos router)
        /// </summary>
        public Link InBoundLink
        {
            get { return this.inBoundLink; }
            set { this.inBoundLink = value;}
        }
        /// <summary>
        /// The inBoundLink holds the Link that processes this message when message is going in to this node (plugin or coos router)
        /// </summary>
        public Link getInBoundLink()
        {
            return inBoundLink;
        }
        /// <summary>
        /// The inBoundLink holds the Link that processes this message when message is going in to this node (plugin or coos router)
        /// </summary>
        public void setInBoundLink(Link inBoundLink)
        {
            this.inBoundLink = inBoundLink;
        }

        #endregion

        #region InBoundChannel
        /// <summary>
        ///  The inBoundChannel holds the Channel that processes this message when message is going in to this node (plugin or coos router)
        /// </summary>
      private IChannel inBoundChannel;
      /// <summary>
      ///  The inBoundChannel holds the Channel that processes this message when message is going in to this node (plugin or coos router)
      /// </summary>
        public IChannel InBoundChannel
        {
            get { return inBoundChannel; }
            set { this.inBoundChannel = value; }
        }

        /// <summary>
        ///  The inBoundChannel holds the Channel that processes this message when message is going in to this node (plugin or coos router)
        /// </summary>
        public IChannel getInBoundChannel()
        {
            return inBoundChannel;
        }
        /// <summary>
        ///  The inBoundChannel holds the Channel that processes this message when message is going in to this node (plugin or coos router)
        /// </summary>
        public void setInBoundChannel(IChannel inBoundChannel)
        {
            this.inBoundChannel = inBoundChannel;
        }

        #endregion

        #region CurrentLink
       
        private Link currentLink;
        /// <summary>
        /// The currentLink holds the Link that currently processes this message
        /// </summary>
        public Link CurrentLink
        {
            get { return this.currentLink; }
            set { this.currentLink = value; }
        }

        /// <summary>
        /// The currentLink holds the Link that currently processes this message
        /// </summary>
        public Link getCurrentLink()
        {
            return currentLink;
        }

        /// <summary>
        /// The currentLink holds the Link that currently processes this message
        /// </summary>
        public void setCurrentLink(Link currentLink)
        {
            this.currentLink = currentLink;
        }

        #endregion

        #region CurrentChannel
        
 
        private IChannel currentChannel;
        /// <summary>
        /// The currentChannel holds the Channel that currently processes this message
        /// </summary>
        public IChannel CurrentChannel
        {
            get { return currentChannel; }
            set { this.currentChannel = value; }
        }
        /// <summary>
        /// The currentChannel holds the Channel that currently processes this message
        /// </summary>
        public IChannel getCurrentChannel()
        {
            return currentChannel;
        }
        /// <summary>
        /// The currentChannel holds the Channel that currently processes this message
        /// </summary>
        public void setCurrentChannel(IChannel currentChannel)
        {
            this.currentChannel = currentChannel;
        }

        #endregion

        #region NextLink
         
        
         
        private Link nextLink;
        
        /// <summary>
        /// The nextLink holds the Link that points to the next link to be used. This pointer is used by the router to indicate that a path exists for this message.
        /// </summary>
        public Link NextLink
        {
            get { return this.nextLink; }
            set { this.nextLink = value; }
        }


        public Link getNextLink()
        {
            return nextLink;
        }

        public void setNextLink(Link nextLink)
        {
            this.nextLink = nextLink;
        }
        #endregion


        
    }
}
