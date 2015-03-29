// Refactored from DefaultEndpoint
// Handles reporting to LCM

#define LOGGING

using Org.Coos.Module;
using Org.Coos.Messaging.Util;
using Org.Coos.Messaging;
using System.Collections;
using System.Threading;
using System;

namespace Org.Coos.Messaging.Impl
{
    public class LCMEndpointReport : ILCMEndpoint
    {

#if LOGGING
        ILog log = LogFactory.getLog(typeof(LCMEndpointReport).FullName);
#endif

        protected bool heartbeat;
        /// <summary>
        /// Schedules life cycle data from endpoint to LCM
        /// </summary>
        protected Timer timer;

        protected DefaultEndpoint endpoint;
        public DefaultEndpoint Endpoint
        {
            get { return this.endpoint; }
            set { this.endpoint = value; }
        }

        private EndpointStateManager stateManager;

        public LCMEndpointReport(DefaultEndpoint endpoint, EndpointStateManager stateManager)
        {
            this.endpoint = endpoint;
            this.stateManager = stateManager;
        }

        public void start()
        {
            registerLCM();
            heartbeat = true;
            startLCMHeartbeat();
        }

        /**
         * Method that registers endpoint into lifecycle manager.
         *
         * @throws EndpointException
         *             if registration fails
         */
        // JAVA  private void registerLCM() throws EndpointException {
        private void registerLCM()
        {

            // registration to LCM

            bool failIfError = false;

            string lcmRegReq = (string)endpoint.getProperties()[IEndpointPrimitives.PROP_LCM_REGISTRATION_REQUIRED];

            if ((lcmRegReq != null) && lcmRegReq.Equals("true"))
            {
                failIfError = true;
            }


#if LOGGING
        log.info("Registering endpoint: " + endpoint.getEndpointUri() + " to LifeCycleManager.");
#endif
            string pollingInterval = endpoint.getProperty(IEndpointPrimitives.PROP_LCM_POLLING_INTERVAL);
            IExchange ex;

            if (pollingInterval == null)
                ex = LCMEdgeMessageFactory.createRegisterEndpointExchange(stateManager.EndpointState,
                        stateManager.getChildStates(), IEndpointPrimitives.DEFAULT_LCM_POLLING_INTERVAL);
            else
                ex = LCMEdgeMessageFactory.createRegisterEndpointExchange(stateManager.EndpointState,
                        stateManager.getChildStates(), long.Parse(pollingInterval));

            InteractionHelper helper = new InteractionHelper(this.endpoint);
            ex = helper.request(IEndpointPrimitives.LIFE_CYCLE_MANGER_ADDRESS, ex);

            if (ex.getFaultMessage() != null)
            {

                if (failIfError)
                    throw new EndpointException("Registration to LifeCycleManager failed due to : " +
                        ex.getFaultMessage().getHeader(IMessagePrimitives.ERROR_REASON));
            }
            else
            {
                IMessage reply = ex.getInBoundMessage();

                if ((reply == null) ||
                        reply.getHeader(IMessagePrimitives.MESSAGE_NAME).Equals(CommonConstants.REPLY_NACK))
                {

                    if (failIfError)
                        throw new EndpointException("Registration to lifecycle manager failed");
                    // else
                    // log.
                }
            }
        }

        // JAVA private void unRegisterLCM() throws EndpointException {
        private void unRegisterLCM()
        {

#if LOGGING
        // unregistration to LCM
        log.info("UnRegistering endpoint: " + endpoint.getEndpointUri() + " from LifeCycleManager.");

#endif
            InteractionHelper helper = new InteractionHelper(this.endpoint);
            IExchange ex = LCMEdgeMessageFactory.createUnregisterEndpointExchange();
            ex = helper.request(IEndpointPrimitives.LIFE_CYCLE_MANGER_ADDRESS, ex);

            if (ex.getFaultMessage() != null)
            {
#if LOGGING
            log.info("Endpoint: " + endpoint.getEndpointUri() +
                ", Unregistration to LifeCycleManager failed due to: " +
                ex.getFaultMessage().getHeader(IMessagePrimitives.ERROR_REASON));
#endif
            }
            else
            {
                IMessage reply = ex.getInBoundMessage();

                if ((reply == null) ||
                        reply.getHeader(IMessagePrimitives.MESSAGE_NAME).Equals(CommonConstants.REPLY_NACK))
                {
#if LOGGING
                log.info("Endpoint: " + endpoint.getEndpointUri() +
                    ", Unregistration to lifecycle manager failed");
#endif

                }
            }

        }


        // JAVA private void reportChildren() throws EndpointException {
        public void reportChildren()
        {
#if LOGGING
        log.info("Reporting state: " + endpoint.getEndpointState() + " to LifeCycleManager.");

#endif
            IExchange ex = LCMEdgeMessageFactory.createSetChildrenStatesExchange(stateManager.ChildStates);
            InteractionHelper helper = new InteractionHelper(this.endpoint);
            ex = helper.request(IEndpointPrimitives.LIFE_CYCLE_MANGER_ADDRESS, ex);

            if (ex.getFaultMessage() != null)
            {
                throw new EndpointException("Children reply to LifeCycleManager failed due to :" +
                    ex.getFaultMessage().getHeader(IMessagePrimitives.ERROR_REASON));
            }

            IMessage reply = ex.getInBoundMessage();
            string signalName = reply.getHeader(IMessagePrimitives.MESSAGE_NAME);

            if (signalName.Equals(CommonConstants.REPLY_NACK))
            {
                throw new EndpointException("Children reply to lifecycle manager failed");
            }
        }

        public void reportState()
        {

            bool failIfError = false;

            string lcmRegReq = (string)endpoint.getProperties()[IEndpointPrimitives.PROP_LCM_REGISTRATION_REQUIRED];

            if ((lcmRegReq != null) && lcmRegReq.Equals("true"))
            {
                failIfError = true;
            }
#if LOGGING
        log.info("Sending state: " + endpoint.getEndpointState() + " to LifeCycleManager.");
#endif
            IExchange ex = LCMEdgeMessageFactory.createSetStateExchange(stateManager.EndpointState);
            InteractionHelper helper = new InteractionHelper(this.endpoint);
            ex = helper.request(IEndpointPrimitives.LIFE_CYCLE_MANGER_ADDRESS, ex);

            if (ex.getFaultMessage() != null)
            {

                if (failIfError)
                    throw new EndpointException("Pushing state to Lifecycle Mangager failed :" +
                        ex.getFaultMessage().getHeader(IMessagePrimitives.ERROR_REASON));
            }
            else
            {
                IMessage reply = ex.getInBoundMessage();
                string signalName = reply.getHeader(IMessagePrimitives.MESSAGE_NAME);

                if (signalName.Equals(CommonConstants.REPLY_NACK))
                {

                    if (failIfError)
                        throw new EndpointException("Pushing state to Lifecycle Manager failed.");
                    // else
                    // log.
                }
            }
        }

        // JAVA    public void reportChildState(string childName) throws EndpointException {
        public void reportChildState(string childName)
        {

            bool failIfError = false;

            string lcmRegReq = (string)endpoint.getProperties()[IEndpointPrimitives.PROP_LCM_REGISTRATION_REQUIRED];

            if ((lcmRegReq != null) && lcmRegReq.Equals("true"))
            {
                failIfError = true;
            }

            string childState = (string)stateManager.getChildStates()[childName];

#if LOGGING
        log.info("Sending state: " + childState + " of child: " + childName +
            " to LifeCycleManager.");
#endif

            IExchange ex = LCMEdgeMessageFactory.createRegisterEndpointChildExchange(childName,
                    childState);
            InteractionHelper helper = new InteractionHelper(this.endpoint);
            ex = helper.request(IEndpointPrimitives.LIFE_CYCLE_MANGER_ADDRESS, ex);

            if (ex.getFaultMessage() != null)
            {

                if (failIfError)
                    throw new EndpointException("Pushing child-state to Lifecycle Mangager failed :" +
                        ex.getFaultMessage().getHeader(IMessagePrimitives.ERROR_REASON));
            }
            else
            {
                IMessage reply = ex.getInBoundMessage();
                string signalName = reply.getHeader(IMessagePrimitives.MESSAGE_NAME);

                if (signalName.Equals(CommonConstants.REPLY_NACK))
                {

                    if (failIfError)
                        throw new EndpointException("Pushing child-state to Lifecycle Manager failed.");
                    // else
                    // log.
                }
            }
        }




        /// <summary>
        /// LCM heartbeat, reports state to LCM
        /// </summary>
        /// <param name="state"></param>
        private void hearbeatTask(object state)
        {
            Thread reportStateThread = new Thread(delegate()
            {

                try
                {
                    reportState();
                }
                catch (EndpointException e)
                {
                }
            });

            reportStateThread.Start();

        }

        private void startLCMHeartbeat()
        {
            // JAVA   timer = new Timer("LCMTimer", true);
            TimerCallback tck = this.hearbeatTask;
           
           long delay = long.Parse(endpoint.getProperty(IEndpointPrimitives.PROP_LCM_HEARTBEAT_INTERVAL, "120000"));

            long ticks = delay * 10000; // 10000 = ticks pr. millisecond (100 nanosec. pr tick in .NET)

            TimeSpan delayTimespan = new TimeSpan(ticks);

            if (delay > 0)
            {
                timer = new Timer(tck, null, delayTimespan, delayTimespan);
                


                //TimerTask task = new TimerTask() {

                //        @Override public void run() {
                //            threadPool.execute(new Runnable() {

                //                    public void run() {

                //                        try {
                //                            reportState();
                //                        } catch (EndpointException e) {
                //                        }
                //                    }
                //                });
                //        }
                //    };
                //timer.schedule(task, delay, delay);
            }
        }

        public void stop()
        {
            if (timer != null)
                timer.Dispose();

            string lcmRegEnabled = endpoint.getProperty(IEndpointPrimitives.PROP_LCM_REGISTRATION_ENABLED);

            if ((lcmRegEnabled == null) || !lcmRegEnabled.ToLower().Equals("false"))
            {

                try
                {
                    unRegisterLCM();
                }
                catch (EndpointException e)
                {
#if LOGGING
                log.error("EndpointException caught while stopping endpoint.", e);
#endif
                }
                catch (Exception e)
                {
#if LOGGING      
                    log.error("Unknown Exception caught while stopping endpoint.", e);
#endif
                }
            }
        }

        public void reportStateInBackground()
        {
            Thread reportThread = new Thread(delegate()
            {
                try
                {
                    reportState();
                }
                catch (EndpointException e)
                {
#if LOGGING
                                log.warn(
                                            "Exception caught by processMessage(). Message " +
                                            EdgeLCMMessageFactory.EDGE_REQUEST_STATE, e);
 
#endif
                }
            });

            reportThread.Start();
        }

        public void reportChildStateInBackground(string childAddress)
        {

            Thread reportThread = new Thread(delegate()
            {
                try
                {
                    reportChildState(childAddress);
                }
                catch (EndpointException e)
                {
#if LOGGING
                                        log.warn(
                                            "Exception caught by processMessage(). Message " +
                                            EdgeLCMMessageFactory.EDGE_REQUEST_CHILD_STATE, e);
#endif
                }
            });
            reportThread.Start();
        }


        public void reportChildrenInBackground()
        {
                    Thread reportThread = new Thread(delegate () {
                        try {
                                        reportChildren();
                                    } catch (EndpointException e) {
#if LOGGING
                                        log.warn(
                                            "Exception caught by processMessage(). Message " +
                                            EdgeLCMMessageFactory.EDGE_REQUEST_CHILD_STATE, e);
#endif
                                        }});
                        reportThread.Start();
        }

    }
}