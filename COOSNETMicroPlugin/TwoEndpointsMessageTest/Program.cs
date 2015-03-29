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
using Org.Coos.Messaging.Impl;
using System.Threading;

//#define KEEP_ROUTER

namespace Org.Coos.Messaging.Plugin.Simple {


//import junit.framework.TestCase;

//import org.coos.messaging.AsyncCallback;
//import org.coos.messaging.Endpoint;
//import org.coos.messaging.Exchange;
//import org.coos.messaging.Link;
//import org.coos.messaging.impl.DefaultMessage;
//import org.coos.messaging.processor.Logger;
//import org.coos.messaging.routing.DefaultRouter;
//import org.coos.messaging.routing.RouterFactory;
//import org.coos.messaging.util.URIHelper;

/**
 * A simple test for attaching to endpoints to the coos message bus
 * 
 * @author Knut Eilif Husa, Tellu AS
 */
public class TwoEndpointsMessageTest  {

	private string endpointUri1 = "coos://test1";
	private string endpointName1 = ".test1";
	private string endpointUuid1 = ".UUID-test1";
	private string routerUuid = ".UUID-R-1";
	private SimpleProducer producer1;
	private SimpleConsumer consumer1;
	private IEndpoint endpoint1;
#if KEEP_ROUTER
    private DefaultRouter router;
#endif
    private Link inLink1, outLink1;
	private IExchange exchange = null;

	private string endpointUri2 = "coos://test2";
	private string endpointName2 = ".test2";
	private string endpointUuid2 = ".UUID-test2";
	private SimpleProducer producer2;
	private SimpleConsumer consumer2;
	private IEndpoint endpoint2;
	private Link inLink2, outLink2;

	public void setUp()  {
#if KEEP_ROUTER
        RouterFactory.clear();
#endif
        // set up endpoint one
		inLink1 = new Link();
		//inLink1.addFilterProcessor(new Logger("inLink 1"));
		outLink1 = new Link();
		//outLink1.addFilterProcessor(new Logger("outLink 1"));
		endpoint1 = new SimpleEndpoint(endpointUri1, inLink1);
		endpoint1.setName(endpointName1);

		producer1 = (SimpleProducer) endpoint1.createProducer();
		consumer1 = (SimpleConsumer) endpoint1.createConsumer();
		outLink1.setChainedProcessor(endpoint1);
		outLink1.addAlias(endpointName1);
#if KEEP_ROUTER
        router = (DefaultRouter) RouterFactory.getRouterInstance(routerUuid);
#endif
        inLink1.setChainedProcessor(router);
		router.addLink(endpointUuid1, outLink1);

		// set up endpoint two
		inLink2 = new Link();
		//inLink2.addFilterProcessor(new Logger("inLink 2"));
		outLink2 = new Link();
		//outLink2.addFilterProcessor(new Logger("outLink 2"));
		endpoint2 = new SimpleEndpoint(endpointUri2, inLink2);
		endpoint2.setName(endpointName2);

		producer2 = (SimpleProducer) endpoint2.createProducer();
		consumer2 = (SimpleConsumer) endpoint2.createConsumer();
		outLink2.setChainedProcessor(endpoint2);
		outLink2.addAlias(endpointName2);
		inLink2.setChainedProcessor(router);
		router.addLink(endpointUuid2, outLink2);

		URIHelper helper1 = new URIHelper(endpointUri1);
		URIHelper helper2 = new URIHelper(endpointUri2);
		endpoint1.setEndpointUuid(endpointUuid1);

		endpoint2.setEndpointUuid(endpointUuid2);
	}

	/**
	 * Send a message from ep1 to ep2 and check that it has arrived
	 * 
	 * @throws InterruptedException
	 */
	public void testSendMessage()  {
		producer1.sendMessage("coos://test2/foo/bar", new DefaultMessage());
		Thread.sleep(100); // let the consumerthread get chance to process
		assertFalse(consumer2.getConsumerQueue().isEmpty());
		System.out.println("testSendMessage finished");
		System.out.println();
        
	}

	/**
	 * Send a message synchronously from ep1 to ep2 and check that the exchange
	 * contains an out message
	 */
	public void testRequestMessage() {
		Exchange exchange = producer1.requestMessage("coos://test2/foo/bar", new DefaultMessage());
		assertNotNull(exchange.getInBoundMessage());
		System.out.println("testRequestMessage finished");
		System.out.println();
	}

	/**
	 * Send a message asynchronously from ep1 to ep2 and check that the exchange
	 * contains an out message when returning in the callback
	 */
	public void testAsyncRequestMessage() throws InterruptedException {
		exchange = null;
		producer1.requestMessage("coos://test2/foo/bar", new DefaultMessage(), new AsyncCallback() {
			public void processExchange(Exchange exchange) {
				synchronized (TwoEndpointsMessageTest.this) {
					TwoEndpointsMessageTest.this.exchange = exchange;
					TwoEndpointsMessageTest.this.notify();
				}
			}
		});
		synchronized (this) {
			if (exchange == null) {
				wait(5000);
			}
			assertNotNull(exchange.getInBoundMessage());
			System.out.println("testAsyncRequestMessage finished");
			System.out.println();
		}
	}

	/**
	 * remove the links from the router
	 * 
	 * @throws Exception
	 */
	public void tearDown() throws Exception {
		router.removeLink(endpointUuid1);
		router.removeLink(endpointUuid2);
		router.stop();
		RouterFactory.clear();
	}

}

