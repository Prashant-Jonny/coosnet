

using System;
using Microsoft.SPOT;
using System.Collections.Generics;
using System.Text ;
using System.IO;
using System.Runtime.Serialization
using System.Runtime.Serialization.Formatters.Binary ;
using System.Collections;
using System.Net;
using System.Net.Sockets ;

// COOS
using Org.Coos.Messaging;

namespace NTNUMasterThesis2010
{


   

public class DefaultMessage : IMessage
{
  

/// </summary>
protected String receiverEndpointUri ;
protected String senderEndpointUri ;
protected Hashtable headers = new Hashtable ( ) ;
protected Object body ;
protected byte [] serializedbody ;
public static TcpClient client;
public static MemoryStream bout ;
public static BinaryWriter w;
public static StringBuilder reply = new StringBuilder( ) ;

//    I n t e r f a c e i n t e r = new I n t e r f a c e ( ) ;
//i n t e r . ShowDialog ( ) ;
//}

public static void sendConnectMessage ( )
{
   I

dout.Write ( ( bool ) false ) ;
MemoryStream bout = new MemoryStream ( ) ;
BinaryWriter dout = new BinaryWriter ( bout ) ;
dout .Write ( ( byte ) 0 ) ;
dout .Write ( ( bool ) false ) ;
dout .Write ( ( bool ) false ) ;
dout .Write ( ( int ) IPAddress.HostToNetworkOrder ( 3 ) ) ;
DefaultMessage.WriteUTF( dout , " type " ) ;
DefaultMessage.WriteUTF( dout , "msg " ) ;
DefaultMessage.WriteUTF( dout , "name " ) ;
DefaultMessage.WriteUTF( dout , " connect " ) ;
DefaultMessage.WriteUTF( dout , " con_uuid " ) ;
DefaultMessage.WriteUTF( dout , " .UUID−1234");
dout .Write ( ( int ) IPAddress.HostToNetworkOrder ( 0 ) ) ;
MemoryStream bouth = new MemoryStream ( ) ;
   
    BinaryWriter douth = new BinaryWriter ( bouth ) ;
byte [ ] payload = bout . ToArray ( ) ;
int length = payload.Length ;
douth.Write ( ( int ) ( IPAddress.HostToNetworkOrder ( length ) ) ) ;
douth.Write ( payload ) ;
byte [ ] serial = bouth.ToArray ( ) ;
douth.Flush ( ) ;
sendAndReceive ( serial ) ;
}

public static void sendMessage ( string mess )
{

MemoryStream bout = new MemoryStream ( ) ;
BinaryWriter dout = new BinaryWriter ( bout ) ;
dout .Write ( ( byte ) 0 ) ;
dout .Write ( ( bool ) t rue ) ;
DefaultMessage.WriteUTF( dout , " coos : / / myownartifact " ) ;
dout .Write ( ( bool ) t rue ) ;
DefaultMessage .WriteUTF( dout , " coos : / / Ping " ) ;
dout .Write ( ( i n t ) IPAddress . HostToNetworkOrder ( 6 ) ) ;
DefaultMessage.WriteUTF( dout , " type " ) ;
DefaultMessage.WriteUTF( dout , "msg " ) ;
DefaultMessage.WriteUTF( dout , "name " ) ;
DefaultMessage.WriteUTF( dout , " t e s t " ) ;
DefaultMessage.WriteUTF( dout , " xId " ) ;
DefaultMessage.WriteUTF( dout , " 1 2 3 4 " ) ;
DefaultMessage.WriteUTF( dout , " xpattern " ) ;
DefaultMessage.WriteUTF( dout , "OutIn " ) ;
DefaultMessage.WriteUTF( dout , " contentType " ) ;
DefaultMessage.WriteUTF( dout , " s t r i n g " ) ;
DefaultMessage.WriteUTF( dout , " s e r " ) ;
DefaultMessage.WriteUTF( dout , " de f " ) ;
int length = mess . Length ;
dout .Write ( ( Int16 ) IPAddress.HostToNetworkOrder (length ) ) ;
DefaultMessage .WriteUTF( dout , mess ) ;
MemoryStream bouth = new MemoryStream ( ) ;
BinaryWriter douth = new BinaryWriter ( bouth ) ;
byte [ ] payload = bout . ToArray ( ) ;
int length = payload.Length ;
douth .Write ( ( int ) ( IPAddress.HostToNetworkOrder ( leng th ) ) ) ;
douth .Write ( payload ) ;
byte [ ] serial = bouth . ToArray ( ) ;

douth . Flush ( ) ;
sendAndReceive ( serial ) ;
}


public static void sendByte ( byte b)
{
MemoryStream bout = new MemoryStream ( ) ;
BinaryWriter dout = new BinaryWriter ( bout ) ;
dout .Write ( ( byte ) 0 ) ;
dout .Write ( ( bool ) t rue ) ;
DefaultMessage.WriteUTF( dout , " coos : / / myownartifact " ) ;
dout .Write ( ( bool ) t rue ) ;
DefaultMessage.WriteUTF( dout , " coos : / / Ping " ) ;
dout .Write ( ( i n t ) IPAddress . HostToNetworkOrder ( 5 ) ) ;
DefaultMessage.WriteUTF( dout , " type " ) ;
DefaultMessage.WriteUTF( dout , "msg " ) ;
DefaultMessage.WriteUTF( dout , "name " ) ;
DefaultMessage.WriteUTF( dout , " Ping ? " ) ;
DefaultMessage.WriteUTF( dout , " xId " ) ;
DefaultMessage.WriteUTF( dout , " 1 2 3 4 " ) ;
DefaultMessage.WriteUTF( dout , " xPattern " ) ;
DefaultMessage.WriteUTF( dout , "OutOnly " ) ;
DefaultMessage.WriteUTF( dout , " contentType " ) ;
DefaultMessage.WriteUTF( dout , " byte " ) ;
dout .Write ( ( int ) IPAddress.HostToNetworkOrder ( 1 ) ) ;
dout .Write ( ( byte )b ) ;
MemoryStream bouth = new MemoryStream ( ) ;
BinaryWriter douth = new BinaryWriter ( bouth ) ;
byte [ ] payload = bout . ToArray ( ) ;
int length = payload . Length ;
douth .Write ( ( i n t ) ( IPAddress.HostToNetworkOrder ( l eng th ) ) ) ;
douth .Write ( payload ) ;
byte [ ] serial = bouth.ToArray ( ) ;
douth . Flush ( ) ;
sendAndReceive ( serial ) ;
}


static void sendAndReceive ( byte [ ] serial )
{
try
{
Console.WriteLine ( " Connected to the Server...." ) ;
c .Write ( serial, 0 , serial.Length ) ;
}
Console.WriteLine ( " Data sent to the Server.... " ) ;
try
{
byte [ ] serial1 = new byte [ 1 5 0 ] ;
DefaultMessage msg1 = new DefaultMessage ( reader ) ;
}catch ( EndOfStreamException e )
{
Console.WriteLine ( e.Message ) ;
}
}
catch ( SocketException e )
{
Console.WriteLine ( e.Message ) ;
}
}

public DefaultMessage ( )
{
setHeader (MESSAGE_NAME, DEFAULT_MESSAGE_NAME) ;
setHeader (TYPE, TYPE_MSG) ;
}

public DefaultMessage ( String signalName )
{
setHeader (MESSAGE_NAME, signalName ) ;
setHeader (TYPE, TYPE_MSG) ;
}

public DefaultMessage ( St r ing signalName , St r ing type )
{
setHeader (MESSAGE_NAME, signalName ) ;
setHeader (TYPE, type ) ;
}

public DefaultMessage ( BinaryReader din )
{
d e s e r i a l i z e ( din ) ;
}

public String getReceiverEndpointUri ( )
{return receiverEndpointUri;
}

public Message setReceiverEndpointUri ( String receiverEndpointUri) {
this.receive rEndpointUr i = receiverEndpointUri ;
return t h i s ;
}


public void deserialize( BinaryReader dinR)
{
int k=0;
Console . WriteLine ( " De s e r i a l i z a t i o n St a r t ing . . . . . . " ) ;
NetworkStream ms = (NetworkStream) dinR . BaseStream ;
int j = IPAddress . HostToNetworkOrder ( dinR . ReadInt32 ( ) ) ;
repl y .Append ( " Message Si z e " ) ;
repl y .Append( j ) ;
repl y .Append( System . Environment . NewLine ) ;
byte v e r s i on = dinR . ReadByte ( ) ;
repl y .Append ( " Version " ) ;
repl y .Append( v e r s i on ) ;
repl y .Append( System . Environment . NewLine ) ;
Console . WriteLine ( " Payload Si z e "+ j ) ;
Console . WriteLine ( " Version " + v e r s i on ) ;
if ( dinR . ReadBoolean ( ) )
{
dinR . ReadByte ( ) ;
receive rEndpointUri = dinR.ReadString ( ) ;
reply.Append ( " Re c e ive r EndPoint URI " ) ;
reply.Append( r e c e ive rEndpo intUr i ) ;
reply.Append( System . Environment . NewLine ) ;
}
if ( dinR.ReadBoolean ( ) )
{
dinR.ReadByte ( ) ;
senderEndpointUri = dinR . ReadString ( ) ;
reply.Append ( " Sender EndPoint URI " ) ;
reply.Append( senderEndpointUri ) ;
reply .Append( System . Environment . NewLine ) ;


Console.WriteLine ( " Header Si z e "+ he ade rSi z e ) ;
reply.Append ( " Header Si z e " ) ;
reply.Append( he ade rSi z e ) ;
reply .Append( System . Environment . NewLine ) ;
for ( int i = 0 ; i < headerSize ; i++)
{
dinR.ReadByte ( ) ;
String key = dinR.ReadString ( ) ;
Console.WriteLine ( " Key " + key ) ;
reply.Append ( " Key " ) ;
reply.Append( key ) ;
dinR.ReadByte ( ) ;
String value = dinR . ReadString ( ) ;
Console.WriteLine ( " Value "+value ) ;
reply.Append ( " Value " ) ;
reply.Append( value ) ;
if ( ! ( headers.Contains ( key ) ) )
headers.Add( key , value ) ;
reply.Append( System . Environment . NewLine ) ;
}

serializedbody = new byte [ IPAddress . HostToNetworkOrder ( dinR . ReadInt32 Console . WriteLine ( "Message Body Length "+ s e r i a l i z e d b o d y . Length r epl y .Append ( " Message Body Length " ) ;
reply.Append( serializedbody.Length ) ;
reply.Append( System . Environment . NewLine ) ;
if ( serializedbody.Length == 0)
{
return ;
}
dinR.Read ( serializedbody , 0 , serializedbody.Length ) ;
}


private void deserializeBody ( )
{
if ( body == null && serializedbody != null && serializedbody.Length {
String serMethod = headers [SERIALIZATION_METHOD].ToString ( ) ;
if ( serMethod != null )
{
Serializerserializer = SerializerFactory.getSerializer( serMethod

body = s e r i a l i z e r . d e s e r i a l i z e ( s e r i a l i z e d b o d y ) ;
}
e l s e
{
throw new Exception ( "No s e r i a l i z a t i o n method i n d i c a t e d in message }
}
}
publ i c byte [ ] s e r i a l i z e ( )
{
MemoryStream bout = new MemoryStream ( ) ;
BinaryWriter dout = new BinaryWriter ( bout ) ;
dout .Write ( ( Byte ) 1 ) ;
// The addr e s s e s
dout .Write ( ( Boolean ) ( r e c e ive rEndpo intUr i != n u l l ) ) ;
i f ( r e c e ive rEndpo intUr i != n u l l )
{
WriteUTF( dout , r e c e ive rEndpo intUr i ) ;
}
dout .Write ( ( Boolean ) ( senderEndpointUri != n u l l ) ) ;
i f ( senderEndpointUri != n u l l )
{W
riteUTF( dout , senderEndpointUri ) ;
}
// The body
i f ( body != n u l l && s e r i a l i z e d b o d y == n u l l )
{
// headers .Add(CONTENT_TYPE, CONTENT_TYPE_STRING) ;
St r ing serMethod = " de f " ;
i f ( serMethod != n u l l )
{
S e r i a l i z e r s e r i a l i z e r = S e r i a l i z e rFa c t o r y . g e t S e r i a l i z e r ( serMethod i f ( s e r i a l i z e r != n u l l )
{
s e r i a l i z e d b o d y = s e r i a l i z e r . s e r i a l i z e ( body ) ;
}
e l s e
{
throw new Exception ( " S e r i a l i z a t i o n method not r e g i s t e r e d : " + }
}



e l s e
{
t ry
{
S e r i a l i z e r s e r i a l i z e r = S e r i a l i z e rFa c t o r y . g e tDe f a u l t S e r i a l i z e r s e r i a l i z e d b o d y = s e r i a l i z e r . s e r i a l i z e ( body ) ;
headers .Add(SERIALIZATION_METHOD, SERIALIZATION_METHOD_DEFAULT) }
catch ( Exception e )
{
S e r i a l i z e r s e r i a l i z e r = S e r i a l i z e rFa c t o r y . g e t S e r i a l i z e r (SERIALIZATION_i f ( s e r i a l i z e r != n u l l )
{s
e r i a l i z e d b o d y = s e r i a l i z e r . s e r i a l i z e ( body ) ;
headers .Add(SERIALIZATION_METHOD, SERIALIZATION_METHOD_JAVA) ;
}
e l s e
{
throw new Exception ( " S e r i a l i z a t i o n f a i l e d " ) ;
}
}
}
// The headers
dout .Write ( ( i n t ) headers . Count ) ;
dout . Flush ( ) ;
IDictionaryEnumerator en = headers . GetEnumerator ( ) ;
whi l e ( en .MoveNext ( ) )
{
dout .Write ( ( St r ing ) en .Key ) ;
dout .Write ( ( St r ing ) en . Value ) ;
}
dout . Flush ( ) ;
// The body
i f ( s e r i a l i z e d b o d y != n u l l )
{
dout .Write ( ( i n t ) s e r i a l i z e d b o d y . Length ) ;
dout .Write ( s e r i a l i z e d b o d y ) ;
}
e l s e
{
dout .Write ( ( i n t ) 0 ) ;

}
St r ing temp = dout . ToString ( ) ;
Console . WriteLine ( temp ) ;
r e turn bout . ToArray ( ) ;
}
}