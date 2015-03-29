using java.lang;
using Org.Coos.Module;
using Org.Coos.Messaging.Util;

namespace Org.Coos.Messaging.Impl
{
    public class LCMReportChildStateTask : IRunnable
    {
        private LCMEndpointReport lcm;
        private ILog log;
        private string childAddress;

        public LCMReportChildStateTask(LCMEndpointReport lcm, ILog log, string childAddress)
        {
            this.lcm = lcm;
            this.log = log;
            this.childAddress = childAddress;
        }

        public void run()
        {
            try
            {
                lcm.reportChildState(childAddress);
            }
            catch (EndpointException e)
            {
                
                log.warn("Exception caught by processMessage(). Message " +
                    EdgeLCMMessageFactory.EDGE_REQUEST_CHILD_STATE, e);
            }
        }

    }

}