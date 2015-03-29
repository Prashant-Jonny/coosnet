using System;
using Microsoft.SPOT;
using System.Net.Sockets;
using System.IO;

using org.apache.harmony.luni.util;

namespace Org.Coos.Messaging.Util
{
    public class DataInputStream
    {
        private Stream nStream;

        byte[] buff;

        public DataInputStream(Stream nStream)
        {
            this.nStream = nStream;
            buff = new byte[8];
        }

        public DataInputStream(byte[] buffer)
        {
            nStream = new MemoryStream(buffer);
        }

       /**
     * Reads a 64-bit double value from this stream.
     * 
     * @return the next double value from the source stream.
     * @throws EOFException
     *             if the end of the filtered stream is reached before eight
     *             bytes have been read.
     * @throws IOException
     *             if a problem occurs while reading from this stream.
     * @see DataOutput#writeDouble(double)
     */
    public  double readDouble() /* throws IOException */ {
        long l = readLong();
        byte[] tlong = Reflection.Serialize(l, typeof(long));
        tlong[0] = 205; // Fake double
        double d = (double)Reflection.Deserialize(tlong,typeof(double));
        //Debug.Print("!!!!Warining!!!! Using binary deserialization format that may be incompatible with Java");
        return d;

        
        //JAVA return Double.longBitsToDouble(readLong());
        //throw new NotImplementedException("Cannot read double from data input stream");
    }

    /**
     * Reads a 32-bit float value from this stream.
     * 
     * @return the next float value from the source stream.
     * @throws EOFException
     *             if the end of the filtered stream is reached before four
     *             bytes have been read.
     * @throws IOException
     *             if a problem occurs while reading from this stream.
     * @see DataOutput#writeFloat(float)
     */
    public  float readFloat() /*throws IOException */ {

        int i = readInt();
        byte[] tint = Reflection.Serialize(i, typeof(Int32));
        tint[0] = 204; // Fake float
        float f = (float)Reflection.Deserialize(tint, typeof(float));
        //Debug.Print("!!!!Warning!!!! Using binary deserialization format that may be incompatible with Java");
        return f;

        //return Float.intBitsToFloat(readInt());
       // throw new NotImplementedException("Cannot read float from data input stream");
    }


        // Source: Apache harmony open jdk. 
        private int readToBuff(int count)  {
        int offset = 0;

        while(offset < count) {
            int bytesRead = nStream.Read(buff, offset, count - offset);
            if(bytesRead == -1) return bytesRead; // EOF
            offset += bytesRead;
        } 
        return offset;
    }
        // Source: Apache harmony open jdk.

      public  int readInt()  {
        if (readToBuff(4) < 0){
            throw new Exception("Tried to read a 4 byte int from network stream, but reached end of file");
        }
        return ((buff[0] & 0xff) << 24) | ((buff[1] & 0xff) << 16) |
            ((buff[2] & 0xff) << 8) | (buff[3] & 0xff);
    }

        // Source: Apache harmony open jdk.

        public void readFully(byte[] buffer) {
            readFully(buffer, 0, buffer.Length);
    }

        // Source: Apache harmony open jdk.
        public void readFully(byte[] buffer, int offset, int length)
             {
        if (length < 0) {
            throw new IndexOutOfRangeException("Length of buffer to store network data is below 0");
        }
        if (length == 0) {
            return;
        }
        if (nStream == null) {
            throw new NullReferenceException("Network stream is null"); 
        }
        if (buffer == null) {
            throw new NullReferenceException("Buffer is null, cannot store data"); 
        }
        if (offset < 0 || offset > buffer.Length - length) {
            throw new IndexOutOfRangeException("Offset is below 0, offset is higher than length of buffer");
        }
        while (length > 0) {
            int result = nStream.Read(buffer, offset, length);
            if (result < 0) {
                throw new Exception("End of file reached on network stream");
            }
            offset += result;
            length -= result;
        }
    }

        public int readUnsignedShort()  {
        if (readToBuff(2) < 0){
            throw new Exception("Tried to read an java unsigned short 2-bytes, but could not read it, probably eof");
        }
        return (char) (((buff[0] & 0xff) << 8) | (buff[1] & 0xff));
    }



        public  string readUTF()  {
        return decodeUTF(readUnsignedShort());
    }


    string decodeUTF(int utfSize)  {
        return decodeUTF(utfSize, this);
    }


    /*******************************/
    /// <summary>
    /// Converts an array of bytes to an array of sbytes
    /// </summary>
    /// <param name="sbyteArray">The array of sbytes to be converted</param>
    /// <returns>The new array of bytes</returns>
    public static sbyte[] ToSByteArray(byte[] byteArray)
    {
        sbyte[] sbyteArray = null;

        if (byteArray != null)
        {
            sbyteArray = new sbyte[byteArray.Length];
            for (int index = 0; index < byteArray.Length; index++)
                sbyteArray[index] = (sbyte)byteArray[index];
        }
        return sbyteArray;
    }

    private static string decodeUTF(int utfSize, DataInputStream inStream) {
        byte[] buf = new byte[utfSize];
        char[] outBuf = new char[utfSize];
        inStream.readFully(buf, 0, utfSize);
        
        return org.apache.harmony.luni.util.Util.convertUTF8WithBuf(DataInputStream.ToSByteArray(buf), outBuf, 0, utfSize);
    }

        public byte readByte()
        {
            return (byte)nStream.ReadByte();
        }

       
        /// <summary>
        /// Java boolean 0 = false,  1 = true, by the way .NET bool is 4 bytes
        /// </summary>
        /// <returns></returns>
        public bool readBoolean()
        {
            byte javaBoolean = (byte)nStream.ReadByte();
            if (javaBoolean == (byte)1)
                return true;
            else if (javaBoolean == (byte)0)
                return false;
            else
            {
                throw new Exception("Can not deserialize bool, expected 0 (false) or 1 (true) from network stream "+"got value: "+javaBoolean.ToString());
               
            }
        }


     // Source: Apache harmony

        /**
     * Reads a 64-bit long value from this stream.
     * 
     * @return the next long value from the source stream.
     * @throws EOFException
     *             if the end of the filtered stream is reached before eight
     *             bytes have been read.
     * @throws IOException
     *             if a problem occurs while reading from this stream.
     * @see DataOutput#writeLong(long)
     */
    public  long readLong() /*throws IOException */{
        if (readToBuff(8) < 0){
            throw new System.Exception("EOFException - not enough available bytes to read long data type");
        }
        int i1 = ((buff[0] & 0xff) << 24) | ((buff[1] & 0xff) << 16) |
            ((buff[2] & 0xff) << 8) | (buff[3] & 0xff);
        int i2 = ((buff[4] & 0xff) << 24) | ((buff[5] & 0xff) << 16) |
            ((buff[6] & 0xff) << 8) | (buff[7] & 0xff);

        return ((i1 & 0xffffffffL) << 32) | (i2 & 0xffffffffL);
    }
    }

}
