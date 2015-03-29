using System;
using Microsoft.SPOT;
using System.IO;
using System.Text;

using java.io;

namespace Org.Coos.Messaging.Util
{
    // Helper class for Java to .NET integration, inspired for Java DataOutputStream
    
    public class DataOutputStream
    {

        Encoding encodeUTF8 = UTF8Encoding.UTF8;

        private Stream dos;

        /**
     * The number of bytes written out so far.
     */
      // protected int written;
       byte []buff;

        public DataOutputStream(Stream dos)
        {
            this.dos = dos;
            this.buff = new byte[8];
        }

        public virtual void flush()
        {
            dos.Flush();
        }

        /// <summary>
        /// Follow Java naming convention for methods in DataOutputStream, writes a byte to dataoutputstream
        /// </summary>
        /// <param name="b"></param>
        public void writeByte(byte b)
        {
           // Debug.GC(true);
            dos.WriteByte(b);
           
            //Debug.Print("Wrote byte "+b.ToString());
        }

        /// <summary>
        /// .NET bool is 4 bytes vs. Java boolean is 1 byte
        /// </summary>
        /// <param name="value"></param>
        public void writeBoolean(bool value)
        {
            if (value)
                dos.WriteByte(1);
            else
                dos.WriteByte(0);
          
        }

        public void writeEndpointUriNET(string endpointUri)
        {
            if (endpointUri != null)
            {
                // JAVA douth.writeUTF(receiverEndpointUri);
                byte[] endpointUriBytes = encodeUTF8.GetBytes(endpointUri);
                dos.Write(endpointUriBytes, 0, endpointUriBytes.Length);
               
              
            }

        }

        public void writeEndpointUriJava(string endpointUri)
        {
            if (endpointUri != null)
            {
                // JAVA douth.writeUTF(receiverEndpointUri);
               // byte[] endpointUriBytes = encodeUTF8.GetBytes(endpointUri);
               // ms.Write(endpointUriBytes, 0, endpointUriBytes.Length);
                writeJAVAUTF(endpointUri);
            }

        }

        public void writeInt(int integer)
        {
            byte[] intBytes = intToByteArray(integer);
            dos.Write(intBytes, 0, intBytes.Length);
            
        }

        public void writeNETUTF(string toUTF8)
        {
            byte[] utf8 = encodeUTF8.GetBytes(toUTF8);
            dos.Write(utf8, 0, utf8.Length);
           

        }

        /// <summary>
        /// From : http://snippets.dzone.com/posts/show/93
        /// Accessed : 19 oct. 2010
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static public byte[] intToByteArray(int value) {
                return new byte[] { 
                                        (byte)(value >> 24), 
                                        (byte)(value >> 16 & 0xff), 
                                        (byte)(value >> 8 & 0xff), 
                                        (byte)(value & 0xff) };
}

        public byte[] getUTFBytes(string str)
        {
            return getUTFBytesJava(str);
        }

        public  byte [] getUTFBytesJava(string str)
        {
            long utfCount = countUTFBytes(str);
            if (utfCount > 65535)
            {
                throw new UTFDataFormatException("String that will be encoded exceeds limit of 65535 bytes");
            }
            sbyte[] buffer = new sbyte[(int)utfCount];
            int offset = 0;
          //  offset = writeShortToBuffer((int)utfCount, buffer, offset);
            offset = writeUTFBytesToBuffer(str, (int)utfCount, buffer, offset);

            return DataOutputStream.ToByteArray(buffer);

        }

        /// <summary>
        ///  Default to Java MUTF format
        /// </summary>
        /// <param name="str"></param>
        public void writeUTF(string str)
        {
            writeJAVAUTF(str);
        }

        // Source: Apache harmony
        public void writeJAVAUTF(System.String str)
        {
            long utfCount = countUTFBytes(str);
            if (utfCount > 65535)
            {
                throw new UTFDataFormatException("String that will be encoded exceeds limit of 65535 bytes"); 
            }
            sbyte[] buffer = new sbyte[(int)utfCount + 2];
            int offset = 0;
            offset = writeShortToBuffer((int)utfCount, buffer, offset);
            offset = writeUTFBytesToBuffer(str, (int)utfCount, buffer, offset);
            write(buffer, 0, offset);
           
        }

        // Source: Java Languagage Conversion Assistant v 3.0 support file
        /*******************************/
        /// <summary>
        /// Converts an array of sbytes to an array of bytes
        /// </summary>
        /// <param name="sbyteArray">The array of sbytes to be converted</param>
        /// <returns>The new array of bytes</returns>
        public static byte[] ToByteArray(sbyte[] sbyteArray)
        {
            byte[] byteArray = null;

            if (sbyteArray != null)
            {
                byteArray = new byte[sbyteArray.Length];
                for (int index = 0; index < sbyteArray.Length; index++)
                    byteArray[index] = (byte)sbyteArray[index];
            }
            return byteArray;
        }

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

        public virtual void write(sbyte[] buffer)
        {
            this.write(buffer, 0, buffer.Length);
        }

        // Source: Apache harmony
        public virtual void write(sbyte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("Buffer not defined from writing to stream"); 
            }

            dos.Write(DataOutputStream.ToByteArray(buffer), offset, count);
            //out_Renamed.write(buffer, offset, count);
            //written += count;
        }

        // Source: Apache harmony
        internal virtual long countUTFBytes(System.String str)
        {
            int utfCount = 0, length = str.Length;
            for (int i = 0; i < length; i++)
            {
                int charValue = str[i];
                if (charValue > 0 && charValue <= 127)
                {
                    utfCount++;
                }
                else if (charValue <= 2047)
                {
                    utfCount += 2;
                }
                else
                {
                    utfCount += 3;
                }
            }
            return utfCount;
        }

        // Source: Apache harmony
        internal virtual int writeUTFBytesToBuffer(System.String str, long count, sbyte[] buffer, int offset)
        {
            int length = str.Length;
            for (int i = 0; i < length; i++)
            {
                int charValue = str[i];
                if (charValue > 0 && charValue <= 127)
                {
                    buffer[offset++] = (sbyte)charValue;
                }
                else if (charValue <= 2047)
                {
                    buffer[offset++] = (sbyte)(0xc0 | (0x1f & (charValue >> 6)));
                    buffer[offset++] = (sbyte)(0x80 | (0x3f & charValue));
                }
                else
                {
                    buffer[offset++] = (sbyte)(0xe0 | (0x0f & (charValue >> 12)));
                    buffer[offset++] = (sbyte)(0x80 | (0x3f & (charValue >> 6)));
                    buffer[offset++] = (sbyte)(0x80 | (0x3f & charValue));
                }
            }
            return offset;
        }

        // Source: Apache harmony
        internal virtual int writeShortToBuffer(int val, sbyte[] buffer, int offset)
        {
            buffer[offset++] = (sbyte)(val >> 8);
            buffer[offset++] = (sbyte)val;
            return offset;
        }

        /// <summary>
        /// Java write-method, wrapper around .NET Stream Write
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void write(byte[] buff, int offset, int count)
        {
            dos.Write(buff, offset, count);
        }

        // Source: Apache harmony
        public void writeLong(long val) /*throws IOException */ {
        buff[0] = (byte) (val >> 56);
        buff[1] = (byte) (val >> 48);
        buff[2] = (byte) (val >> 40);
        buff[3] = (byte) (val >> 32);
        buff[4] = (byte) (val >> 24);
        buff[5] = (byte) (val >> 16);
        buff[6] = (byte) (val >> 8);
        buff[7] = (byte) val;
        dos.Write(buff, 0, 8);
        
    }


        // Source: Apache harmony

        /**
     * Writes a 64-bit double to the target stream. The resulting output is the
     * eight bytes resulting from calling Double.doubleToLongBits().
     * 
     * @param val
     *            the double to write to the target stream.
     * @throws IOException
     *             if an error occurs while writing to the target stream.
     * @see DataInputStream#readDouble()
     */
    public  void writeDouble(double val) /*throws IOException */ {

        byte[] tdouble = Reflection.Serialize(val, typeof(double));
        tdouble[0] = 202; // Fake long type
        long tlong = (long)Reflection.Deserialize(tdouble, typeof(long));

        writeLong(tlong);
        //Debug.Print("!!!WARNING!!!! Using double binary format that may be incompatible with Java binary representation");
       // JAVA  writeLong(Double.doubleToLongBits(val));
       // throw new Exception("Double cannot yet be written to dataoutputstream");
  
    }

        // Source: Apache harmony
    /**
     * Writes a 32-bit float to the target stream. The resulting output is the
     * four bytes resulting from calling Float.floatToIntBits().
     * 
     * @param val
     *            the float to write to the target stream.
     * @throws IOException
     *             if an error occurs while writing to the target stream.
     * @see DataInputStream#readFloat()
     */
    public  void writeFloat(float val) /*throws IOException */ {

        // Might work??
        // Analysis: seems like first byte is a "type", 200=int32, 204=float, 202=long, 205 = double

        byte[] tfloat = Reflection.Serialize(val, typeof(float));
        
        tfloat[0] = 200; // Fake type float to be Int32
        Int32 i = (Int32) Reflection.Deserialize(tfloat, typeof(Int32));
        writeInt(i);
        //Debug.Print("!!!WARNING!!!! Using float binary format that may be incompatible with Java binary representation");

       // JAVA writeInt(Float.floatToIntBits(val));
        //throw new Exception("Float cannot yet be written to dataoutputstream");
    }
    }
}
