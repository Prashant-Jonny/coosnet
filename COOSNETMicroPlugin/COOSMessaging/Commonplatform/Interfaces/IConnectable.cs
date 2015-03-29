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
namespace Org.Coos.Messaging
{

    ///<summary>The LinkManager, Both Endpoints And Routers are LinkManagers.</summary>
    ///<author>Knut Eilif Husa, Tellu AS</author>
    public interface IConnectable
    {
        ///<summary>Adds a Link to this LinkManager</summary>
        ///<param name="destinationUuid">the unique identifier of the destination of this link</param>
        ///<param name="link">the link</param>
        ///<exception cref="ConnectingException">Thrown if this LinkManager is not able to add the Link, i.e. there is a configuration mismatch</exception>
        void addLink(string destinationUuid, Link link);
        
        ///<summary>The Default processor of this LinkManager</summary>
        ///<returns>the default Processor</returns>
        IProcessor getDefaultProcessor();

        ///<summary>Returns the link associated with the supplied destionation Uuid</summary>
        ///<param name="destinationUuid">the destination uuid</param>
        Link getLink(string destinationUuid);

        ///<summary>Removes the link associated with the supplied destinationUuid uuid</summary>
        ///<param name="destinationUuid">the endpoint uuid</param>
        void removeLink(string destinationUuid);

         ///<summary>Removes Link by the unique identifier of this Link</summary>
         ///<param name="linkId">linkId</param>
        void removeLinkById(string linkId);


    }
}
