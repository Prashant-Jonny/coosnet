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

using Org.Coos.Messaging;
using Org.Coos.Messaging.Impl;
using System.Collections;

namespace Org.Coos.Module
{

//import java.util.Hashtable;

//import org.coos.messaging.Exchange;
//import org.coos.messaging.ExchangePattern;
//import org.coos.messaging.Message;
//import org.coos.messaging.impl.DefaultExchange;
//import org.coos.messaging.impl.DefaultMessage;


public class LCMEdgeMessageFactory {

    public static string LCM_REGISTER_ENDPOINT = "lcmRegisterEndpoint";

    public static string LCM_UNREGISTER_ENDPOINT = "lcmUnregisterEndpoint";

    public static string LCM_REGISTER_ENDPOINT_CHILD = "lcmRegisterEndpointChild";

    public static string LCM_UNREGISTER_ENDPOINT_CHILD = "lcmUnregisterEndpointChild";

    public static string LCM_SETSTATE = "lcmSetState";

    public static string LCM_CHILD_SETSTATE = "lcmChildSetState";

    public static string LCM_CHILDREN_SETSTATE = "lcmChildrenSetState";

    public static IExchange createRegisterEndpointExchange(string endpointState, Hashtable childStates, long ms) {
        IExchange ex = new DefaultExchange(new ExchangePattern(ExchangePattern.OutIn));
        DefaultMessage msg = new DefaultMessage(LCM_REGISTER_ENDPOINT);
        Hashtable props = new Hashtable();
        props.Add(LCMMessageProperties.LCM_PROP_STATE, endpointState);
        props.Add(LCMMessageProperties.LCM_PROP_CHILDSTATES, childStates);
        props.Add(LCMMessageProperties.LCM_PROP_POLLING_INTERVAL, ms);
        msg.setBody(props);
        ex.setOutBoundMessage(msg);

        return ex;
    }

    public static IExchange createUnregisterEndpointExchange() {
        IExchange ex = new DefaultExchange(new ExchangePattern(ExchangePattern.OutOnly));
        DefaultMessage msg = new DefaultMessage(LCM_UNREGISTER_ENDPOINT);
        Hashtable props = new Hashtable();
        msg.setBody(props);
        ex.setOutBoundMessage(msg);

        return ex;
    }

    public static IExchange createRegisterEndpointChildExchange(string childName, string childState) {
        IExchange ex = new DefaultExchange(new ExchangePattern(ExchangePattern.OutOnly));
        DefaultMessage msg = new DefaultMessage(LCM_REGISTER_ENDPOINT_CHILD);
        Hashtable props = new Hashtable();
        props.Add(LCMMessageProperties.LCM_PROP_CHILD_ADDRESS, childName);
        props.Add(LCMMessageProperties.LCM_PROP_STATE, childState);
        msg.setBody(props);
        ex.setOutBoundMessage(msg);

        return ex;
    }

    public static IExchange createUnregisterEndpointChildExchange(string childName) {
        IExchange ex = new DefaultExchange(new ExchangePattern(ExchangePattern.OutOnly));
        DefaultMessage msg = new DefaultMessage(LCM_UNREGISTER_ENDPOINT_CHILD);
        Hashtable props = new Hashtable();
        props.Add(LCMMessageProperties.LCM_PROP_CHILD_ADDRESS, childName);
        msg.setBody(props);
        ex.setOutBoundMessage(msg);

        return ex;
    }

    public static IExchange createSetStateExchange(string state) {
        IExchange ex = new DefaultExchange(new ExchangePattern(ExchangePattern.OutOnly));
        DefaultMessage msg = new DefaultMessage(LCM_SETSTATE);
        Hashtable props = new Hashtable();
        props.Add(LCMMessageProperties.LCM_PROP_STATE, state);
        msg.setBody(props);
        ex.setOutBoundMessage(msg);

        return ex;
    }

    public static IExchange createSetChildStateExchange(string childName, string state) {
        IExchange ex = new DefaultExchange(new ExchangePattern(ExchangePattern.OutOnly));
        DefaultMessage msg = new DefaultMessage(LCM_CHILD_SETSTATE);
        Hashtable props = new Hashtable();
        props.Add(LCMMessageProperties.LCM_PROP_STATE, state);
        props.Add(LCMMessageProperties.LCM_PROP_CHILD_ADDRESS, childName);
        msg.setBody(props);
        ex.setOutBoundMessage(msg);

        return ex;
    }

    public static IExchange createSetChildrenStatesExchange(Hashtable childStates) {
        IExchange ex = new DefaultExchange(new ExchangePattern(ExchangePattern.OutOnly));
        IMessage msg = new DefaultMessage(LCM_CHILDREN_SETSTATE);
        Hashtable props = new Hashtable();
        props.Add(LCMMessageProperties.LCM_PROP_CHILDSTATES, childStates);
        msg.setBody(props);
        ex.setOutBoundMessage(msg);

        return ex;
    }

}}
