/**
 * COOS - Connected objects Operating System (www.connectedobjects.org).
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
/*package org.coos.messaging;

import java.io.DataInputStream;

import java.util.Hashtable;

import org.coos.util.serialize.AFClassLoader;
*/

using System.Collections;

using Org.Coos.Util.Serialize;

namespace Org.Coos.Messaging
{

/**
 * @author Knut Eilif Husa, Tellu AS
 * The message that is transported on the bus
 */
public interface IMessage {

   

   

    string getReceiverEndpointUri();

    IMessage setReceiverEndpointUri(string endpointUri);

    string getSenderEndpointUri();

    IMessage setSenderEndpointUri(string endpointUri);

    string getHeader(string key);

    string getType();

    string getContentType();
    #region Name
    /*
     * The name of the message.
     */
    string Name { get; }

    string getName();

    #endregion
    void setSenderEndpointName(string endpointName);

    string getSenderEndpointName();

    void setReceiverEndpointName(string endpointName);

    string getReceiverEndpointName();

    IMessage setHeader(string key, string value);

    Hashtable getHeaders();

    /**
     * Returns the message context of the message.
     *
     * @return
     */
    MessageContext getMessageContext();

    void setMessageContext(MessageContext ctx);

    IMessage setBody(byte[] byteBody);

    IMessage setBody(Hashtable propertyBody);

    IMessage setBody(string stringBody);

    IMessage setBody(object objectBody);

    byte[] getBodyAsBytes();

    Hashtable getBodyAsProperties();

    string getBodyAsString();

    object getBody();

    byte[] getSerializedBody();

    void setSerializedBody(byte[] body);

    // JAVA void deserialize(DataInputStream din) throws Exception;
    //void deserialize(DataInputStream din);

    // JAVA byte[] serialize() throws Exception;
    byte[] serialize();

    
    IMessage copy();

    byte getVersion();

    void setVersion(byte version);

    void setDeserializeClassLoader(AFClassLoader cl);
}
}