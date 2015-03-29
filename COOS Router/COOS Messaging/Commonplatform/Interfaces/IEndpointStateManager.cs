// Refactored from IEndpoint
namespace Org.Coos.Messaging.Impl
{
    public interface IEndpointStateManager
    {
        /**
     * Returns the endpoint state, Can be one of: STATE_RUNNING,
     * STATE_STARTUP_FAILED, STATE_STOPPING, STATE_STARTING, STATE_INSTALLED,
     * STATE_PAUSED, STATE_READY, STATE_UPDATING, STATE_UPGRADING,
     * STATE_UNINNSTALLED
     *
     * @return the state
     */
        string getEndpointState();

        /**
         * Sets the Endpoint state. Can be one of: STATE_RUNNING,
         * STATE_STARTUP_FAILED, STATE_STOPPING, STATE_STARTING, STATE_INSTALLED,
         * STATE_PAUSED, STATE_READY, STATE_UPDATING, STATE_UPGRADING,
         * STATE_UNINNSTALLED
         *
         * @param endpointState
         */
        void setEndpointState(string endpointState);

        /**
         * If this Endpoint contains a complex structure this method allows for
         * setting the State on a inner child
         *
         * @param childName
         * @param state
         */
        void setChildEndpointState(string childName, string state);


        string getChildEndpointState(string childName);

    }
}