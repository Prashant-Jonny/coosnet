namespace Org.Coos.Messaging
{
    public interface ILCMEndpoint
    {
        /**
         * Reports the state of your endpoint to the LCM
         */
        // JAVA public void reportState() throws EndpointException;
        void reportState();

        /**
         * Reports the state of the given child to the LCM
         *
         * @param childName
         */
        // JAVA public void reportChildState(string childName) throws EndpointException;
        void reportChildState(string childName);

        /**
         * If this Endpoint contains a complex structure this method returns the
         * State of a inner child
         *
         * @param childName
         * @return the State
         */
    }
}