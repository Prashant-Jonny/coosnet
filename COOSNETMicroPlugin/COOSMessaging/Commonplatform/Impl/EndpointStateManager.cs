using System.Collections;

namespace Org.Coos.Messaging.Impl
{
    public class EndpointStateManager : IEndpointStateManager
    {


        /// <summary>
        /// State to endpoint, initialized to STATE_READY
        /// </summary>
        private string endpointState = IEndpointPrimitives.STATE_READY;
        public string EndpointState
        {
            get { return this.endpointState; }
        }



        private Hashtable childStates = new Hashtable();
        public System.Collections.Hashtable ChildStates
        {
            get
            {
                return childStates;
            }

            set
            {
                this.childStates = value;
            }

        }

        /// <summary>
        /// Statemanager assosiated endpoint
        /// </summary>
        private IEndpoint endpoint;

        public EndpointStateManager(IEndpoint endpoint, string state)
        {
            endpointState = state;
            this.endpoint = endpoint;
        }

        public string getChildEndpointState(string childName)
        {
            return (string)childStates[childName];
        }

        public string getEndpointState()
        {
            return endpointState;
        }

        public bool isStateRunning()
        {

            if (endpointState.Equals(IEndpointPrimitives.STATE_RUNNING))
            {
                return true;
            }

            if (endpointState.Equals(IEndpointPrimitives.STATE_STARTING) || endpointState.Equals(IEndpointPrimitives.STATE_INSTALLED) ||
                    endpointState.Equals(IEndpointPrimitives.STATE_PAUSED) || endpointState.Equals(IEndpointPrimitives.STATE_READY) ||
                    endpointState.Equals(IEndpointPrimitives.STATE_STARTUP_FAILED) ||
                    endpointState.Equals(IEndpointPrimitives.STATE_UNINNSTALLED) || endpointState.Equals(IEndpointPrimitives.STATE_UPDATING) ||
                    endpointState.Equals(IEndpointPrimitives.STATE_UPGRADING))
                return false;

            return true;
        }

        public void setChildStates(Hashtable childStates)
        {
            this.childStates = childStates;
        }

        public Hashtable getChildStates()
        {
            return childStates;
        }


        public void setEndpointState(string endpointState)
        {
            this.endpointState = endpointState;

            
        }

        public void setChildEndpointState(string childName, string state)
        {
            childStates.Add(childName, state);
        }

        /// <summary>
        /// Changes endpoint state and process deferred messages
        /// </summary>
        /// <param name="state"></param>
        public void StateTransition(string state)
        {
            this.setEndpointState(state);
            ((DefaultEndpoint)endpoint).processDefereQueue();
        }

    }
}