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
using Org.Coos.Messaging.Util;

using System;

namespace Org.Coos.Messaging
{

//package org.coos.messaging;

//import org.coos.messaging.COContainer;
//import org.coos.messaging.util.Log;
//import org.coos.messaging.util.LogFactory;


public class COFactory {

    private static readonly ILog logger = LogFactory.getLog(typeof(COFactory).FullName);

#if JAVA
    protected static Class<?> tryClass(ICOContainer cl, string className) {
        
       
        if (cl != null) {

            try {
                return cl.loadClass(className);
            } catch (ClassNotFoundException e) {
                logger.debug("Class not found in extender list: " + className);
            }
        }

        try {
            return Class.forName(className);
        } catch (ClassNotFoundException e) {
            logger.info("No classloader was able to find class: " + className, e);

            return null;
        }
    }

#endif

    protected static Type tryClass(ICOContainer cl, string typeName)
    {
        if (cl == null)
            return null;

        try
        {
            return cl.loadClass(typeName);
        }
        catch (Exception e)
        {
            logger.debug("Problems with loading of type: " + typeName);
            return null;
        }
    }

}
}