//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Org.Coos.Messaging
{
    
    internal partial class Resource1
    {
        private static System.Resources.ResourceManager manager;
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if ((Resource1.manager == null))
                {
                    Resource1.manager = new System.Resources.ResourceManager("Org.Coos.Messaging.Resource1", typeof(Resource1).Assembly);
                }
                return Resource1.manager;
            }
        }
        internal static byte[] GetBytes(Resource1.BinaryResources id)
        {
            return ((byte[])(Microsoft.SPOT.ResourceUtility.GetObject(ResourceManager, id)));
        }
        [System.SerializableAttribute()]
        internal enum BinaryResources : short
        {
            COOSFakeCA = 23228,
        }
    }
}