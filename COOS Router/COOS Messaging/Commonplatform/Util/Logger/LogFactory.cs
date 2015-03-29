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
//package org.coos.messaging.util;

namespace Org.Coos.Messaging.Util
{
    /**
     *
     * @author Knut Eilif Husa, Tellu AS Log factory
     */
    public class LogFactory
    {

        private static string LOG_TYPE_KEY = "Type";

        /**
         * This method returns a logger. If the present in the classpath a
         * Slf4jLogImpl will be returned. This logger has full support for MDC
         * (properties on the logger). If not present on the class path a basic
         * logger only supporting log output to the console will be returned. MDC
         * info is not supported for the basic logger.
         *
         * @param logName
         *            - the name of the logger
         * @return the logger
         */

        public static ILog getLog(string logName, bool inheritMDC)
        {

#if SL4JLOG
        try {
            Log logger = (Log) (Class.forName("org.coos.messaging.util.Slf4jLogImpl").newInstance());
            logger.setLoggerName(logName);
            logger.setInheritMDC(inheritMDC);

            return logger;
        } catch (Exception e) {
        }
#endif
            return new DefaultLogger(logName);
        }

        public static ILog getLog(string logName)
        {
            return getLog(logName, true);
        }

#if SLF4J
    /**
     * This method returns a logger. If the present in the classpath a
     * Slf4jLogImpl will be returned. This logger has full support for MDC
     * (properties on the logger). If not present on the class path a basic
     * logger only supporting log output to the console will be returned. MDC
     * info is not supported for the basic logger.
     *
     * @param Class
     *            - the class that the name of the logger will be derived from
     * @return the logger
     */
    public static Log getLog(Class theClass) {
        return getLog(theClass.getName(), true);
    }

    public static Log getLog(Class theClass, boolean inheritMDC) {
        return getLog(theClass.getName(), inheritMDC);
    }

    /**
     * This method returns a logger for a specific type of log statements, e.g.
     * billing, etc. The log type is placed in the MDC of the logger.
     *
     * @param logName
     *            - the name of the logger
     * @param type
     *            - the type of the logger
     * @return the logger
     */
    public static Log getLog(String logName, String type) {
        Log log = getLog(logName);

        if (type != null) {
            log.putMDC(LOG_TYPE_KEY, type);
        }

        return log;
    }
#endif

    }
}
