using System;
using Microsoft.SPOT;

namespace Hks.Itprojects.GPS
{
    public class PositionArgs : EventArgs
    {
        private Position position;

        public Position Position
        {
            get { return position; }
        }

        public PositionArgs() : base() { }

        public PositionArgs(Position p)
        {
            position = p;
        }


    }
}
