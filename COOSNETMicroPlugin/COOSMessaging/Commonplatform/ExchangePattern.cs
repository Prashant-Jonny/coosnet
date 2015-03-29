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

using System.Collections;

namespace Org.Coos.Messaging {

//import java.util.Hashtable;



///<summary>Represents the kind of message exchange pattern</summary>
///<author>Knut Eilif Husa, Tellu AS</author>
public class ExchangePattern {
    public static string OutOnly = "OutOnly";
    public static string RobustOutOnly = "RobustOutOnly";
    public static string OutIn = "OutIn";
    public static string OutOptionalIn = "OutOptionalIn";
    public static string InOnly = "InOnly";
    public static string RobustInOnly = "RobustInOnly";
    public static string InOut = "InOut";
    public static string InOptionalOut = "InOptionalOut";

    protected static  Hashtable map = new Hashtable();

    private string exchangePattern;

    public ExchangePattern(string exchangePattern) {
        this.exchangePattern = exchangePattern;
    }

    /**
     * Returns the WSDL URI for this message exchange pattern
     *
     * @return the WSDL URI for this message exchange pattern
     */
    public static string getWsdlUri(string exchangePattern) {

        if (exchangePattern.Equals(OutOnly))
            return "http://www.w3.org/ns/wsdl/out-only";

        if (exchangePattern.Equals(OutOptionalIn))
            return "http://www.w3.org/ns/wsdl/out-optional-in";

        if (exchangePattern.Equals(OutIn))
            return "http://www.w3.org/ns/wsdl/out-in";

        if (exchangePattern.Equals(InOut))
            return "http://www.w3.org/ns/wsdl/in-out";

        if (exchangePattern.Equals(InOnly))
            return "http://www.w3.org/ns/wsdl/in-only";

        if (exchangePattern.Equals(InOptionalOut))
            return "http://www.w3.org/ns/wsdl/in-optional_out";

        if (exchangePattern.Equals(RobustOutOnly))
            return "http://www.w3.org/ns/wsdl/robust-out-only";

        if (exchangePattern.Equals(RobustInOnly))
            return "http://www.w3.org/ns/wsdl/robust-in-only";

        
       // JAVA  throw new IllegalArgumentException("Unknown message exchange pattern: " + exchangePattern);

        throw new System.ArgumentException("Unknown message exchange pattern: " + exchangePattern);
    }

    /**
     * Return true if there can be an IN message
     */
   
   
    public bool isInCapable() {

        if (exchangePattern.Equals(InOnly) || exchangePattern.Equals(RobustInOnly))
            return false;
        else
            return true;

    }

    /**
     * Return true if there can be an OUT message
     */
    public bool isOutCapable() {

        if (exchangePattern.Equals(OutOnly) || exchangePattern.Equals(RobustOutOnly))
            return false;
        else
            return true;

    }

    /**
     * Return true if there can be a FAULT message
     */
    public bool isFaultCapable() {

        if (exchangePattern.Equals(OutOnly) || exchangePattern.Equals(InOnly))
            return false;
        else
            return true;

    }

    public bool equals(object obj) {

        if ((obj is string) && exchangePattern.Equals(obj)) {
            return true;
        }

        return base.Equals(obj);
    }

    public string toString() {
        return exchangePattern;
    }

    /**
     * Converts the WSDL URI into a {@link ExchangePattern} instance
     */
    /*
     * public static ExchangePattern fromWsdlUri(String wsdlUri) { return
     * (ExchangePattern) map.get(wsdlUri); }
     *
     * static { for (int i = 1; i < 7; i++) { String uri = getWsdlUri(i); String
     * pattern = null; populatePattern(uri, pattern); } }
     *
     * private static void populatePattern(String uri, String pattern) {
     * map.put(uri, pattern); String name = uri.substring(uri.lastIndexOf('/') +
     * 1); map.put("http://www.w3.org/2004/08/wsdl/" + name, pattern);
     * map.put("http://www.w3.org/2006/01/wsdl/" + name, pattern); }
     */

    public bool isOutBoundInitiated() {
        return exchangePattern.Equals(OutOnly) || exchangePattern.Equals(RobustOutOnly) || exchangePattern.Equals(OutIn) ||
            exchangePattern.Equals(OutOptionalIn);
    }

    public bool isInBoundInitiated() {
        return exchangePattern.Equals(InOnly) || exchangePattern.Equals(RobustInOnly) || exchangePattern.Equals(InOut) ||
            exchangePattern.Equals(InOptionalOut);
    }
}}
