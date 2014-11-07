
using System; 
using System.Xml.Serialization; 
using System.Web.Services.Protocols; 

//add public member to the auto gen'd proxy 
//public bNb.pathHeader pathHeaderVal; 
//add to WebMethod that will be called on auto gen'd proxy 
//[SoapHeaderAttribute("pathHeaderVal",Direction=SoapHeaderDirection.InOut)] 

//INTRO
//http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnglobspec/html/wsroutspecindex.asp
//SPEC
//http://msdn.microsoft.com/ws/2001/10/Routing/
//SCHEMA
//http://schemas.xmlsoap.org/rp/


/*
<wsrp:path soap:actor="http://schemas.xmlsoap.org/soap/actor/next" soap:mustUnderstand="1" xmlns:wsrp="http://schemas.xmlsoap.org/rp">
  <wsrp:action>http://www.microsoft.com/GetTopDownloads</wsrp:action>
  <wsrp:to>http://ws.microsoft.com/mscomservice/mscom.asmx</wsrp:to>
  <wsrp:id>uuid:11e62a57-ba3b-4a50-a3e6-c684f57349b4</wsrp:id>
  <wsrp:relatesTo>uuid:84b9f5d0-33fb-4a81-b02b-5b760641c1d6</wsrp:relatesTo>
</wsrp:path>

<wsrp:path S:actor="http://schemas.xmlsoap.org/soap/actor/next" S:mustUnderstand="1" xmlns:wsrp="http://schemas.xmlsoap.org/rp">
  <wsrp:action>http://www.contoso.com/chat</wsrp:action>
  <wsrp:to>http://D.example.com/some/endpoint</wsrp:to>
  <wsrp:fwd>
    <wsrp:via>http://B.example.com</wsrp:via>
    <wsrp:via>http://C.example.com</wsrp:via>
	<wsrp:via wsrp:vid="cid:122326@B.com"/>
  </wsrp:fwd>
  <wsrp:from>mailto:someone@example.com</wsrp:from>
  <wsrp:id>uuid:84b9f5d0-33fb-4a81-b02b-5b760641c1d6</wsrp:id>
</wsrp:path>

<wsrp:path soap:actor=http://schemas.xmlsoap.org/soap/actor/next   
        soap:mustUnderstand="1" xmlns:wsrp="http://schemas.xmlsoap.org/rp"> 
  <wsrp:action>http://schemas.xmlsoap.org/soap/fault</wsrp:action> 
  <wsrp:id>uuid:ef4218a8-3765-4efa-bb73-0f5555aee4b5</wsrp:id> 
  <wsrp:fault> 
    <wsrp:code>712</wsrp:code> 
    <wsrp:reason>Endpoint Not Supported</wsrp:reason> 
    <endpoint>http://notebook/WSEQuickStart/router/sumservice.asmx</endpoint> 
  </wsrp:fault> 
</wsrp:path>
*/

namespace OpenNETCF.Web.Services2
{
	/// <summary>
	/// WS-Routing is a simple, stateless, SOAP-based protocol for routing SOAP messages 
	/// over a variety of transports like TCP, UDP, and HTTP. With WS-Routing, the entire 
	/// message path for a SOAP message (as well as its return path) can be described 
	/// directly within the SOAP envelope. It supports one-way messaging, 
	/// two-way messaging such as request/response and peer-to-peer conversations, 
	/// and long running dialogs.
	/// </summary>
	/// <remarks>just keeping this around for WSE 1.0.
	/// WSE 2.0 uses WS-Addressing</remarks>
	[XmlRoot(Namespace=Ns.m, ElementName="path")] 
	public class pathHeader : SoapHeader 
	{ 
		public pathHeader() {}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="action">Element that indicates the intent of the message in a manner 
		/// akin to the SOAPAction HTTP header field defined for SOAP (required).</param>
		/// <param name="to">The ultimate receiver (required).</param>
		/// <param name="relatesTo">Element that indicates a relation with another message (optional).</param>
		public pathHeader(string action, string to, string relatesTo)
		{
			//soap namespace attributes
			this.MustUnderstand = true; 
			this.Actor = Misc.pathActorNext; 
			
			//required
			//'id' - Element that uniquely identifies a message (required).
			//id = "uuid:bd360a63-4d38-461f-97b1-2f5f80ba766e"; 
			this.id = "uuid:" + Guid.NewGuid().ToString("D"); 
			//this.action = "http://microsoft.com/wse/samples/SumService/AddInt"; 
			//this.to = "http://notebook/WSEQuickStart/router/sumservice.asmx"; 
			this.action = action; 
			this.to = to; 			
		
			//optional
			if(relatesTo!=null && relatesTo!=String.Empty)
			{
				this.relatesTo = relatesTo;
			}
		}
  

		//required
		[XmlElement(Namespace=Ns.m)] 
		public string to; 
		[XmlElement(Namespace=Ns.m)] 
		public string id; 
		[XmlElement(Namespace=Ns.m)] 
		public string action; 

		//optional
		[XmlElement(Namespace=Ns.m)] 
		public string from; //The message originator (optional).
		[XmlElement(Namespace=Ns.m)] 
		public fwd fwd; //The forward message path. (optional)
		[XmlElement(Namespace=Ns.m)] 
		public rev rev; //The reverse message path (optional).

		[XmlElement(Namespace=Ns.m)] 
		public string relatesTo; 
		[XmlElement(Namespace=Ns.m)] 
		public fault fault; 
	} 
	
	public class fwd
	{
		public fwd() {}

		//TODO is this array setup correct?
		//[XmlArray(IsNullable=false)]
		[XmlElement(Namespace=Ns.m)]
		public via []  via;
	}

	public class rev
	{
		public rev() {}

		//TODO is this array setup correct?
		//[XmlArray(IsNullable=false)]
		[XmlElement(Namespace=Ns.m)]
		public via []  via;
	}

	//An intermediary. (optional)
	public class via
	{
		public via() {}

		[XmlAttribute(Namespace=Ns.m)]
		public string vid;
		[XmlText()]
		public string text;
	}

	//Contains the fault information. (required)
	public class fault
	{
		public fault() {}

		//required
		[XmlElement(Namespace=Ns.m)] 
		public long code; //Identifies the specific WS-Routing fault by its code number. (required)
		[XmlElement(Namespace=Ns.m)] 
		public string reason; //Provides a phrase explaining the cause of the fault. (required)
		//sometimes required :)
		[XmlElement(Namespace=Ns.m)] 
		public string endpoint; 
		//optional
		[XmlElement(Namespace=Ns.m)] 
		public long retryAfter; //given in seconds
	}

	public enum RoutingFaults
	{
		//WS-Routing Sender Faults
		[XmlEnum("Invalid WS-Routing Header")]
		InvalidWsRoutingHeader = 700,
		[XmlEnum("WS-Routing Header Required")]
		WsRoutingHeaderRequired = 701,
		[XmlEnum("Endpoint Not Found")]
		EndpointNotFound = 710,
		[XmlEnum("Endpoint Gone")]
		EndpointGone = 711,
		[XmlEnum("Endpoint Not Supported")]
		EndpointNotSupported = 712,
		[XmlEnum("Endpoint Invalid")]
		EndpointInvalid = 713,
		[XmlEnum("Alternative Endpoint Found")]	
		AlternativeEndpointFound = 720,
		[XmlEnum("Endpoint Too Long")]
		EndpointTooLong = 730,
		[XmlEnum("Message Too Large")]
		MessageTooLarge = 731,
		[XmlEnum("Message Timeout")]
		MessageTimeout = 740, 
		[XmlEnum("Message Loop Detected")]
		MessageLoopDetected = 750,
		[XmlEnum("Reverse Path Unavailable")]
		ReversePathUnavailable = 751,
		
		//WS-Routing Receiver Faults
		[XmlEnum("Unknown WS-Routing Fault")]
		UnknownWsRoutingFault = 800,
		[XmlEnum("Element Not Implemented")]
		ElementNotImplemented = 810,
		[XmlEnum("Service Unavailable")]
		ServiceUnavailable = 811,
		[XmlEnum("Service Too Busy")]
		ServiceTooBusy = 812,
		[XmlEnum("Endpoint Not Reachable")]
		EndpointNotReachable = 820,
	}
}
