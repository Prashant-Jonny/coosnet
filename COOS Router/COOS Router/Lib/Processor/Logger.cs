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
using System.Collections.Generic;

using Org.Coos.Messaging.Util;
using Org.Coos.Messaging.Impl;

namespace Org.Coos.Messaging.Processor
{
//package org.coos.messaging.processor;

//import java.util.Hashtable;

//import org.coos.messaging.util.Log;
//import org.coos.messaging.util.LogFactory;
//import org.coos.messaging.Message;
//import org.coos.messaging.Processor;
//import org.coos.messaging.impl.DefaultProcessor;

/**
 * @author Knut Eilif Husa, Tellu AS 
 * Logger log messages that go through
 */
public class Logger : DefaultProcessor, IProcessor {

	private static readonly String PROPERTY_LOGGER_NAME = "loggerName";
	private static readonly ILog logger = LogFactory.getLog(typeof(Logger).FullName);
	private String loggerName = "DefaultLoggerName";

	public Logger() {
	}

	public Logger(String loggerName) {
		this.loggerName = loggerName;
	}

	//@SuppressWarnings("unchecked")
	public override  void setProperties(Dictionary<string,string> properties) {
		base.setProperties(properties);
		if(properties.ContainsKey(PROPERTY_LOGGER_NAME)){
			loggerName = (String) properties[PROPERTY_LOGGER_NAME];
		} 
	}

	public override void processMessage(IMessage msg) {
		if(msg.getType().Equals(IMessagePrimitives.TYPE_MSG)){
		logger.info(loggerName + ", ReceiverEndpoint: " + msg.getReceiverEndpointUri() + ", " + "SenderEndpoint: "
				+ msg.getSenderEndpointUri() + ", Message name: " + msg.getHeader(IMessagePrimitives.MESSAGE_NAME)
                + ", Message type: " + msg.getHeader(IMessagePrimitives.TYPE) + ", isOutLink: " + msg.getMessageContext().getCurrentLink().isOutLink());
		}
	}

	public override void setShared(Boolean shared) {
		// This processor is always shared
	}

	public override bool isShared() {
		return true;
	}
}}
