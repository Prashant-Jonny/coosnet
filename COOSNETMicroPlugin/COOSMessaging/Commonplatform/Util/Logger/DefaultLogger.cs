/**
 * Copyright Telenor Objects AS
 */
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

//import java.util.Date;

using System;
using Microsoft.SPOT;

namespace Org.Coos.Messaging.Util
{

/**
 * This class is a logger class used in environments where j2se is not available
 *
 * @author Knut Eilif Husa, Tellu AS
 *
 */
    public class DefaultLogger : ILog
    {

        public static int TRACE = 1;
        public static int DEBUG = 2;
        public static int INFO = 3;
        public static int WARN = 4;
        public static int ERROR = 5;
        public static int FATAL = 6;


        private static int level = 1;

        private string name = String.Empty;

        public DefaultLogger(string loggerName)
        {
            
            this.name = loggerName;
        }

        public static int getLevel()
        {
            return level;
        }

        public static void setLevel(int level)
        {
            DefaultLogger.level = level;
        }

        public void setLoggerName(string loggerName)
        {
            name = loggerName;
        }

        public void trace(string arg) {

        if (level <= TRACE) {
            printMsg(":[TRACE]:",arg);
        }

    }

private static void printMsg(string category,string arg)
{
    string buf;

    buf = System.DateTime.Now.ToString()+" "+category+" "+arg;
    Debug.Print(buf);
}

        public void debug(string arg) {

        if (level <= DEBUG) {
           printMsg(":[DEBUG]:",arg);
        }
    }

        public void info(string arg) {

        if (level <= INFO) {
            printMsg(":[INFO]:",arg);
        }

    }

        public void warn(string arg) {

        if (level <= WARN) {
            printMsg(":[WARNING]:",arg);
        }

    }


        public void warn(string arg, Exception e)
        {

            if (level <= WARN)
            {
                warn(arg);
                Debug.Print(e.StackTrace.ToString());
            }

        }

        public void error(string arg)
        {

            if (level <= ERROR)
            {
              printMsg(":[ERROR]:",arg);
            }

        }

        public void error(string arg, Exception e)
        {

            if (level <= ERROR)
            {
                error(arg);
                Debug.Print(e.StackTrace.ToString());
            }

        }

        public void putMDC(string key, string value)
        {
            // No support for MDC
        }

        public void removeMDC(string key)
        {
            // No support for MDC
        }

        public bool isInheritMDC()
        {

            // No support for MDC
            return false;
        }

        public void setInheritMDC(bool inheritMDC)
        {
            // No support for MDC
        }

        public void debug(string msg, Exception e)
        {

            if (level <= DEBUG)
            {
                debug(msg);
                Debug.Print(e.StackTrace.ToString());
            }
        }

        public void info(string msg, Exception e)
        {

            if (level <= INFO)
            {
                info(msg);
                Debug.Print(e.StackTrace.ToString());
            }
        }
    }
}
