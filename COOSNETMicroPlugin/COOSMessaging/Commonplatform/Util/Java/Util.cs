/*
*  Licensed to the Apache Software Foundation (ASF) under one or more
*  contributor license agreements.  See the NOTICE file distributed with
*  this work for additional information regarding copyright ownership.
*  The ASF licenses this file to You under the Apache License, Version 2.0
*  (the "License"); you may not use this file except in compliance with
*  the License.  You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
*  Unless required by applicable law or agreed to in writing, software
*  distributed under the License is distributed on an "AS IS" BASIS,
*  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*  See the License for the specific language governing permissions and
*  limitations under the License.
*/
using System;
using UTFDataFormatException = java.io.UTFDataFormatException;
//UPGRADE_TODO: The type 'org.apache.harmony.luni.internal_Renamed.nls.Messages' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
//using Messages = org.apache.harmony.luni.internal_Renamed.nls.Messages;
namespace org.apache.harmony.luni.util
{

    public sealed class Util
    {

        private static System.String[] WEEKDAYS = new System.String[] { "", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        private static System.String[] MONTHS = new System.String[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        private static System.String defaultEncoding;

#if ENABLECODE
        /// <summary> Get bytes from String using default encoding; default encoding can
        /// be changed via "os.encoding" property
        /// </summary>
        /// <param name="name">input String
        /// </param>
        /// <returns> byte array
        /// </returns>
        public static sbyte[] getBytes(System.String name)
        {
            if (defaultEncoding != null)
            {
                try
                {
                    //UPGRADE_TODO: Method 'java.lang.String.getBytes' was converted to 'System.Text.Encoding.GetEncoding(string).GetBytes(string)' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangStringgetBytes_javalangString'"
                    return SupportClass.ToSByteArray(System.Text.Encoding.GetEncoding(defaultEncoding).GetBytes(name));
                }
                catch (System.IO.IOException e)
                {
                }
            }
            return SupportClass.ToSByteArray(SupportClass.ToByteArray(name));
        }

        /// <summary> Get bytes from String with UTF8 encoding</summary>
        /// <param name="name">input String
        /// </param>
        /// <returns> byte array
        /// </returns>
        public static sbyte[] getUTF8Bytes(System.String name)
        {
           // try
           // {
                //UPGRADE_TODO: Method 'java.lang.String.getBytes' was converted to 'System.Text.Encoding.GetEncoding(string).GetBytes(string)' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangStringgetBytes_javalangString'"
                return SupportClass.ToSByteArray(System.Text.Encoding.UTF8.GetBytes(name));
            //}
            //catch (System.IO.IOException e)
            //{
            //    return getBytes(name);
            //}
        }

        public static System.String toString(sbyte[] bytes)
        {
            if (defaultEncoding != null)
            {
                try
                {
                    System.String tempStr;
                    //UPGRADE_TODO: The differences in the Format  of parameters for constructor 'java.lang.String.String'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    tempStr = System.Text.Encoding.GetEncoding(defaultEncoding).GetString(SupportClass.ToByteArray(bytes));
                    return new System.String(tempStr.ToCharArray(), 0, bytes.Length);
                }
                catch (System.IO.IOException e)
                {
                }
            }
            return new System.String(SupportClass.ToCharArray(bytes), 0, bytes.Length);
        }

        public static System.String toUTF8String(sbyte[] bytes)
        {
            return toUTF8String(bytes, 0, bytes.Length);
        }

        public static System.String toString(sbyte[] bytes, int offset, int length)
        {
            if (defaultEncoding != null)
            {
                try
                {
                    System.String tempStr;
                    //UPGRADE_TODO: The differences in the Format  of parameters for constructor 'java.lang.String.String'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    tempStr = System.Text.Encoding.GetEncoding(defaultEncoding).GetString(SupportClass.ToByteArray(bytes));
                    return new System.String(tempStr.ToCharArray(), offset, length);
                }
                catch (System.IO.IOException e)
                {
                }
            }
            return new System.String(SupportClass.ToCharArray(bytes), offset, length);
        }

        public static System.String toUTF8String(sbyte[] bytes, int offset, int length)
        {
            try
            {
                System.String tempStr;
                //UPGRADE_TODO: The differences in the Format  of parameters for constructor 'java.lang.String.String'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                tempStr = System.Text.Encoding.GetEncoding("UTF-8").GetString(SupportClass.ToByteArray(bytes));
                return new System.String(tempStr.ToCharArray(), offset, length);
            }
            catch (System.IO.IOException e)
            {
                return toString(bytes, offset, length);
            }
        }

        /// <summary> Answers the millisecond value of the date and time parsed from the
        /// specified String. Many date/time formats are recognized
        /// 
        /// </summary>
        /// <param name="string">the String to parse
        /// </param>
        /// <returns> the millisecond value parsed from the String
        /// </returns>
        public static long parseDate(System.String string_Renamed)
        {
            int offset = 0, length = string_Renamed.Length, state = 0;
            int year = -1, month = -1, date = -1;
            int hour = -1, minute = -1, second = -1;
            //UPGRADE_NOTE: Final was removed from the declaration of 'PAD '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
            //UPGRADE_NOTE: Final was removed from the declaration of 'LETTERS '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
            //UPGRADE_NOTE: Final was removed from the declaration of 'NUMBERS '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
            int PAD = 0;
            int LETTERS = 1;
            int NUMBERS = 2;
            StringBuilder buffer = new StringBuilder();

            while (offset <= length)
            {
                char next = offset < length ? string_Renamed[offset] : '\r';
                offset++;

                int nextState;
                if ((next >= 'a' && next <= 'z') || (next >= 'A' && next <= 'Z'))
                    nextState = LETTERS;
                else if (next >= '0' && next <= '9')
                    nextState = NUMBERS;
                else if (" ,-:\r\t".IndexOf((System.Char)next) == -1)
                    throw new System.ArgumentException();
                else
                    nextState = PAD;

                if (state == NUMBERS && nextState != NUMBERS)
                {
                    int digit = Integer.parseInt(buffer.toString());
                    buffer.setLength(0);
                    if (digit >= 70)
                    {
                        if (year != -1 || (next != ' ' && next != ',' && next != '\r'))
                            throw new System.ArgumentException();
                        year = digit;
                    }
                    else if (next == ':')
                    {
                        if (hour == -1)
                            hour = digit;
                        else if (minute == -1)
                            minute = digit;
                        else
                            throw new System.ArgumentException();
                    }
                    else if (next == ' ' || next == ',' || next == '-' || next == '\r')
                    {
                        if (hour != -1 && minute == -1)
                            minute = digit;
                        else if (minute != -1 && second == -1)
                            second = digit;
                        else if (date == -1)
                            date = digit;
                        else if (year == -1)
                            year = digit;
                        else
                            throw new System.ArgumentException();
                    }
                    else if (year == -1 && month != -1 && date != -1)
                        year = digit;
                    else
                        throw new System.ArgumentException();
                }
                else if (state == LETTERS && nextState != LETTERS)
                {
                    System.String text = buffer.toString().toUpperCase();
                    buffer.setLength(0);
                    if (text.Length < 3)
                        throw new System.ArgumentException();
                    if (parse(text, WEEKDAYS) != -1)
                    {
                    }
                    else if (month == -1 && (month = parse(text, MONTHS)) != -1)
                    {
                    }
                    else if (text.Equals("GMT"))
                    {
                    }
                    else
                        throw new System.ArgumentException();
                }

                if (nextState == LETTERS || nextState == NUMBERS)
                    buffer.append(next);
                state = nextState;
            }

            if (year != -1 && month != -1 && date != -1)
            {
                if (hour == -1)
                    hour = 0;
                if (minute == -1)
                    minute = 0;
                if (second == -1)
                    second = 0;
                //UPGRADE_ISSUE: Method 'java.util.TimeZone.getTimeZone' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilTimeZonegetTimeZone_javalangString'"
                TimeZone.getTimeZone("GMT");
                System.Globalization.Calendar cal = new System.Globalization.GregorianCalendar();
                //UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
                int current = SupportClass.CalendarManager.manager.Get(cal, SupportClass.CalendarManager.YEAR) - 80;
                if (year < 100)
                {
                    year += current / 100 * 100;
                    if (year < current)
                        year += 100;
                }
                SupportClass.CalendarManager.manager.Set(cal, SupportClass.CalendarManager.YEAR, year);
                SupportClass.CalendarManager.manager.Set(cal, SupportClass.CalendarManager.MONTH, month);
                SupportClass.CalendarManager.manager.Set(cal, SupportClass.CalendarManager.DATE, date);
                SupportClass.CalendarManager.manager.Set(cal, SupportClass.CalendarManager.HOUR_OF_DAY, hour);
                SupportClass.CalendarManager.manager.Set(cal, SupportClass.CalendarManager.MINUTE, minute);
                SupportClass.CalendarManager.manager.Set(cal, SupportClass.CalendarManager.SECOND, second);
                SupportClass.CalendarManager.manager.Set(cal, SupportClass.CalendarManager.MILLISECOND, 0);
                //UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
                //UPGRADE_TODO: The equivalent in .NET for method 'java.util.Calendar.getTime' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                return SupportClass.CalendarManager.manager.GetDateTime(cal).Ticks;
            }
            throw new System.ArgumentException();
        }

        private static int parse(System.String string_Renamed, System.String[] array)
        {
            int length = string_Renamed.Length;
            for (int i = 0; i < array.Length; i++)
            {
                if (String.Compare(string_Renamed, 0, array[i], 0, length, true) == 0)
                    return i;
            }
            return -1;
        }
#endif
        public static System.String convertFromUTF8(sbyte[] buf, int offset, int utfSize)
        {
            return convertUTF8WithBuf(buf, new char[utfSize], offset, utfSize);
        }

        public static System.String convertUTF8WithBuf(sbyte[] buf, char[] out_Renamed, int offset, int utfSize)
        {
            int count = 0, s = 0, a;
            while (count < utfSize)
            {
                if ((out_Renamed[s] = (char)buf[offset + count++]) < '\u0080')
                    s++;
                else if (((a = out_Renamed[s]) & 0xe0) == 0xc0)
                {
                    if (count >= utfSize)
                        throw new UTFDataFormatException("Convert problem");
                    int b = buf[count++];
                    if ((b & 0xC0) != 0x80)
                        throw new UTFDataFormatException("Convert problem");
                    out_Renamed[s++] = (char)(((a & 0x1F) << 6) | (b & 0x3F));
                }
                else if ((a & 0xf0) == 0xe0)
                {
                    if (count + 1 >= utfSize)
                        throw new UTFDataFormatException("Convert problem");
                    int b = buf[count++];
                    int c = buf[count++];
                    if (((b & 0xC0) != 0x80) || ((c & 0xC0) != 0x80))
                        throw new UTFDataFormatException("Convert problem");
                    out_Renamed[s++] = (char)(((a & 0x0F) << 12) | ((b & 0x3F) << 6) | (c & 0x3F));
                }
                else
                {
                    throw new UTFDataFormatException("Convert problem");
                }
            }
            return new System.String(out_Renamed, 0, s);
        }

#if ENABLECODE
		/// <summary> '%' and two following hex digit characters are converted to the
		/// equivalent byte value. All other characters are passed through
		/// unmodified. e.g. "ABC %24%25" -> "ABC $%"
		/// 
		/// </summary>
		/// <param name="s">java.lang.String The encoded string.
		/// </param>
		/// <returns> java.lang.String The decoded version.
		/// </returns>
		public static System.String decode(System.String s, bool convertPlus)
		{
			return decode(s, convertPlus, null);
		}
		
		/// <summary> '%' and two following hex digit characters are converted to the
		/// equivalent byte value. All other characters are passed through
		/// unmodified. e.g. "ABC %24%25" -> "ABC $%"
		/// 
		/// </summary>
		/// <param name="s">java.lang.String The encoded string.
		/// </param>
		/// <param name="encoding">the specified encoding
		/// </param>
		/// <returns> java.lang.String The decoded version.
		/// </returns>
		public static System.String decode(System.String s, bool convertPlus, System.String encoding)
		{
			if (!convertPlus && s.IndexOf('%') == - 1)
				return s;
			StringBuilder result = new StringBuilder(s.Length);
			System.IO.MemoryStream out_Renamed = new System.IO.MemoryStream();
			for (int i = 0; i < s.Length; )
			{
				char c = s[i];
				if (convertPlus && c == '+')
					result.append(' ');
				else if (c == '%')
				{
					//UPGRADE_ISSUE: Method 'java.io.ByteArrayOutputStream.reset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioByteArrayOutputStreamreset'"
					out_Renamed.reset();
					do 
					{
						if (i + 2 >= s.Length)
							throw new IllegalArgumentException(Messages.getString("luni.80", i));
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Character.digit' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						int d1 = System.Char.GetNumericValue(s[i + 1]);
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Character.digit' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						int d2 = System.Char.GetNumericValue(s[i + 2]);
						if (d1 == - 1 || d2 == - 1)
							throw new IllegalArgumentException(Messages.getString("luni.81", s.Substring(i, (i + 3) - (i)), System.Convert.ToString(i)));
						out_Renamed.WriteByte((byte) ((d1 << 4) + d2));
						i += 3;
					}
					while (i < s.Length && s[i] == '%');
					if (encoding == null)
					{
						char[] tmpChar;
						byte[] tmpByte;
						tmpByte = out_Renamed.GetBuffer();
						tmpChar = new char[out_Renamed.Length];
						System.Array.Copy(tmpByte, 0, tmpChar, 0, tmpChar.Length);
						result.append(new System.String(tmpChar));
					}
					else
					{
						try
						{
							//UPGRADE_ISSUE: Method 'java.io.ByteArrayOutputStream.toString' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioByteArrayOutputStreamtoString_javalangString'"
							result.append(out_Renamed.toString(encoding));
						}
						catch (System.IO.IOException e)
						{
							throw new IllegalArgumentException(e);
						}
					}
					continue;
				}
				else
					result.append(c);
				i++;
			}
			return result.toString();
		}
		
		public static System.String toASCIILowerCase(System.String s)
		{
			int len = s.Length;
			StringBuilder buffer = new StringBuilder(len);
			for (int i = 0; i < len; i++)
			{
				char c = s[i];
				if ('A' <= c && c <= 'Z')
				{
					buffer.append((char) (c + ('a' - 'A')));
				}
				else
				{
					buffer.append(c);
				}
			}
			return buffer.toString();
		}
		
		public static System.String toASCIIUpperCase(System.String s)
		{
			int len = s.Length;
			StringBuilder buffer = new StringBuilder(len);
			for (int i = 0; i < len; i++)
			{
				char c = s[i];
				if ('a' <= c && c <= 'z')
				{
					buffer.append((char) (c - ('a' - 'A')));
				}
				else
				{
					buffer.append(c);
				}
			}
			return buffer.toString();
		}
		static Util()
		{
			{
				//UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
				System.String encoding = System_Renamed.getProperty("os.encoding");
				if (encoding != null)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.lang.String.getBytes' was converted to 'System.Text.Encoding.GetEncoding(string).GetBytes(string)' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangStringgetBytes_javalangString'"
						SupportClass.ToSByteArray(System.Text.Encoding.GetEncoding(encoding).GetBytes(""));
					}
					//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
					catch (System.Exception t)
					{
						encoding = null;
					}
				}
				defaultEncoding = encoding;
			}
		}
	}
	
	// Source:  Save This Page Home » apache-harmony-6.0-src-r917296-snapshot » org.apache.harmony.luni.util » [javadoc | source]

#endif
    }
}

