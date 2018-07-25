
using System;
using System.Web.Services.Protocols;

namespace OpenNETCF.Web.Services2.Dime {
    /// <summary>
    /// Attribute to be used on the web method on the server and client proxy
    /// to enable the DimeExtension.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DimeExtensionAttribute : SoapExtensionAttribute 
	{
        public override Type ExtensionType 
		{
            get { return typeof(DimeExtension); }
        }
        public override int Priority 
		{
            get { return 1; }
            set {  }
        }
		private DimeDir dd;
		public DimeDir DimeDirection
		{
			get{return dd;}
			set{dd = value;}
		}
    }

	public enum DimeDir
	{
		Request,
		Response,
		Both,
	}
}
