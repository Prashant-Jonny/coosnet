

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

//import java.util.Hashtable;

//import org.coos.messaging.*;

using System.Collections.Generic;

namespace Org.Coos.Messaging.Impl
{
    /**
     * @author Knut Eilif Husa, Tellu AS
     */
    public abstract class DefaultProducer : IProducer, IService
    {
        private IEndpoint endpoint;
        private Dictionary<string,string> properties;

        public DefaultProducer(IEndpoint endpoint)
        {
            this.endpoint = endpoint;
        }

        #region Properties


        public virtual Dictionary<string,string> Properties

        {
            get { return properties; }
            set { this.properties = value; }
        }
        
        public virtual void setProperties(Dictionary<string,string> props)
        {
            this.properties = props;
        }

        public virtual Dictionary<string,string> getProperties()
        {
            return properties;
        }

        public virtual string getProperty(string key)
        {
            return (string)properties[key];
        }

        public virtual void setProperty(string key, string value)
        {
            properties.Add(key, value);
        }

        #endregion


        public virtual IEndpoint Endpoint
        {
            get { return endpoint; }
        }
        
        public virtual IEndpoint getEndpoint()
        {
            return endpoint;
        }

        public virtual void start() { }

        public virtual void stop() { }
        
        public virtual IExchange sendMessage(string endpointUri, IMessage msg)
        {
            IExchange exchange = getEndpoint().createExchange(new ExchangePattern(ExchangePattern.OutOnly));
            InteractionHelper helper = new InteractionHelper(getEndpoint());
            exchange.setOutBoundMessage(msg);

            return helper.send(endpointUri, exchange);
        }

        public virtual IExchange sendMessageRobust(string endpointUri, IMessage msg)
        {
            IExchange exchange = getEndpoint().createExchange(new ExchangePattern(ExchangePattern.RobustOutOnly));
            InteractionHelper helper = new InteractionHelper(getEndpoint());
            exchange.setOutBoundMessage(msg);

            return helper.send(endpointUri, exchange);
        }

        public virtual IExchange requestMessage(string endpointUri, IMessage msg)
        {
            IExchange exchange = getEndpoint().createExchange(new ExchangePattern(ExchangePattern.OutIn));
            InteractionHelper helper = new InteractionHelper(getEndpoint());
            exchange.setOutBoundMessage(msg);

            return helper.send(endpointUri, exchange);
        }

        public virtual void requestMessage(string endpointUri, IMessage msg, IAsyncCallback callback)
        {
            IExchange exchange = getEndpoint().createExchange(new ExchangePattern(ExchangePattern.OutIn));
            InteractionHelper helper = new InteractionHelper(getEndpoint());
            exchange.setOutBoundMessage(msg);

            helper.send(endpointUri, exchange, callback);
        }

    }
}
