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
using System.Runtime.CompilerServices;

namespace Org.Coos.Messaging.Util
{

    /**
     * @author Knut Eilif Husa, Tellu AS Generator of UUIDs
     */

    public class UuidGenerator
    {

        private static string UNIQUE_STUB = "-1-" + System.Guid.NewGuid().ToString() + "-";
        private static int instanceCount = 0;
        private static string hostName = "localhost";
        public static string UUID = "UUID-";

        //static {
        //    String stub = "";

        //    hostName = "localhost";
        //    stub = "-1-" + System.currentTimeMillis() + "-";



        //    UNIQUE_STUB = stub;
        //}

        private string seed;
        private long sequence;

        //private string currentTimeMillis()
        //{
        //    // Accessed 18 oct 2010 : http://bytes.com/topic/c-sharp/answers/281572-how-get-milliseconds-since-unix-epoch-c
        //    DateTime baseTime = new DateTime(1970, 1, 1, 0, 0, 0);
        //    DateTime nowInUTC = DateTime.UtcNow;
        //    return ((nowInUTC-baseTime).Ticks/10000).ToString();

        //}

        /**
         * Construct an IdGenerator
         */

        public UuidGenerator(string prefix)
        {

            if (UNIQUE_STUB == null)
                return;

            lock (UNIQUE_STUB)
            {
                this.seed = prefix + UNIQUE_STUB + (instanceCount++) + "-";
            }
        }

        public UuidGenerator()
            : this(UUID + hostName)
        {
        }

        /**
         * As we have to find the hostname as a side-affect of generating a unique
         * stub, we allow it's easy retrevial here
         *
         * @return the local host name
         */

        public static string getHostName()
        {
            return hostName;
        }

        /**
         * Generate a unqiue id
         *
         * @return a unique id
         */
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string generateId()
        {
           
                return this.seed + (this.sequence++);
            
        }

        ///**
        // * Generate a unique ID - that is friendly for a URL or file system
        // *
        // * @return a unique id
        // */
        public String generateSanitizedId()
        {
            String result = generateId();
            result = result.Replace(':', '-');
            result = result.Replace('_', '-');
            result = result.Replace('.', '-');

            return result;
        }

    }
}
