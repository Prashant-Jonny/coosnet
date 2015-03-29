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

using System;
using Org.Coos.Messaging;

namespace Org.Coos.Messaging.Impl
{

    ///<summary>
    /// Class with base implementation of Exchange interface.
    ///</summary>
    ///<author>Knut Eilif Husa, Tellu AS</author>
    public class DefaultExchange : IExchange
    {

        private IMessage outBoundMessage;
        private IMessage inBoundMessage;
        private IMessage fault;
        private Exception exception;
        private ExchangePattern pattern;
        private string exchangeId;
        private bool isProc = false;

        public DefaultExchange(ExchangePattern pattern)
        {
            this.pattern = pattern;
        }

        public DefaultExchange()
        {

        }

        #region Pattern
        public virtual ExchangePattern Pattern
        {
            get { return pattern; }
            set { this.pattern = value; }
        }

        public virtual void setPattern(ExchangePattern pattern)
        {
            this.pattern = pattern;
        }

        public virtual ExchangePattern getPattern()
        {
            return pattern;
        }
        #endregion

        #region OutBoundMessage/InBoundMessage

        public virtual IMessage OutBoundMessage
        {
            get { return outBoundMessage; }
            set { outBoundMessage = value; }
        }
        
        public virtual IMessage getOutBoundMessage()
        {
            return outBoundMessage;
        }

        public virtual IMessage InBoundMessage
        {
            get { return inBoundMessage; }
            set { inBoundMessage = value; }
        }
        
        public virtual IMessage getInBoundMessage()
        {
            return inBoundMessage;
        }

        public virtual void setOutBoundMessage(IMessage outboundMsg)
        {
            this.outBoundMessage = outboundMsg;
        }

        public virtual void setInBoundMessage(IMessage inboundMsg)
        {
            this.inBoundMessage = inboundMsg;
        }

        #endregion

        #region ExchangeId

        public virtual string ExchangeId
        {
            get { return exchangeId; }
            set { exchangeId = value; }
        }
        
        public virtual string getExchangeId()
        {
            return exchangeId;
        }

        public virtual void setExchangeId(string exchangeId)
        {
            this.exchangeId = exchangeId;
        }

        #endregion

        #region FaultMessage

        public virtual IMessage FaultMessage
        {
            get { return fault; }
            set { fault = value; }
        }
        

        
        public virtual IMessage getFaultMessage()
        {
            return fault;
        }

        public virtual void setFaultMessage(IMessage fault)
        {
            this.fault = fault;
        }

        #endregion

        #region Exception

        public virtual Exception Exception
        {
            get { return exception; }
            set { exception = value; }
        }

        public virtual Exception getException()
        {
            return exception;
        }

        public virtual void setException(Exception exception)
        {
            this.exception = exception;
        }

        #endregion

        #region ToString/ToLongString
        public override string  ToString()
        {
            return ("ExchangeID: " + exchangeId);
        }

        public string toLongString()
        {
           
            string buf = "ExchangeID: " + exchangeId + ", ExchangePattern: " + pattern + ", OutboundMsg: " + outBoundMessage + ", InboundMsg:" + inBoundMessage;


            if (fault != null)
            {
                buf += ", FaultMsg: "+fault.ToString();
            }

            return buf;


        }

        #endregion

        #region Processed
        
        public virtual bool Processed
        { 
            get { return this.isProc; }
            set{ this.isProc = value; }
        }

        public virtual void setProcessed(bool isProcessed)
        {
            this.isProc = isProcessed;

        }

       
        public virtual bool isProcessed()
        {
            return this.isProc;
        }

        #endregion
    }
}
