using System;
using System.Threading;
using Microsoft.SPOT; // EventArgs



namespace Hks.Itprojects.GPS
{

    // A delegate type for hooking up change notifications.
    public delegate void PositionEventHandler(object sender, PositionArgs p);
    
    public class GPSSensor : IGPSSensor
    {
        /// <summary>
        /// Time for device to start and connect to satellites in milliseconds.
        /// </summary>
        readonly int startupTime = 1000;

        /// <summary>
        /// Time for device to shutdown (ms)
        /// </summary>
        readonly int stopTime = 50;

        /// <summary>
        /// Start reading of position at regular intervals
        /// </summary>
        Timer timer;

        // An event that clients can use to be notified whenever the
        // a new position is read from GPS.
        public event PositionEventHandler NewPosition;

        // Invoke the NewPosition event; called whenever new position is read
        protected virtual void OnNewPosition(PositionArgs pArgs)
        {
            if (NewPosition != null)
                NewPosition(this, pArgs);
        }

        public GPSSensor(int dueTime, int period)
        {
            timer = new Timer(readTimerPosition,null, dueTime, period);
        }

        public GPSSensor()
        {
        }

        /// <summary>
        /// Called by timer at regular intervals, gives notification to subscribers of NewPosition event
        /// </summary>
        /// <param name="state"></param>
        public void readTimerPosition(object state)
        {
            startDeviceAndSeekSignal();
            Position p = readPosition();
            PositionArgs pArgs = new PositionArgs(p);
            OnNewPosition(pArgs); // Signal new position
            stopDevice();
        }

        public virtual Position readPosition()
        {
            //Random r = new Random();
            
             Position p = new Position(69.19, 18.7, DateTime.Now);
            return p;
        }

        /// <summary>
        /// Simulate start on calling thread
        /// </summary>
        public void startDeviceAndSeekSignal()
        {
            // Start GPS device, seek signal
            Thread.Sleep(startupTime);
            
        }

        /// <summary>
        /// Simulate stop on calling thread
        /// </summary>
        public void stopDevice()
        {
            Thread.Sleep(stopTime);
        }

        public void stopTimer()
        {
            if (timer != null)
                timer.Dispose();
        }

    }
}