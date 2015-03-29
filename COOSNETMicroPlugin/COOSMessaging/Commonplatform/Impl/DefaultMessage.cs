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
using System.Collections;
using System.IO;
using System.Text;

using Org.Coos.Messaging.Util;
using Org.Coos.Util.Serialize;

using org.apache.harmony.luni.util;

//#define IMPLEMENT_AFTER_MIGRATION


namespace Org.Coos.Messaging.Impl
{

    //import java.io.ByteArrayInputStream;
    //import java.io.ByteArrayOutputStream;
    //import java.io.DataInputStream;
    //import java.io.DataOutputStream;
    //import java.io.Serializable;

    //import java.util.Enumeration;
    //import java.util.Hashtable;

    //import org.coos.messaging.Message;
    //import org.coos.messaging.MessageContext;
    //import org.coos.messaging.Serializer;
    //import org.coos.messaging.SerializerFactory;
    //import org.coos.messaging.util.Log;
    //import org.coos.messaging.util.LogFactory;
    //import org.coos.util.serialize.AFClassLoader;


    /**
     * @author Knut Eilif Husa, Tellu AS
     */

    // Java serialization : http://java.sun.com/developer/technicalArticles/Programming/serialization/

    // JAVA public class DefaultMessage : IMessage, Serializable
public class DefaultMessage : IMessage
{

    #region ReceiverEndpointUri
    protected string receiverEndpointUri;
    public virtual string ReceiverEndpointUri
    {
        get { return receiverEndpointUri; }
//        set { this.receiverEndpointUri = value; }
    }


    public virtual IMessage setReceiverEndpointUri(string receiverEndpointUri)
    {
        this.receiverEndpointUri = receiverEndpointUri;
        return this;
    }

    public virtual string getReceiverEndpointUri()
    {
        return receiverEndpointUri;
    }

    #endregion

    #region SenderEndpointUri
    protected string senderEndpointUri;
    public virtual string SenderEndpointUri
    {
        get { return senderEndpointUri; }
        set { this.senderEndpointUri = value; }
    }

    public virtual string getSenderEndpointUri()
    {
        return senderEndpointUri;
    }

    public virtual IMessage setSenderEndpointUri(string senderEndpointUri)
    {
        this.senderEndpointUri = senderEndpointUri;
        return this;
    }

    
    #endregion

    #region Headers
    protected Hashtable headers = new Hashtable();
    public virtual Hashtable Headers
    {
        get { return headers; }
    }


    public virtual string getHeader(String key)
    {
        return (string)headers[key];
    }

    public virtual IMessage setHeader(String key, String value)
    {
        if (!headers.Contains(key))
            headers.Add(key, value);
        return this;
    }

    public virtual Hashtable getHeaders()
    {
        return headers;
    }
    #endregion

    #region Body

    protected Object body;
    public virtual object Body
    {
        get
        {
            try
            {
                deserializeBody();
            }
            catch (Exception e)
            {
                // logger.error("Unknown Exception caught", e);
            }

            return body;
        }
    }
    public virtual byte[] BodyAsBytes
    {
        get
        {
            try
            {
                deserializeBody();
            }
            catch (Exception e)
            {
                // logger.error("Unknown Exception caught", e);
            }

            return (byte[])body;
        }
    }
   
    public virtual string BodyAsString
    {
        get
        {
            try
            {
                deserializeBody();
            }
            catch (Exception e)
            {
                // logger.error("Unknown Exception caught", e);
            }

            return (string)body;
        }
    }
    public virtual Hashtable BodyAsProperties
    {
        get
        {
            try
            {
                deserializeBody();
            }
            catch (Exception e)
            {
                // logger.error("Unknown Exception caught", e);
            }

            return (Hashtable)body;
        }
    }


	protected byte[] serializedbody;

    #endregion

    #region MessageContext
    protected  MessageContext messageContext = new MessageContext();
    public virtual MessageContext Messagecontext
    {
        get { return messageContext; }
    }

    public virtual MessageContext getMessageContext()
    {
        return messageContext;
    }

    public virtual void setMessageContext(MessageContext ctx)
    {

        this.messageContext = ctx;

    }


    #endregion

    #region Version
   
    protected byte version;
    
    public virtual byte Version
    {
        get { return version; }
    }

    public virtual byte getVersion()
    {
        return version;
    }

    public virtual void setVersion(byte version)
    {
        this.version = version;
    }

    #endregion
    
    #region ContentType

    public virtual string ContentType
    {
        get { return (string)headers[IMessagePrimitives.CONTENT_TYPE]; }
    }



    public virtual string getContentType()
    {
        return (string)headers[IMessagePrimitives.CONTENT_TYPE];
    }

       
    #endregion

    #region Name
    public virtual string Name
    {
        get { return (string)headers[IMessagePrimitives.MESSAGE_NAME]; }
    }

    public virtual string getName()
    {
        return (string)headers[IMessagePrimitives.MESSAGE_NAME];
    }

    #endregion

    #region Type
    public virtual string Type
    {
        get { return (string)headers[IMessagePrimitives.TYPE]; }
    }

    public virtual string getType()
    {
        return (string)headers[IMessagePrimitives.TYPE];
    }

    #endregion

    #region ReceiverEndpointName
    public virtual string ReceiverEndpointName
    {
        get { return (string)headers[IMessagePrimitives.RECEIVER_ENDPOINT_NAME]; }
    }

    public virtual string getReceiverEndpointName()
    {
        return (string)headers[IMessagePrimitives.RECEIVER_ENDPOINT_NAME];
    }

    public virtual void setReceiverEndpointName(string endpointName)
    {
        if (endpointName != null)
        {
            if (!headers.Contains(IMessagePrimitives.RECEIVER_ENDPOINT_NAME))
              headers.Add(IMessagePrimitives.RECEIVER_ENDPOINT_NAME, endpointName);
        }
    }

    #endregion

    #region SenderEndpointName
    public virtual string SenderEndpointName
    {
        get { return (string)headers[IMessagePrimitives.SENDER_ENDPOINT_NAME]; }
    }


    public virtual string getSenderEndpointName()
    {
        return (string)headers[IMessagePrimitives.SENDER_ENDPOINT_NAME];
    }


    public virtual void setSenderEndpointName(string endpointName)
    {
        if (endpointName != null)
        {
            if (!headers.Contains(IMessagePrimitives.SENDER_ENDPOINT_NAME))
            headers.Add(IMessagePrimitives.SENDER_ENDPOINT_NAME, endpointName);
        }
    }


    #endregion

    private AFClassLoader serializeCl;

        public DefaultMessage()
        {
            setHeader(IMessagePrimitives.MESSAGE_NAME, IMessagePrimitives.DEFAULT_MESSAGE_NAME);
            setHeader(IMessagePrimitives.TYPE, IMessagePrimitives.TYPE_MSG);
        }

        /// <summary>
        /// Signal can be f.ex. CONNECT_ACK from router
        /// </summary>
        /// <param name="signalName"></param>
        public DefaultMessage(string signalName) 

        {
            setHeader(IMessagePrimitives.MESSAGE_NAME, signalName);
            setHeader(IMessagePrimitives.TYPE, IMessagePrimitives.TYPE_MSG);
        }

        public DefaultMessage(string signalName, string type) 
        {
            setHeader(IMessagePrimitives.MESSAGE_NAME, signalName);
            setHeader(IMessagePrimitives.TYPE, type);

        }


        /// <summary>
        /// Method is called from transport layer, i.e. TCPTransport when a message (socket bytes) is
        /// received from router
        /// </summary>
        /// <param name="din"></param>
        public DefaultMessage(DataInputStream din)  {
            deserialize(din);
        }




        public void setDeserializeClassLoader(AFClassLoader cl)
        {
            this.serializeCl = cl;
        }




        public virtual byte[] getBodyAsBytes()
        {

            try
            {
                deserializeBody();
            }
            catch (Exception e)
            {
                //logger.error("Unknown Exception caught", e);
            }

            return (byte[])body;
        }

        //@SuppressWarnings("unchecked")
        public virtual Hashtable getBodyAsProperties()
        {

            try
            {
                deserializeBody();
            }
            catch (Exception e)
            {
                // logger.error("Unknown Exception caught", e);
            }

            return (Hashtable)body;
        }

        public virtual string getBodyAsString()
        {

            try
            {
                deserializeBody();
            }
            catch (Exception e)
            {
                //logger.error("Unknown Exception caught", e);
            }

            return (String)body;
        }

        /// <summary>
        /// Deserialize body
        /// </summary>
        /// <returns>Deserialized body</returns>
        public virtual object getBody()
        {

            try
            {
                deserializeBody();
            }
            catch (Exception e)
            {
                // logger.error("Unknown Exception caught", e);
            }

            return body;
        }

        /// <summary>
        /// Deserialize msg. from network
        /// </summary>
        /// <param name="din"></param>
        public virtual void deserialize(DataInputStream din)
        {
            
          int msgLength =  din.readInt(); // Length of message

            version = din.readByte();

            // Receiver URI
            if (din.readBoolean()) {
                receiverEndpointUri = din.readUTF();
            }

            // Sender URI
            if (din.readBoolean()) {
                senderEndpointUri = din.readUTF();
            }

            // Header
            
            int headerSize = din.readInt();

            for (int i = 0; i < headerSize; i++) {
                String key = din.readUTF();
                String value = din.readUTF();
                headers.Add(key, value);
            }

            // Body

            int length = din.readInt();

            if (length == 0) {
                return;
            }

            serializedbody = new byte[length]; // Setup buffer to hold body
            din.readFully(serializedbody); // Reads network data into buffer
        }

        private  void deserializeBody()
        {
                 Encoding utf8 = Encoding.UTF8;


            if ((body == null) && (serializedbody != null) && (serializedbody.Length > 0)) {
                string contentType = (string)headers[IMessagePrimitives.CONTENT_TYPE];
                if(contentType == null){
                     throw new Exception("No content type indicated in message header");
                }
                if (contentType.Equals(IMessagePrimitives.CONTENT_TYPE_BYTES)) {
                    body = serializedbody;
                } else if (contentType.Equals(IMessagePrimitives.CONTENT_TYPE_STRING)) {
                    // JAVA body = new String(serializedbody, "UTF-8");
                     
                    body = org.apache.harmony.luni.util.Util.convertFromUTF8(DataInputStream.ToSByteArray(serializedbody), 0, serializedbody.Length);
                   
                    // .NET UTF-8 deserialization char[] utf8chars = utf8.GetChars(netserializedbody);
                    //body = utf8chars.ToString();
                } else {

                    string serMethod = (string)headers[IMessagePrimitives.SERIALIZATION_METHOD];

                    if (serMethod != null) {
                        ISerializer serializer = SerializerFactory.getSerializer(serMethod);
                        body = serializer.deserialize(DataOutputStream.ToSByteArray(serializedbody), serializeCl);
                    } else {
                        throw new Exception("No serialization method indicated in message header");
                    }

                }
            }
        }

 /// <summary>
 /// Serialize message
 /// </summary>
 /// <returns></returns>
        public virtual byte[] serialize()
        {

           // Message format:
           //
           // Length - 4 byte
           // Version - 1 byte
           // ReceiverEndpointUriflag - 1 byte
           //   optional:receveierEndpointUri - n bytes
           // SenderEndpointUriflag - 1 byte 
           //   optional:senderendpointUri - n bytes
           // header size - 4 bytes
           //   key - n bytes
           //   value - n bytes 
           // body size - ? bytes
           //   serializedbody -n bytes

            // Info  : http://java.sun.com/developer/technicalArticles/Intl/Supplementary/

            // Found no BitConverter nor BinaryWriter in .NET Micro Framework BCL v4.1
           

            MemoryStream byteMessageStream = new MemoryStream();
           // StreamWriter sw = new StreamWriter(byteMessageStream); // Hook up stream to memorystream
            DataOutputStream dataOverByteMessageStream = new DataOutputStream(byteMessageStream);

            Encoding utf8 = Encoding.UTF8;
            
            // Make a temporary array to find the size of the message.
            // ByteArrayOutputStream bouth = new ByteArrayOutputStream();
            //DataOutputStream douth = new DataOutputStream(bouth);

            //douth.writeByte(version);

            // Version
           
            byteMessageStream.WriteByte(version);
           
            // Receiver endpoint URI
            
            dataOverByteMessageStream.writeBoolean(receiverEndpointUri != null); // mandatory
            dataOverByteMessageStream.writeEndpointUriJava(receiverEndpointUri); // optional
            
          
            // Sender endpoint URI

            dataOverByteMessageStream.writeBoolean(senderEndpointUri != null); // mandatory
            dataOverByteMessageStream.writeEndpointUriJava(senderEndpointUri); // optional

            //// The body

            if ((body != null) && (serializedbody == null))
            {

                // Byte

                if (body is byte[])
                {
                    // C# exception "key exists" if DefaultMessage.setBody called before running serialize
                    string key = IMessagePrimitives.CONTENT_TYPE;
                    if (!headers.Contains(key))
                        headers.Add(key, IMessagePrimitives.CONTENT_TYPE_BYTES);
                    serializedbody = (byte[])body;
                }

                // String

                else if (body is string)
                {
                    string key = IMessagePrimitives.CONTENT_TYPE;
                    if (!headers.Contains(key))
                        headers.Add(IMessagePrimitives.CONTENT_TYPE, IMessagePrimitives.CONTENT_TYPE_STRING);

                    //byte[] NETserializedbody;

                    //NETserializedbody = utf8.GetBytes(body as string);

                    serializedbody = dataOverByteMessageStream.getUTFBytes(body as string);
                }



                 // Hashtable
                else
                {

                    if (body is Hashtable)
                    {
                        if (!headers.Contains(IMessagePrimitives.CONTENT_TYPE))
                            headers.Add(IMessagePrimitives.CONTENT_TYPE, IMessagePrimitives.CONTENT_TYPE_PROPERTY);
                    }
                    else
                    {
                        if (!headers.Contains(IMessagePrimitives.CONTENT_TYPE))
                          headers.Add(IMessagePrimitives.CONTENT_TYPE, IMessagePrimitives.CONTENT_TYPE_OBJECT);
                    }

                    String serMethod = headers[IMessagePrimitives.SERIALIZATION_METHOD] as string;

                    if (serMethod != null)
                    {
                        ISerializer serializer = SerializerFactory.getSerializer(serMethod);

                        if (serializer != null)
                        {
                            serializedbody = DataOutputStream.ToByteArray(serializer.serialize(body));
                        }
                        else
                        {
                            throw new Exception("Serialization method not registered: " + serMethod);
                        }
                    }
                    else
                    {

                        try
                        {
                            ISerializer serializer = SerializerFactory.getDefaultSerializer();
                            serializedbody = DataOutputStream.ToByteArray(serializer.serialize(body));
                            headers.Add(IMessagePrimitives.SERIALIZATION_METHOD, IMessagePrimitives.SERIALIZATION_METHOD_DEFAULT);
                        }
                        catch (Exception e)
                        {
                            ISerializer serializer = SerializerFactory.getSerializer(IMessagePrimitives.SERIALIZATION_METHOD_JAVA);

                            if (serializer != null)
                            {
                                serializedbody = DataOutputStream.ToByteArray(serializer.serialize(body));
                                headers.Add(IMessagePrimitives.SERIALIZATION_METHOD, IMessagePrimitives.SERIALIZATION_METHOD_JAVA);
                            }
                            else
                            {
                                throw new Exception("Serialization failed");
                            }
                        }
                    }
                }

            }
            


            //// The headers
            
            
            dataOverByteMessageStream.writeInt(headers.Count);

            foreach (string key in headers.Keys)
            {
                dataOverByteMessageStream.writeUTF(key);
                dataOverByteMessageStream.writeUTF((string)headers[key]);
            }

            
            //// The body

            if (serializedbody != null)
            {
                dataOverByteMessageStream.writeInt(serializedbody.Length);
                byteMessageStream.Write(serializedbody,0,serializedbody.Length);
            }
            else
            {
                dataOverByteMessageStream.writeInt(0); // Empty body
            }

            //ByteArrayOutputStream bout = new ByteArrayOutputStream();
            //DataOutputStream dout = new DataOutputStream(bout);

            //byte[] body = bouth.toByteArray();

            //// Write length and then the body, Nio peeks at the msg length.

            //dout.writeInt(body.length);
            //dout.write(body);

            //return bout.toByteArray();
           
            // Add length of message to beginning
            long msgLength = byteMessageStream.Length;
            MemoryStream msgWithLength = new MemoryStream();
            msgWithLength.Write(DataOutputStream.intToByteArray((int)msgLength), 0, 4);
            msgWithLength.Write(byteMessageStream.ToArray(), 0, (int)msgLength);
            return msgWithLength.ToArray();
        }
     
        public virtual  IMessage copy()  {
           
            MemoryStream bais = new MemoryStream(this.serialize());
            DataInputStream din = new DataInputStream(bais);

            return new DefaultMessage(din);
        }

       

        public override string ToString() {

            string buf = "Receiver:" +receiverEndpointUri+", Sender: "+senderEndpointUri;
            foreach (string key in headers.Keys)
                buf += ", "+key+":"+headers[key].ToString();

            return buf;
        }

  
        public virtual byte[] getSerializedBody()
        {
            return serializedbody;
        }

        public virtual void setSerializedBody(byte[] body)
        {
          if (!headers.Contains(IMessagePrimitives.CONTENT_TYPE))
            headers.Add(IMessagePrimitives.CONTENT_TYPE,IMessagePrimitives.CONTENT_TYPE_BYTES);
            serializedbody = body;
        }


        public virtual IMessage setBody(byte[] bytesBody)
        {
            if (!headers.Contains(IMessagePrimitives.CONTENT_TYPE))
                headers.Add(IMessagePrimitives.CONTENT_TYPE, IMessagePrimitives.CONTENT_TYPE_BYTES);
            body = bytesBody;
            return this;
            
        }

        public virtual IMessage setBody(Hashtable propertyBody)
        {
            if (!headers.Contains(IMessagePrimitives.CONTENT_TYPE))
                headers.Add(IMessagePrimitives.CONTENT_TYPE, IMessagePrimitives.CONTENT_TYPE_PROPERTY);
            body = propertyBody;
            return this;

            
        }


        public virtual IMessage setBody(string stringBody)
        {
            if (!headers.Contains(IMessagePrimitives.CONTENT_TYPE))
                headers.Add(IMessagePrimitives.CONTENT_TYPE, IMessagePrimitives.CONTENT_TYPE_STRING);
            body = stringBody;
            return this;
           
        }

        public virtual IMessage setBody(object objectBody)
        {
           

           if (!headers.Contains(IMessagePrimitives.CONTENT_TYPE))
             headers.Add(IMessagePrimitives.CONTENT_TYPE, IMessagePrimitives.CONTENT_TYPE_OBJECT);
           
            body = objectBody;
            return this;
        }


     
       

    }
}
