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

//#define IMPLEMENT_AFTER_MIGRATION

using System.Collections;
using System.IO;

namespace Org.Coos.Messaging.Impl
{

//import java.io.ByteArrayInputStream;
//import java.io.DataInputStream;

//import java.util.Hashtable;

//import org.coos.messaging.Message;
//import org.coos.messaging.Notification;


/**
 *
 * @author Knut Eilif Husa, Tellu AS
 *
 */
    using System;

    using Org.Coos.Messaging.Util;


public class DefaultNotification : DefaultMessage, INotification {

#region Moved from INotification interface, C# does not allow property definitions in interface

    public static string TYPE_NOTIFICATION
    {
        get { return "notf"; } 
    }

    public static string NOTIFY { 
        get { return "notify"; }
    }

#endregion

    public DefaultNotification() {
        headers.Add(IMessagePrimitives.CONTENT_TYPE, IMessagePrimitives.CONTENT_TYPE_PROPERTY);

        // headers.put(TYPE, TYPE_NOTIFICATION);
        headers.Add(IMessagePrimitives.MESSAGE_NAME, NOTIFY);
        body = new Hashtable();
    }

    public DefaultNotification(IMessage msg) {

        if (!msg.getName().Equals(DefaultNotification.NOTIFY)) {
            throw new ArgumentException("Cannot create DefaultNotification");
        }

        receiverEndpointUri = msg.getReceiverEndpointUri();
        senderEndpointUri = msg.getSenderEndpointUri();
        headers = msg.getHeaders();
        serializedbody = msg.getSerializedBody();
        body = msg.getBody();
    }

  

 
   public DefaultNotification(DataInputStream din)  {
    base.deserialize(din);
    }


    public void putAttribute(string name, bool value) {
#if IMPLEMENT_AFTER_MIGRATION
        ((Hashtable) body).Add(name, new AttributeValue(value));
#endif
    }

    public void putAttribute(string name, string value) {
#if IMPLEMENT_AFTER_MIGRATION
        ((Hashtable) body).Add(name, new AttributeValue(value));
#endif
    }

    public void putAttribute(string name, int value) {
#if IMPLEMENT_AFTER_MIGRATION
        ((Hashtable) body).Add(name, new AttributeValue(value));
#endif
    }

    public void putAttribute(string name, byte[] value) {
#if IMPLEMENT_AFTER_MIGRATION
        ((Hashtable) body).Add(name, new AttributeValue(value));
#endif
    }

    public bool getAttributeAsBoolean(string name) {
#if IMPLEMENT_AFTER_MIGRATION
        return ((AttributeValue) (((Hashtable) body)[name]).booleanValue();
#endif
        return false;
        }

    public string getAttributeAsString(string name) {
#if IMPLEMENT_AFTER_MIGRATION
        return ((AttributeValue) (((Hashtable) body)[name])).stringValue();
#endif
        return "";
        }

    public int getAttributeAsInt(string name) {
#if IMPLEMENT_AFTER_MIGRATION        
        return ((AttributeValue) (((Hashtable) body)[name])).intValue();
#endif
        return 0;
        }

    public byte[] getAttributeAsBytes(string name) {
#if IMPLEMENT_AFTER_MIGRATION
        return ((AttributeValue) (((Hashtable) body)[name])).byteArrayValue();
        
#endif
        byte [] fake = new byte[1];
        fake[0] = 0;
        return fake;
        }


    public override IMessage copy()  {
        MemoryStream bais = new MemoryStream(this.serialize());
        DataInputStream din = new DataInputStream(bais);

        return new DefaultNotification(din);
    }


}}
