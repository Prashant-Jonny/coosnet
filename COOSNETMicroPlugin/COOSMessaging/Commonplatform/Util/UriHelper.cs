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

namespace Org.Coos.Messaging.Util
{

/**
 * @author Knut Eilif Husa, Tellu AS Helper class for parsing URIs
 */
public class URIHelper {

	private string endpointUri;
	private string path = "";
	private string endpoint;

	// JAVA public URIHelper(string endpointUri) throws IllegalArgumentException {
	public URIHelper(string endpointUri)  {
			
    
    this.endpointUri = endpointUri;
		int i = endpointUri.IndexOf("coos://");
		if (i == -1) {
			throw new System.ArgumentException("Unknown protocol for uri: " + endpointUri);
		}
		i = i + 7; // end of protocol name part

		int j = endpointUri.IndexOf("/", i + 1); // end of endpoint name part
		string s;
		if (j != -1) {
			endpoint = endpointUri.Substring(i, j-i);
			path = endpointUri.Substring(j);
		} else {
			endpoint = endpointUri.Substring(i);
		}
	}

	public bool isEndpointUuid() {
		return UuidHelper.isUuid(endpoint);
	}
	
	public bool isEndpointQualified(){
		return endpoint.IndexOf('.') != -1;
	}

	public string getEndpoint() {
		return endpoint;
	}
	
    public string getUnqualifiedEndpoint() {
        int idx = endpoint.IndexOf('.');

        if (idx != -1) {
            return endpoint.Substring(idx + 1);
        }

        return endpoint;
    }

	public void setEndpoint(string endpoint) {
		this.endpoint = endpoint;
	}

	public string getPath() {
		return path;
	}

	public void setPath(string path) {
		this.path = path;
	}

	public string getEndpointUri() {
		return "coos://" + endpoint + path;
	}

	public string getSegment() {
		int idx = endpoint.LastIndexOf('.');
		if (idx == 0){
			return ".";
		} else if (idx != -1) {
			return endpoint.Substring(0, idx);
		} 
		return "";
	}

}
}