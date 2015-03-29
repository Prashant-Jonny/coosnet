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

namespace Org.Coos.Messaging.Util
{


    /**
     * @author Knut Eilif Husa, Tellu AS
     * Helper class for parsing UUIDs
     */
    public class UuidHelper
    {

        
        /// <summary>
        /// This method returns the segment part from a endpointName/alias or an endpointUuid
        /// Must only be used for qualified endpoint identificators
        /// </summary>
        /// <param name="segmentedUuid"></param>
        /// <returns></returns>
        public static string getSegmentFromEndpointNameOrEndpointUuid(string segmentedUuid)
        {
            int idx = segmentedUuid.LastIndexOf('.');

            if (idx > 0)
            {
                string seg = segmentedUuid.Substring(0, idx);

                /*if(!seg.startsWith(".")){
                        seg = "." + seg;
                }*/
                return seg;
            }
            else
            {
                return ".";
            }
        }

        
        /// <summary>
        /// This method returns the segment part from an endpointUuid or segment
        /// Must not be used on endpointNames/Aliases
        /// </summary>
        /// <param name="segmentOrEndpointUuid"></param>
        /// <returns></returns>
        public static string getSegmentFromSegmentOrEndpointUuid(string segmentOrEndpointUuid)
        {

            if (isSegment(segmentOrEndpointUuid))
            {
                return segmentOrEndpointUuid;
            }
            else
            {
                return getSegmentFromEndpointNameOrEndpointUuid(segmentOrEndpointUuid);
            }
        }

        /// <summary>
        /// Checks if the alias is valid for for an segmented endpointUuid
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="segmentedUuid"></param>
        /// <returns></returns>
        public static bool isValidAliasForUuid(string alias, string segmentedUuid)
        {
            string aliasSegment = getSegmentFromEndpointNameOrEndpointUuid(alias);

            if (aliasSegment.Equals("dico") || aliasSegment.Equals("localcoos"))
            {
                return true;
            }

            string segment = getSegmentFromEndpointNameOrEndpointUuid(segmentedUuid);

            if (aliasSegment.Equals(segment))
            {
                return true;
            }

            return false;
        }

        public static string getParentSegment(string segmentedUuid)
        {
            string segment = getSegmentFromSegmentOrEndpointUuid(segmentedUuid);

            if (segment.Equals("."))
            {
                return ""; // segementedUuid on top level
            }

            int idx = segment.LastIndexOf('.');

            if (idx > 0)
            {
                return segmentedUuid.Substring(0, idx);
            }
            else
            {
                return ".";
            }
        }

        public static string getQualifiedUuid(string uuid)
        {

            if (uuid == null)
                return null;

            if (uuid.IndexOf("UUID-") == -1)
            {
                return uuid; //Not an Uuid, returning input parameter
            }

            
            if (uuid.StartsWith("UUID-")) 
            {
                uuid = "." + uuid;
            }

            return uuid;
        }

        public static bool isUuid(string segmentedUuid)
        {

            if (segmentedUuid == null)
            {
                return false;
            }

            if (segmentedUuid.IndexOf("UUID-") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool isRouterUuid(string segmentedUuid)
        {

            if (segmentedUuid == null)
            {
                return false;
            }

            if (segmentedUuid.IndexOf("UUID-R-") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isSegment(string segmentedUuid)
        {

            if (segmentedUuid == null)
            {
                return false;
            }

            return segmentedUuid.IndexOf("UUID-") == -1;
        }

        public static string replaceSegment(string uuid, string segmentName)
        {

            if (isSegment(uuid))
            {
                return segmentName;
            }
            else
            {
                int idx = uuid.LastIndexOf('.');

                if (idx != -1)
                {

                    if (segmentName.Equals("."))
                    {
                        return uuid.Substring(idx);
                    }
                    else
                    {
                        return segmentName + uuid.Substring(idx);
                    }
                }
                else
                {
                    return segmentName;
                }
            }
        }

        
        /// <summary>
        /// Checks whether segments is in a parent child relation
        /// </summary>
        /// <param name="seg1"></param>
        /// <param name="seg2"></param>
        /// <returns></returns>
        public static bool isInParentChildRelation(string seg1, string seg2)
        {

            return (getParentSegment(seg1).Equals(seg2) || getParentSegment(seg2).Equals(seg1));

        }
    }
}
