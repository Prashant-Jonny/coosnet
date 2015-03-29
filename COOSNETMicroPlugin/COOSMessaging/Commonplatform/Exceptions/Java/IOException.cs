using System;
using Microsoft.SPOT;

// Skip this, since .NET has similar IOException
namespace java.io
{
    public class IOException : Exception
    {
        public IOException() : base()
        {
            
        }

        public IOException(string msg)
            : base(msg)
        {
        }
    }
}
