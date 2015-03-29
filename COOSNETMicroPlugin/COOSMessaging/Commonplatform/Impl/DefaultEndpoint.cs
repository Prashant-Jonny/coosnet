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

#define LOGGING
#define LCM
#define THREADPOOL
#define COCONTAINER
//#define SIGNLETHREAD
//#define TRY_INCOMPATIBLE_CODE
//#define WAIT_NOTIFY_NOTIFYALL_CODE

using Org.Coos.Messaging;
using Org.Coos.Messaging.Util;
using System.Collections;
using Org.Coos.Module;
using System;
using System.Threading;

namespace Org.Coos.Messaging.Impl
{

//import org.coos.messaging.AsyncCallback;
//import org.coos.messaging.ConnectingException;
//import org.coos.messaging.Consumer;
//import org.coos.messaging.Endpoint;
//import org.coos.messaging.EndpointException;
//import org.coos.messaging.Exchange;
//import org.coos.messaging.ExchangePattern;
//import org.coos.messaging.InteractionHelper;
//import org.coos.messaging.Link;
//import org.coos.messaging.Message;
//import org.coos.messaging.Notification;
//import org.coos.messaging.Plugin;
//import org.coos.messaging.Processor;
//import org.coos.messaging.ProcessorException;
//import org.coos.messaging.Producer;
//import org.coos.messaging.Service;
//import org.coos.messaging.SubscriptionFilter;
//import org.coos.messaging.util.ExecutorService;
//import org.coos.messaging.util.Executors;
//import org.coos.messaging.util.Log;
//import org.coos.messaging.util.LogFactory;
//import org.coos.messaging.util.URIHelper;
//import org.coos.messaging.util.URIProtocolHelper;
//import org.coos.messaging.util.UuidGenerator;
//import org.coos.messaging.util.UuidHelper;

//import org.coos.module.CommonConstants;
//import org.coos.module.EdgeLCMMessageFactory;
//import org.coos.module.EdgeMessageProperties;
//import org.coos.module.LCMEdgeMessageFactory;

//import java.util.Enumeration;
//import java.util.Hashtable;
//import java.util.Timer;
//import java.util.TimerTask;
//import java.util.Vector;


/**
 * @author Knut Eilif Husa, Tellu AS
 * @author anders
 */
public abstract class DefaultEndpoint : DefaultProcessor, IEndpoint {




    private string uri;
    private string endpointUuid;

    private Plugin plugin;
    
    private ArrayList aliases = new ArrayList();
    
    private Hashtable outLinks = new Hashtable();
    
    /// <summary>
    /// Keeps track of services offered by endpoint (f.ex. a producer)
    /// </summary>
    private ArrayList services = new ArrayList();
    
    
    /// <summary>
    /// Exchanges contains exchange id. to exchange that is processed asynchronously and for messages of OutIn/RobustOutOnly
    /// </summary>
    private Hashtable exchanges = new Hashtable();

    /// <summary>
    /// Holds callbacks for asynchronous message processing
    /// </summary>
    private Hashtable callbacks = new Hashtable();
    
    private UuidGenerator uuidGenerator = new UuidGenerator();
    
    private int maxPoolSize = IEndpointPrimitives.DEFAULT_MAX_POOL_SIZE;
#if THREADPOOL
    protected IExecutorService threadPool;
#endif
    private long timeout;
    
    
    protected ArrayList deferQueue = new ArrayList();
#if LOGGING
   // JAVA protected ILog log = LogFactory.getLog(this.getClass().getName());
    protected ILog log = LogFactory.getLog(typeof(DefaultEndpoint).FullName);
   
#endif
    /// <summary>
    /// Used to regulate access to exchange
    /// </summary>
    AutoResetEvent exchangeSignal = new AutoResetEvent(false);
   

    public IProcessor DefaultProcessor
    {
        get
        {
            return this;
        }

    }

    /// <summary>
    /// Report endpoint state to LCM at regular intervals if enabled
    /// </summary>
    LCMEndpointReport LCMReport;
    
    /// <summary>
    /// State manager keeps track of endpoint state
    /// </summary>
    public EndpointStateManager StateManager;
    
    protected DefaultEndpoint() {
    }

    public DefaultEndpoint(string uri, IProcessor processor) {

        // This is for test purposes since this constructor only is called in
        // tests
        this.uri = uri;
        outLinks.Add("coos", processor);
#if THREADPOOL
        threadPool = Executors.newThreadPool(2);
#endif
        uuidGenerator = new UuidGenerator("xId-" +
                ((getName() == null) ? getEndpointUuid() : getName()));

        EndpointStateManager StateManager = new EndpointStateManager(this, IEndpointPrimitives.STATE_RUNNING);

    }

     public override void setName(string name) {

        if ((name != null) && !UuidHelper.isUuid(name)) {
            aliases.Add(name);
        }

        base.setName(name);
    }

    public virtual IConsumer createConsumer() {

        // TODO Auto-generated method stub
        return null;
    }

    public virtual IProducer createProducer() {

        // TODO Auto-generated method stub
        return null;
    }

    #region EndpointUuid

    public virtual string EndpointUuid
    {
        get { return endpointUuid; }
        set
        {
            endpointUuid = UuidHelper.getQualifiedUuid(endpointUuid);
            this.endpointUuid = value;
#if LOGGING
            log.putMDC("UUID", endpointUuid);
#endif
        }
    }
    
    
    public virtual string getEndpointUuid() {
        return endpointUuid;
    }

    public virtual void setEndpointUuid(string endpointUuid) {
        endpointUuid = UuidHelper.getQualifiedUuid(endpointUuid);
        this.endpointUuid = endpointUuid;
#if LOGGING
        log.putMDC("UUID", endpointUuid);
#endif
        }

    #endregion


    public override void setProperties(Hashtable properties) {
        this.properties = properties;

        string timoutStr = (string) properties[IEndpointPrimitives.PROP_EXCHANGE_TIMEOUT];

        if (timoutStr != null) {
            // JAVA timeout = Long.parseLong(timoutStr);
            timeout = long.Parse(timoutStr);
        } else {
            timeout = IEndpointPrimitives.DEFAULT_TIMEOUT;
        }

        //maxPoolSize = Integer.parseInt(getProperty(PROP_MAX_POOL_SIZE,
        //            string.valueOf(DEFAULT_MAX_POOL_SIZE)));

        maxPoolSize = int.Parse(getProperty(IEndpointPrimitives.PROP_MAX_POOL_SIZE, ((int)IEndpointPrimitives.DEFAULT_MAX_POOL_SIZE).ToString()));

    }

    #region Timeout
    public virtual long Timeout
    {
        set { this.timeout = value; }
    }
    public virtual void setTimeout(long timeout) {
        this.timeout = timeout;
    }
    #endregion

    public virtual void setMaxPoolSize(int maxPoolSize) {
        this.maxPoolSize = maxPoolSize;
    }

    public virtual IExchange createExchange() {
        IExchange exchange = new DefaultExchange(new ExchangePattern(ExchangePattern.OutIn));

        return exchange;
    }

    public virtual IExchange createExchange(ExchangePattern pattern) {
        IExchange exchange = new DefaultExchange(pattern);

        return exchange;
    }

    #region EndpointUri

    public virtual string EndpointUri
    {
        get { return uri; }
        set
        {
            this.uri = value;
#if LOGGING
            log.putMDC("URI", value);
#endif
        }
    }
    public virtual string getEndpointUri() {
        return uri;
    }

    public virtual void setEndpointUri(string endpointUri) {
        this.uri = endpointUri;
#if LOGGING
        log.putMDC("URI", endpointUri);
#endif
        }

    #endregion

    public string createUuid() {
        return uuidGenerator.generateId();
    }

    

    
    ///<summary>Sync processing of messages</summary>
    ///<param name="exchange">exchange to be processed</param>
    public virtual IExchange processExchange(IExchange exchange) {

        // Check if exchange is already processed

        if (exchange.isProcessed()) {
            exchange.setException(new EndpointException(
                    "Exchange can not be reused. Is already processed"));
#if LOGGING
            log.warn("Endpoint: " + uri + ", Exception processing exchange, already processed: " +
                exchange);
#endif
            return exchange;
        }

        // Prepare exchange, get next processor for further "processing" on message
        IProcessor processor = prepareExchange(exchange);
#if LOGGING
        log.debug("Endpoint: " + uri + ", Processing outgoing exchange: " + exchange);
#endif

        // If we got a fault now (could happend in prepare exchange) return
        if (exchange.getFaultMessage() != null) {
            return exchange;
        }

        // Critical region, only one thread at a time can access exchange now
        lock (exchange) {

            try {
                processor.processMessage(exchange.getOutBoundMessage());
            } catch (ProcessorException e) {
                createExchangeFault(exchange,e.Message,String.Empty);
                exchange.setException(e);
#if LOGGING
                log.warn("Endpoint: " + uri + ", Exception processing exchange: " + exchange);
#endif
                return exchange;
            }

            ExchangePattern xp = exchange.getPattern();

            #region ExchangePattern OutIn, RobustOutOnly, wait until timeout
            if (xp.equals(ExchangePattern.OutIn) || xp.equals(ExchangePattern.RobustOutOnly)) {


                try {
                   // JAVA exchange.wait(timeout);
                    // This thread is now put to sleep waiting for exchange signal
                    bool signalReceived = exchangeSignal.WaitOne((int)timeout,false);

                    if (!exchange.isProcessed()) {
      
#if LOGGING
                        log.warn("Endpoint: " + uri + ", exchange: " + exchange + " timed out (" +
                            timeout + " ms).");
#endif
                        createExchangeFault(exchange,"Exchange timeout","504");
                       
                    }
                } catch (Exception e) {
#if LOGGING
                    log.warn("Endpoint: " + uri + ", exchange: " + exchange +
                        " interrupted. Ignored.", e);
#endif
                  }

            }
            #endregion

        }

        return exchange;
    }

    private void createExchangeFault(IExchange exchange, string errorReason, string errorCode)
    {
        IMessage fault = new DefaultMessage();
        fault.setReceiverEndpointUri(getEndpointUri());
        fault.setHeader(IMessagePrimitives.TYPE_MSG, IMessagePrimitives.TYPE_ERROR);
        fault.setHeader(IMessagePrimitives.ERROR_REASON, errorReason);
        fault.setHeader(IMessagePrimitives.EXCHANGE_ID, exchange.getExchangeId());
        exchange.setFaultMessage(fault);
    }

    /**
     * Simple method to send a message to a receiver, using a set exchange pattern. Returns the inbound Message object from the used exchange.
     * @param msg
     * @param receiver - coos url of the receiver
     * @param exchangePattern
     * @return
     */
    public virtual IMessage sendMessage(IMessage msg, string receiver, string exchangePattern) {
        IExchange e = createExchange(new ExchangePattern(exchangePattern));
        msg.setReceiverEndpointUri(receiver);
        e.setOutBoundMessage(msg);

        IExchange response = processExchange(e);

        return response.getInBoundMessage();
    }

    /**
     * Prepares the exchange before it is processed. This includes checking that
     * sender and receiver adresses are resolved
     *
     * @param exchange
     *            the exchange
     */
    private IProcessor prepareExchange(IExchange exchange) {

        try {

            //            if (!isStateRunning()&&!endpointState.equals(STATE_STARTING)) {
            //                throw new EndpointException("Endpoint not able to handle exchanges. Endpoint State is: "
            //                        + endpointState);
            //            }

            string senderUri = exchange.getOutBoundMessage().getSenderEndpointUri();
            URIHelper helper;

            if ((senderUri == null) || senderUri.Equals("")) {
                helper = new URIHelper(uri);
            } else {
                helper = new URIHelper(senderUri);
            }

            helper.setEndpoint(endpointUuid);
            exchange.getOutBoundMessage().setSenderEndpointUri(helper.getEndpointUri());

            // Local variabel with same name as uri in class -> ambiguity

            string uriReceiverEndpoint = exchange.getOutBoundMessage().getReceiverEndpointUri();

            if (exchange.getExchangeId() == null) {
                exchange.setExchangeId(uuidGenerator.generateId());
            }

            ExchangePattern xPattern = exchange.getPattern();

            // Info: http://telenorobjects.onjira.com/wiki/display/coos/About#About-Endpointexchangepatterns
            // OutIn = Request-Response pattern, producer expects response message
            // RobustOutOnly = Request, but don't expect response, notification if message not delivered
            if (xPattern.equals(ExchangePattern.OutIn) || xPattern.equals(ExchangePattern.RobustOutOnly)) 
                exchanges.Add(exchange.getExchangeId(), exchange);
            

            if (exchange.getPattern().equals(ExchangePattern.InOut)) {

                //If this is a reply we search through the message for header values that are to be passed
                //along with the reply
                exchange.getOutBoundMessage().setHeader(IMessagePrimitives.EXCHANGE_ID,
                    exchange.getInBoundMessage().getHeader(IMessagePrimitives.EXCHANGE_ID));

                IMessage outMsg = exchange.getOutBoundMessage();
                IMessage inMsg = exchange.getInBoundMessage();
                
                foreach (string key in inMsg.getHeaders().Keys)
               
                    if (key.IndexOf(IMessagePrimitives.EXCHANGE_PREFIX) == 0) {
                        outMsg.setHeader(key, exchange.getInBoundMessage().getHeader(key));
                    }
                

            } else {
                exchange.getOutBoundMessage().setHeader(IMessagePrimitives.EXCHANGE_ID,
                    exchange.getExchangeId());
            }

            exchange.getOutBoundMessage().setHeader(IMessagePrimitives.EXCHANGE_PATTERN,
                exchange.getPattern().toString());

            if (exchange.getOutBoundMessage().getType() == null) {
                exchange.getOutBoundMessage().setHeader(IMessagePrimitives.TYPE, IMessagePrimitives.TYPE_MSG);
            }

            if (xPattern.equals(ExchangePattern.RobustOutOnly)) {
                long rdTimeout = timeout / 2;
                exchange.getOutBoundMessage().setHeader(IMessagePrimitives.ROBUST_DELIVERY_TIME,
                    rdTimeout.ToString());
                exchange.getOutBoundMessage().setHeader(IMessagePrimitives.ROBUST_DELIVERY_ACK_URI,
                    helper.getEndpointUri());
            }

            string protocol = URIProtocolHelper.getProtocol(uriReceiverEndpoint);

            IProcessor processor = resolveOutgoingProcessor(protocol);

       // JAVA     if (protocol.startsWith("coos")) {

                 if (protocol.IndexOf("coos") == 0) {

                helper = new URIHelper(uriReceiverEndpoint);
                exchange.getOutBoundMessage().setReceiverEndpointName(helper.getEndpoint());
                exchange.getOutBoundMessage().setSenderEndpointName(((getName() == null)
                        ? getEndpointUuid() : getName()));

            }

            return processor;

        } catch (Exception e) {
#if LOGGING
            log.warn("Exception caught by prepareExchange().", e);
#endif
            exchange.setFaultMessage(new DefaultMessage().setHeader(IMessagePrimitives.ERROR_REASON,
                    "Exception: " + e.StackTrace + ", Message: " + e.Message));
            exchange.setException(e);
        }

        return null;

    }

    

   // JAVA protected IProcessor resolveOutgoingProcessor(string protocol) throws EndpointException {
        protected IProcessor resolveOutgoingProcessor(string protocol)  {
        
    IProcessor processor = (IProcessor) outLinks[protocol];

        if (processor == null) {
            throw new EndpointException("No channel defined for protocol: " + protocol);
        }

        return processor;
    }

    
    ///<summary>Async processing of outbound messages</summary>
    ///<param name="callback">callback to be called after processing</param>
    ///<param name="exchange">exchange to be processed</param>
    public void processExchange(IExchange exchange, IAsyncCallback callback) {

#if LOGGING
        log.debug("Endpoint: " + uri + ", Processing outgoing exchange: " + exchange);
#endif

        // Prepare exchange and fetch processor for further message processing
        IProcessor processor = prepareExchange(exchange);

        // Check for fault message, if fault then immediatly call callback
        if (exchange.getFaultMessage() != null) {
            callback.processExchange(exchange);

            return;
        }

        // Keep track of which exchanges that are to be processed asynchronously
        exchanges.Add(exchange.getExchangeId(), exchange);
        callbacks.Add(exchange.getExchangeId(), callback);

        // Now, only one thread have access to exchange while processing
        lock (exchange) {

            try {
                processor.processMessage(exchange.getOutBoundMessage());
            } catch (ProcessorException e) {
                IMessage fault = createFault(exchange, e);
                processMessage(fault);

            }
        }
    }

    private  IMessage createFault(IExchange exchange, ProcessorException e)
    {
        IMessage fault = new DefaultMessage();
        fault.setHeader(IMessagePrimitives.TYPE_MSG, IMessagePrimitives.TYPE_ERROR);
        fault.setHeader(IMessagePrimitives.ERROR_REASON, e.Message);
        fault.setHeader(IMessagePrimitives.EXCHANGE_ID, exchange.getExchangeId());
        fault.setHeader(IMessagePrimitives.EXCHANGE_PATTERN, exchange.getPattern().toString());
        return fault;
    }


    
    private void processErrorMsg(string xId, IMessage retMsg)
    {
        // First if, Processing of exchanges that was called with asynchronous processing or OutIn RobustOutOnly exchange pattern
         if ((xId != null) && exchanges.Contains(xId)) {
                     IExchange exchange = (IExchange) exchanges[xId];
                    exchanges.Remove(xId);
#if LOGGING
                    log.warn("Endpoint: " + uri + ", Processing incoming exchange: " + exchange +
                        ": fault :" + retMsg);

#endif
                    exchange.setFaultMessage(retMsg);
                    exchange.setProcessed(true);

                    if (callbacks.Contains(xId)) {
#if THREADPOOL
                        //threadPool.execute(new Runnable() {
                        //        public void run() {

                        //            // this is an async return
                        //            IAsyncCallback callback = (IAsyncCallback) callbacks.Remove(xId);
                        //            callback.processExchange(exchange);
                        //        }
                        //    });

                        CallbackTask cbt = new CallbackTask(callbacks, xId, exchange);
                        threadPool.execute(cbt);

#endif
                       
#if SINGLE_THREAD
                       
                         // For simplicity at the moment, run no thread pool
                        Thread callbackThread = 
                         new Thread(() => {
                               

                                    // this is an async return
                                    IAsyncCallback callback = (IAsyncCallback) callbacks[xId];
                                    callbacks.Remove(xId);
                                    callback.processExchange(exchange);
                                });

                        callbackThread.Start();
#endif

                    } else {
#if WAIT_NOTIFY_NOTIFYALL_CODE
                        // this is a sync return
                        // todo when moved to java 1.6 can check if current thread holds lock
                        // New thread is only necessary in that case
                        //new Thread(new Runnable() {
                        //        public void run() {

                        //            synchronized (exchange) {
                        //                exchange.notifyAll();
                        //            }
                        //        }
                        //    }).start();

                        // FIX FIX!!
#endif
                        exchangeSignal.Set();

                    }
                } else {
#if LOGGING
                    log.warn("Endpoint: " + uri + ", Error message:" + retMsg);
#endif
                         }

            }
 


    private void processMessageUtility(string xId, IMessage msg, IMessage retMsg, ExchangePattern xp, IConsumer consumer, string xpattern)
{
     // Message handling
                IExchange exchange = null;

                if ((xId != null) && exchanges.Contains(xId) && !xp.isOutBoundInitiated())
                {

                    #region Seems like processing callbacks
                    // this is a return or a message to same endpoint.
                    IExchange exchange2 = (IExchange) exchanges[xId];
                    exchanges.Remove(xId);
#if LOGGING
                    log.debug("Endpoint: " + uri + ", Processing incoming exchange: " + exchange2);
#endif
                    if (xp.equals(ExchangePattern.InOut)) 
                        exchange2.setInBoundMessage(retMsg);
                    

                    exchange2.setProcessed(true);

                    // If this exchange is to be processed async., then call callback on thread
                    if (callbacks.Contains(xId)) {
#if THREADPOOL
                        //threadPool.execute(new Runnable() {
                        //        public void run() {

                        //            // this is an async return
                        //            AsyncCallback callback = (AsyncCallback) callbacks.remove(xId);
                        //            callback.processExchange(exchange2);
                        //        }
                        //    });


                        CallbackTask cbt = new CallbackTask(callbacks, xId, exchange2);
                        threadPool.execute(cbt);
#endif

#if SINGLETHREAD
                        Thread callbackThread = new Thread(() =>
                            {
                                IAsyncCallback callback;
                                lock (callbacks) // Mutually exclusive thread access to callbacks
                                {
                                    callback = (IAsyncCallback)callbacks[xId];
                                    callbacks.Remove(xId);
                                }
                                callback.processExchange(exchange2);
                            });

                        callbackThread.Start();
#endif

                    #endregion

                    } else {

                        // No callback found for exchange, then we have sync. processing

                        // this is a sync return
                        // todo when moved to java 1.6 can check if current thread holds lock
                        // New thread is only necessary in that case

                        // 24 oct 10; tried to search for exchange2.wait, but did not find it
                        // why notify threads that is not locked on exchange2?

#if TRY_INCOMPATIBLE_CODE
                        new Thread(new Runnable() {
                                public void run() {

                                    synchronized (exchange2) {
                                        exchange2.notifyAll();
                                    }
                                }
                            }).start();


                       
#endif
                        exchangeSignal.Set();

                    }
                } else if (consumer != null)
                {

                    #region LCM processing
                    if (msg.getHeader(IMessagePrimitives.MESSAGE_NAME).Equals(EdgeLCMMessageFactory.EDGE_REQUEST_STATE)) {

#if THREADPOOL

                        LCMReportStateTask lcmrst = new LCMReportStateTask(LCMReport,log);
                        threadPool.execute(lcmrst);

                        //threadPool.execute(new Runnable() {
                        //        public void run() {

                        //            try {
                        //                reportState();
                        //            } catch (EndpointException e) {
                        //                log.warn(
                        //                    "Exception caught by processMessage(). Message " +
                        //                    EdgeLCMMessageFactory.EDGE_REQUEST_STATE, e);
                        //            }
                        //        }
                        //    });
#endif

#if SINGLETHREAD
                        // We got a EDGE request for state, report state on a seperate thread
                        
                        LCMReport.reportStateInBackground();
#endif
                        
                        return;
                    } else if (msg.getHeader(IMessagePrimitives.MESSAGE_NAME).Equals(
                                EdgeLCMMessageFactory.EDGE_REQUEST_CHILD_STATE)) {
                         string childAddress = (string) msg.getBodyAsProperties()[
                                EdgeMessageProperties.EDGE_PROP_CHILD_NAME];

#if THREADPOOL
                        //threadPool.execute(new Runnable() {
                        //        public void run() {

                        //            try {
                        //                reportChildState(childAddress);
                        //            } catch (EndpointException e) {
                        //                log.warn(
                        //                    "Exception caught by processMessage(). Message " +
                        //                    EdgeLCMMessageFactory.EDGE_REQUEST_CHILD_STATE, e);
                        //            }
                        //        }
                        //    });


                         LCMReportChildStateTask lcmrcst = new LCMReportChildStateTask(LCMReport,log,childAddress);
                         threadPool.execute(lcmrcst);
#endif

#if SINGLETHREAD
                         LCMReport.reportChildStateInBackground(childAddress);
#endif

                        
                        return;
                    } else if (
                        msg.getHeader(IMessagePrimitives.MESSAGE_NAME).Equals(
                                EdgeLCMMessageFactory.EDGE_REQUEST_CHILDREN) &&
                            (msg.getBodyAsProperties()[
      
                        EdgeMessageProperties.EDGE_PROP_CHILDREN] == null)) {
                        
#if THREADPOOL


                            LCMReportChildrenTask lcmrct = new LCMReportChildrenTask(LCMReport, log);
                            threadPool.execute(lcmrct);

                        //threadPool.execute(new Runnable() {
                        //        public void run() {

                        //            try {
                        //                reportChildren();
                        //            } catch (EndpointException e) {
                        //                log.warn(
                        //                    "Exception caught by processMessage(). Message " +
                        //                    EdgeLCMMessageFactory.EDGE_REQUEST_CHILDREN, e);
                        //            }
                        //        }
                        //    });

#endif

#if SINGLETHREAD
                                        LCMReport.reportChildrenInBackground();
#endif

                        
                        return;
                                    }
                    #endregion

                    // this is an initiating request, will only be handled in
                    // state running, else it will be deferred to
                    // the endpoint reaches state running
                    if (checkDefer(msg))
                        return;

                    if (xpattern.Equals(ExchangePattern.OutIn)) {
                        exchange = createExchange(new ExchangePattern(ExchangePattern.InOut));
                    } else {

                        // It seems like this section handles MEP RobustOutOnly, but returning
                        // a delivery success message in a confirmation message back to sender

                        // default behaviour is inOnly
                        if (xpattern.Equals(ExchangePattern.RobustOutOnly))
                        {
                            #region RobustOutOnly
                            IMessage confMsg = new DefaultMessage();
                            confMsg.setHeader(IMessagePrimitives.DELIVERY_STATUS, IMessagePrimitives.DELIVERY_SUCCESS);

                            IExchange e = createExchange(new ExchangePattern(
                                        ExchangePattern.RobustInOnly));
                            e.setExchangeId(msg.getHeader(IMessagePrimitives.EXCHANGE_ID));
                            confMsg.setReceiverEndpointUri(msg.getHeader(
                                    IMessagePrimitives.ROBUST_DELIVERY_ACK_URI));
                            e.setOutBoundMessage(confMsg);
#if LOGGING
           
                            log.debug("Endpoint: " + uri + ", Sending robust ack: " + e);
#endif
                            processExchange(e);
                            #endregion
                        }

                        exchange = createExchange(new ExchangePattern(ExchangePattern.InOnly));
                    }

                    #region Start processing of message in consumer, starts in a thread
                    // It seems like we forward the exchange to consumer here for further processing
                    if (exchange != null) {
                        exchange.setInBoundMessage(msg);
                        exchange.setExchangeId(xId);

                        IExchange exchange1 = exchange; // Why change variables here?
                        IConsumer consumer1 = consumer;

#if THREADPOOL

                        ConsumerTask ct = new ConsumerTask(consumer1, exchange1, log, uri);
                        threadPool.execute(ct);

                        //threadPool.execute(new Runnable() {
                        //        public void run() {

                        //            // Todo might insert e semaphore here to control
                        //            // concurrent access to consumer
                        //            log.debug(
                        //                "Endpoint: " + uri + ", Processing incoming exchange: " +
                        //                exchange1);
                        //            consumer1.process(exchange1);
                        //        }
                        //    });
#endif

#if SINGLETHREAD

                        Thread consumerThread = new Thread(delegate() {
#if LOGGING
                                        log.debug(
                                        "Endpoint: " + uri + ", Processing incoming exchange: " +
                                        exchange1);
#endif
                                    consumer1.process(exchange1);});


                        consumerThread.Start();

#endif
                    }
                    #endregion

                }
  }

    /// <summary>Processing of Inbound messages</summary>
    /// <param name="msg"></param>
    public override void processMessage(IMessage msg) {

        //if(coContainer != null)
#if COCONTAINER
        msg.setDeserializeClassLoader(coContainer);
#endif

        // Find out what type of message that we want to process
        string msgType = msg.getHeader(IMessagePrimitives.TYPE);

        bool typeMsg = msgType.Equals(IMessagePrimitives.TYPE_MSG);
        bool errorMsg = msgType.Equals(IMessagePrimitives.TYPE_ERROR);

        // Analysis:
        //          Cyclomatic compelxity : 35 for this methods thats only 69 lines
        // To do: 
        //          Should be refactored to make it more understandable/maintainable

        // It starts with an if statement, and it will only prosess
        // messages of 
        //  1.TYPE_MSG = "msg", 
        //  2.TYPE_ERROR = "error", 
        // so the first thing could be to rewrite it to just return without processing when these conditions are not true
        // Could have to boolean variables for each acceptable msg type that default endpoint will accept processing
        // bool typeMsg = msgType.Equals(IMessagePrimitives.TYPE_MSG)
        // bool errorMsg = msgType.Equals(IMessagePrimitives.TYPE_ERROR)

        if (!typeMsg && !errorMsg)
        {
            log.error("Message type unknown, expected msg- or error-type, got:" + msgType + " skipping msg processing.");
            return;
        }
        
       // if (msgType.Equals(IMessagePrimitives.TYPE_MSG) || msgType.Equals(IMessagePrimitives.TYPE_ERROR)) {

            if (msg.getName().Equals(DefaultNotification.NOTIFY)) {  // Why not use msgType = "Notification" instead?
                msg = new DefaultNotification(msg);
            }

            string xpattern = msg.getHeader(IMessagePrimitives.EXCHANGE_PATTERN);
            ExchangePattern xp = new ExchangePattern(xpattern);
             string xId = msg.getHeader(IMessagePrimitives.EXCHANGE_ID);



            IMessage retMsg = msg;

            // Tries to deserialize msg
            try {
                retMsg.getBody();
            } catch (Exception e) {
                retMsg.setHeader(IMessagePrimitives.TYPE, IMessagePrimitives.TYPE_ERROR);
                retMsg.setHeader(IMessagePrimitives.ERROR_REASON, "Deserialization error: " + e.Message);
            }

            // Consumer creation 
            IConsumer consumer = createConsumer();


            if (errorMsg)
                processErrorMsg(xId, retMsg);
            else if (typeMsg)
                processMessageUtility(xId, msg, retMsg, xp, consumer, xpattern);

               
       
    }

    protected bool checkDefer(IMessage msg) {

        if (ExchangePattern.InOut.Equals(msg.getHeader(IMessagePrimitives.EXCHANGE_PATTERN)) ||
                StateManager.isStateRunning()) {
            return false;
        }

#if LOGGING
        log.debug("State isn't Running (" + StateManager.EndpointState.ToString() + "). Deferring message to queue.");
#endif
        deferQueue.Add(msg);

        return true;
    }

    #region Services that the endpoint offers, f.ex. a producer

    public ArrayList Services
    {
        get { return services; }
    }
    
    public ArrayList getServices() {
        return services;
    }

    #endregion
   
    public virtual void initializeEndpoint() {
        uuidGenerator = new UuidGenerator("xId-" +
                ((getName() == null) ? getEndpointUuid() : getName()));

#if THREADPOOL
        threadPool = Executors.newThreadPool(maxPoolSize);
#endif
        deferQueue.Clear();

        StateManager = new EndpointStateManager(this,IEndpointPrimitives.STATE_READY);


        
            try {
                preStart();
                start();
                postStart();
            } catch (Exception e) {
#if LOGGING
                log.error("Startup failed.", e);
#endif
                StateManager.setEndpointState(IEndpointPrimitives.STATE_STARTUP_FAILED);
            }
        
    }

    // JAVA protected void preStart() throws EndpointException {
    protected void preStart() {
            
    string s = (!name.Equals(endpointUuid)) ? ("(name=" + name + ")") : "";
#if LOGGING
        log.info("Starting endpoint: " + endpointUuid + s);
#endif

    StateManager.StateTransition(IEndpointPrimitives.STATE_STARTING);

        // if (producer instanceof Service && !(producer instanceof Endpoint)) {
        // services.addElement(producer);
        // ((Service) producer).start();
        // }
    }

   
    public virtual void start()  {
          
    
    }

    // JAVA protected void postStart() throws EndpointException {
    protected void postStart()  {
            
    createProducer();

    StateManager.StateTransition(IEndpointPrimitives.STATE_RUNNING);

        // register to the life cycle manager
    string lcmRegEnabled = null;
    
        lcmRegEnabled = getProperty(IEndpointPrimitives.PROP_LCM_REGISTRATION_ENABLED);
    
        if ((lcmRegEnabled == null) || !lcmRegEnabled.ToLower().Equals("false")) {
            LCMReport = new LCMEndpointReport(this,StateManager);
            LCMReport.start();
        }

        string s = (!name.Equals(endpointUuid)) ? ("(name=" + name + ")") : "";
#if LOGGING
        log.info("Successfully started endpoint: " + endpointUuid + s);
#endif
        }

    

    //// JAVA public void stop() throws Exception {
    public virtual void stop()  {
          
   
    }

    public virtual void shutDownEndpoint() {

        if (!StateManager.isStateRunning()) {
            return;
        }
#if LOGGING
        log.info("Stopping endpoint: " + uri);
#endif       
        StateManager.StateTransition(IEndpointPrimitives.STATE_STOPPING);

        try {
            stop();
        } catch (Exception e) {
#if LOGGING      
log.error("Exception caught while stopping endpoint.", e);
#endif        
}
        foreach (IService service in services) {
        //for (int i = 0; i < services.Count; i++) {
        //    IService service = (IService) services[i];

            try {
                service.stop();
            } catch (Exception e) {
#if  LOGGING
                log.error("Exception caught while stopping endpoint.", e);
#endif
                }
        }

        // unregister to the life cycle manager
        if (LCMReport != null)
          LCMReport.stop();


#if THREADPOOL
        if (threadPool != null) // Null if endpoint was not started successfully

            // (ie couldn't connect)
            threadPool.stop();

#endif
        if (StateManager != null)
          StateManager.setEndpointState(IEndpointPrimitives.STATE_READY);
        
    }

    
    public void processDefereQueue()
    {
        if (StateManager.isStateRunning())
        {

            for (int i = 0; i < deferQueue.Count; i++)
            {
                IMessage message = (IMessage)deferQueue[i];
                processMessage(message);
            }

            deferQueue.Clear();
        }
    }

    // JAVA public void addLink(string protocol, Link link) throws ConnectingException {
    public virtual void addLink(string protocol, Link link)  {
    
        outLinks.Add(protocol, link);

    }

    public virtual void addAlias(string alias)
    {

        if ((alias != null) && !UuidHelper.isUuid(alias) &&
                UuidHelper.isValidAliasForUuid(alias, getEndpointUuid())) {
            aliases.Add(alias);
            updateAliases();
        } else {
#if LOGGING
            log.warn("Cannot assign this endpoint alias:" + alias +
                ". Only alias with segment 'dico', 'localcoos' or '" +
                UuidHelper.getSegmentFromEndpointNameOrEndpointUuid(getEndpointUuid()) +
                "' is allowed.");
#endif
            }
    }

    public virtual void removeAlias(string alias)
    {
        aliases.Remove(alias);
        updateAliases();
    }

    #region Aliases

    public virtual ArrayList Aliases
    {
        get { return aliases; }
    }
    
    public virtual  ArrayList getAliases() {
        return aliases;
    }

    #endregion

    private void updateAliases() {
        Link link = (Link) outLinks["coos"];

        if (link != null) {
            IMessage msg = new DefaultMessage("alias", IMessagePrimitives.TYPE_ALIAS);
            msg.setReceiverEndpointUri("coos://" + link.getDestinationUuid());
            msg.setBody(aliases);

            IExchange ex = createExchange(new ExchangePattern(ExchangePattern.OutOnly));
            ex.setOutBoundMessage(msg);
            processExchange(ex);
        }
    }

    public virtual IProcessor getDefaultProcessor()
    {
        return this;
    }

    
    
    public virtual Link getLink(string id)
    {
        return (Link) outLinks[id];
    }
   
    public virtual void removeLink(string id)
    {
        outLinks.Remove(id);

    }

    public virtual  void removeLinkById(string linkId) {
        // TODO Auto-generated method stub

    }

    public virtual  bool subscribe(ISubscriptionFilter filter) {
#if LOGGING
        log.debug("Endpoint: " + uri + " subscribing: " + filter);
#endif
        filter.setReceiverEndpointUri(IEndpointPrimitives.NOTIFICATION_BROKER_ADDRESS);
        filter.setSenderEndpointUri(uri);
        filter.setHeader(IMessagePrimitives.MESSAGE_NAME, ISubscriptionFilterPrimitives.SUBSCRIBE);

        IExchange ex = createExchange(new ExchangePattern(ExchangePattern.OutIn));
        ex.setOutBoundMessage(filter);
        processExchange(ex);

        if (ex.getFaultMessage() != null) {
            return false;
        }

        return true;
    }

    public virtual  void unsubscribe() {
#if LOGGING
        log.debug("Endpoint: " + uri + " unSubscribing all");
#endif
        IMessage msg = new DefaultMessage(ISubscriptionFilterPrimitives.UNSUBSCRIBE_ALL);
        msg.setReceiverEndpointUri(IEndpointPrimitives.NOTIFICATION_BROKER_ADDRESS);
        msg.setSenderEndpointUri(uri);

        IExchange ex = createExchange(new ExchangePattern(ExchangePattern.OutOnly));
        ex.setOutBoundMessage(msg);
        processExchange(ex);
    }

    public virtual void unsubscribe(ISubscriptionFilter filter)
    {
#if LOGGING
        log.debug("Endpoint: " + uri + " unSubscribing: " + filter);
#endif
        filter.setReceiverEndpointUri(IEndpointPrimitives.NOTIFICATION_BROKER_ADDRESS);
        filter.setSenderEndpointUri(uri);
        filter.setHeader(IMessagePrimitives.MESSAGE_NAME, ISubscriptionFilterPrimitives.UNSUBSCRIBE);

        IExchange ex = createExchange(new ExchangePattern(ExchangePattern.OutOnly));
        ex.setOutBoundMessage(filter);
        processExchange(ex);
    }

    public virtual void publish(INotification notification)
    {
#if LOGGING
        log.debug("Endpoint: " + uri + " publishing: " + notification);
#endif
        notification.setReceiverEndpointUri(IEndpointPrimitives.NOTIFICATION_BROKER_ADDRESS);
        notification.setSenderEndpointUri(uri);

        IExchange ex = createExchange(new ExchangePattern(ExchangePattern.OutOnly));
        ex.setOutBoundMessage(notification);
        processExchange(ex);
    }




    public virtual void setLinkAliases(ArrayList regAliases, Link outlink)
    {
        // Not implemented
    }

    #region Plugin

    public virtual Plugin Plugin
    {
        get { return plugin; }
        set { this.plugin = value; }
    }
    
    public virtual Plugin getPlugin()
    {
        return plugin;
    }

    public virtual void setPlugin(Plugin plugin)
    {
        this.plugin = plugin;
    }

    #endregion

    #region EndpointState
    public virtual string EndpointState
    {
        get { return StateManager.EndpointState; }
        set { StateManager.setEndpointState(value); }
    }
    
    public virtual string getEndpointState()
    {
        return StateManager.EndpointState;
    }

    public virtual void setEndpointState(string endpointState)
    {
        StateManager.setEndpointState(endpointState);
    }
    #endregion

}
}
