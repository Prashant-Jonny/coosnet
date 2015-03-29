#define NET
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

using Org.Coos.Util.Serialize;

using System;
using System.IO;

namespace Org.Coos.Messaging
{
    //package org.coos.messaging;

    //import org.coos.util.serialize.AFClassLoader;

    //import java.io.InputStream;
    //import java.io.IOException;

    /**
     * This interface hides the container implementation from the COOS implementation, i.e. OSGI or standalone J2SE.
     *
     * @author Knut Eilif Husa, Tellu AS
     */
    public interface ICOContainer : AFClassLoader
    {
        // Not allowed in C#
#if FIELDS
        static readonly string BUNDLE_CONTEXT = "BundleContext";
        static readonly string CLASS_LOADER = "ClassLoader";
#endif
        /**
         * This method loads the class
         * 
         * @param className
         *            the class name
         * @return Class the Class
         * @throws ClassNotFoundException
         */
#if JAVA
	public Class loadClass(String className) throws ClassNotFoundException;
#endif
#if NET
   new Type  loadClass(string typeName);
#endif
        /**
	 * Opens a resource input stream
	 * 
	 * @param resourceName
	 *            the resource name
	 * @return InputStream
	 * @throws IOException
	 */
#if JAVA
    public InputStream getResource(String resourceName) throws IOException;
#endif

    MemoryStream getResource(string resourceName);

        /**
	 * Getter for Objects provided by the Container
	 * 
	 * @param name
	 *            of the Object
	 * @return the named Object
	 */
        object getObject(string name);

#if JAVA
	public void start() throws Exception;
#endif
        void start();
#if JAVA
	public void stop() throws Exception;
#endif
        void stop();
    }
}
