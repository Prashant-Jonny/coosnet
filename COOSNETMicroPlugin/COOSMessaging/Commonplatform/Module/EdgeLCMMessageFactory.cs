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
//package org.coos.module;

//import java.util.Hashtable;
//import org.coos.messaging.Exchange;
//import org.coos.messaging.ExchangePattern;
//import org.coos.messaging.Message;
//import org.coos.messaging.impl.DefaultExchange;
//import org.coos.messaging.impl.DefaultMessage;

using Org.Coos.Messaging;
using Org.Coos.Messaging.Impl;
using System.Collections;

namespace Org.Coos.Module
{

    public class EdgeLCMMessageFactory
    {

        public static string EDGE_REQUEST_STATE = "edgeRequestState";

        public static string EDGE_REQUEST_CHILD_STATE = "edgeRequestChildState";

        public static string EDGE_REQUEST_CHILDREN = "edgeRequestChildren";

        public static string EDGE_UPGRADE = "edgeUpgrade";

        public static string EDGE_START = "edgeStart";

        public static string EDGE_PAUSE = "edgePause";

        public static string EDGE_STOP = "edgeStop";

        public static IExchange createRequestStateExchange()
        {
            IExchange ex = new DefaultExchange(new ExchangePattern(ExchangePattern.OutOnly));
            IMessage msg = new DefaultMessage(EDGE_REQUEST_STATE);
            Hashtable props = new Hashtable();
            msg.setBody(props);
            ex.setOutBoundMessage(msg);
            return ex;
        }

        public static IExchange createRequestChildStateExchange(string childName)
        {
            IExchange ex = new DefaultExchange(new ExchangePattern(ExchangePattern.OutOnly));
            IMessage msg = new DefaultMessage(EDGE_REQUEST_CHILD_STATE);
            Hashtable props = new Hashtable();
            props.Add(EdgeMessageProperties.EDGE_PROP_CHILD_NAME, childName);
            msg.setBody(props);
            ex.setOutBoundMessage(msg);
            return ex;
        }

        public static IExchange createRequestChildrenExchange()
        {
            IExchange ex = new DefaultExchange(new ExchangePattern(ExchangePattern.OutOnly));
            IMessage msg = new DefaultMessage(EDGE_REQUEST_CHILDREN);
            Hashtable props = new Hashtable();
            msg.setBody(props);
            ex.setOutBoundMessage(msg);
            return ex;
        }

        public static IExchange createStartExchange(string endpoint)
        {
            IExchange ex = new DefaultExchange(new ExchangePattern(ExchangePattern.OutOnly));
            IMessage msg = new DefaultMessage(EDGE_START);
            Hashtable props = new Hashtable();
            props.Add(EdgeMessageProperties.EDGE_PROP_ENDPOINT_NAME, endpoint);
            msg.setBody(props);
            ex.setOutBoundMessage(msg);
            return ex;
        }

        public static IExchange createStopExchange(string endpoint)
        {
            IExchange ex = new DefaultExchange(new ExchangePattern(ExchangePattern.OutOnly));
            IMessage msg = new DefaultMessage(EDGE_STOP);
            Hashtable props = new Hashtable();
            props.Add(EdgeMessageProperties.EDGE_PROP_ENDPOINT_NAME, endpoint);
            msg.setBody(props);
            ex.setOutBoundMessage(msg);
            return ex;
        }

        public static IExchange createPauseExchange(string endpoint)
        {
            IExchange ex = new DefaultExchange(new ExchangePattern(ExchangePattern.OutOnly));
            IMessage msg = new DefaultMessage(EDGE_PAUSE);
            Hashtable props = new Hashtable();
            props.Add(EdgeMessageProperties.EDGE_PROP_ENDPOINT_NAME, endpoint);
            msg.setBody(props);
            ex.setOutBoundMessage(msg);
            return ex;
        }

        public static IExchange createUpgradeExchange(string url)
        {
            IExchange ex = new DefaultExchange(new ExchangePattern(ExchangePattern.OutIn));
            IMessage msg = new DefaultMessage(EDGE_UPGRADE);
            Hashtable props = new Hashtable();
            props.Add(EdgeMessageProperties.EDGE_PROP_BUNDLE_URL, url);
            msg.setBody(props);
            ex.setOutBoundMessage(msg);
            return ex;
        }

    }
}
