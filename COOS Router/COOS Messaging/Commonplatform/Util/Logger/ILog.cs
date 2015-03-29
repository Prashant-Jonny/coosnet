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
using System;
namespace Org.Coos.Messaging.Util
{
    /**
     *
     * @author Knut Eilif Husa, Tellu AS Log interface
     */
    public interface ILog
    {

        /**
         * Logging a debug message
         *
         * @param msg
         */
        void debug(string msg);

        /**
         * Logging a debug message
         *
         * @param msg
         */
        void debug(string msg, Exception e);

        /**
         * Logging an info message
         *
         * @param msg
         */
        void info(string msg);

        /**
         * Logging an info message
         *
         * @param msg
         */
        void info(string msg, Exception e);

        /**
         * Logging a warning message
         *
         * @param msg
         */
        void warn(string msg);

        /**
         * Logging a warning message with exception
         *
         * @param msg
         * @param e
         */
        void warn(string msg, Exception e);

        /**
         * Logging an error message
         *
         * @param msg
         */
        void error(string msg);

        /**
         * Logging an error message with exception
         *
         * @param msg
         * @param e
         */
        void error(string msg, Exception e);

        /**
         * Logging a trace message
         *
         * @param msg
         */
        void trace(string msg);

        /**
         * Setting the logger name
         *
         * @param loggerName
         */
        void setLoggerName(string loggerName);

        /**
         * Setting whether this logger shall inherit the MDC of the creating thread
         *
         * @param inheritMDC
         */
        void setInheritMDC(bool inheritMDC);

        /**
         *
         * @return
         */
        bool isInheritMDC();

        /**
         * Sets a MDC property of this logger
         *
         * @param key
         * @param value
         */
        void putMDC(string key, string value);

        /**
         * Removes a MDC property of this logger
         *
         * @param key
         */
        void removeMDC(string key);

    }
}