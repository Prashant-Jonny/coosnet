﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COOS_Router {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ResourceCOOS {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourceCOOS() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("COOS_Router.ResourceCOOS", typeof(ResourceCOOS).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        ///
        ///&lt;coos xmlns=&quot;http://www.coos.org/CoosXMLSchema&quot; name=&quot;coosNET&quot;&gt;
        ///
        ///  &lt;router class=&quot;Org.Coos.Messaging.Routing.DefaultRouter&quot;&gt;
        ///
        ///    &lt;segment name=&quot;.&quot; routeruuid=&quot;RouterNET&quot; routeralgorithm=&quot;simpledv&quot; /&gt;
        ///
        ///    &lt;!--The simpledv algorithm is specifically targeted towards the dispatcher setup in telenor objects --&gt;
        ///    &lt;routeralgorithm name=&quot;simpledv&quot;  class=&quot;Org.Coos.Messaging.Routing.SimpleDVAlgorithm&quot;&gt;
        ///      &lt;property name=&quot;refreshInterval&quot; value=&quot;5000&quot; /&gt;
        ///     [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string coos {
            get {
                return ResourceManager.GetString("coos", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        ///&lt;plugins xmlns=&quot;http://www.coos.org/PluginXMLSchema&quot;&gt;
        ///  &lt;plugin name=&quot;thePong&quot; startLevel=&quot;10&quot; class=&quot;org.coos.messaging.plugin.actorframe.ActorFrameContainer&quot; channel=&quot;ch1&quot;&gt;
        ///    &lt;property name=&quot;lcmRegRequired&quot; value=&quot;false&quot; /&gt;														
        ///    &lt;property name=&quot;spec&quot; value=&quot;/ApplicationSpec.xml&quot; /&gt;
        ///    &lt;property name=&quot;defaultReceiverUri&quot; value=&quot;coos://thePong/pongActor@Pong&quot; /&gt;
        ///    &lt;!--    &lt;channel&gt;chjms&lt;/channel&gt;  --&gt;
        ///    &lt;/plugin&gt;
        ///
        ///  &lt;channel name=&quot;ch1&quot; prot [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string pluginPong {
            get {
                return ResourceManager.GetString("pluginPong", resourceCulture);
            }
        }
    }
}