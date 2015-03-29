using System;

namespace Hks.Itprojects.GPS
{
    public class Position
    {
        private double latitude;
        private double longitude;
        private DateTime timeStamp;

        #region Properties
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        #endregion

        public Position(double latitude, double longitude, DateTime timeStamp)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.timeStamp = timeStamp;
        }
    }
}