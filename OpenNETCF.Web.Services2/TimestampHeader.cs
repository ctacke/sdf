
using System; 
using System.Xml;
using System.Xml.Serialization; 
using System.Web.Services.Protocols; 

//declare member and in/out header attribute in proxy too 
//public bNb.TimestampHeader timestampHeader; 
//[SoapHeaderAttribute("timestampHeader",Direction=SoapHeaderDirection.InOut)] 

/*
<wsu:Timestamp xmlns:wsu="http://schemas.xmlsoap.org/ws/2002/07/utility">
  <wsu:Created>2003-09-15T18:00:24Z</wsu:Created>
  <wsu:Expires>2003-09-15T18:05:24Z</wsu:Expires>
</wsu:Timestamp>

<wsu:Timestamp xmlns:wsu="http://schemas.xmlsoap.org/ws/2002/07/utility"> 
  <wsu:Created>2002-12-22T04:15:29Z</wsu:Created> 
  <wsu:Expires>2002-12-22T04:20:29Z</wsu:Expires> 
  <wsu:Received Actor="http://notebook/WSEQuickStart/router/sumservice.asmx" Delay="807"> 
    2002-12-22T04:15:29Z 
  </wsu:Received> 
</wsu:Timestamp> 
*/

namespace OpenNETCF.Web.Services2
{
	[XmlRoot(Namespace=Ns.wsu, ElementName="Timestamp")] 
	public class TimestampHeader : SoapHeader 
	{ 
		public const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";

		public TimestampHeader() {}

		//seconds
		public TimestampHeader(long secondsToLive) : this(DateTime.UtcNow, secondsToLive)
		{
 
		}

		//UTC, seconds
		public TimestampHeader(DateTime created, long secondsToLive) 
		{ 
			this.Created = new ExpiresCreated();
			this.Created.text = XmlConvert.ToString(created, dateTimeFormat);
			
			this.Expires = new ExpiresCreated();
			DateTime dtExpires = created.AddSeconds(secondsToLive);
			this.Expires.text = XmlConvert.ToString(dtExpires, dateTimeFormat); 
		}

		public static bool IsExpired(string strExpires)
		{
			DateTime now = DateTime.UtcNow;
			string nowStr = now.ToString(dateTimeFormat);
			
			//if(dtExpires < now)
			//if(dtExpires.CompareTo(now) == 1)
			if(strExpires.CompareTo(nowStr) == -1) //less than
				return true; //expired
			else
				return false; //not expired
		}

		public bool IsExpired()
		{
			bool retVal = TimestampHeader.IsExpired(Expires.text);
			return retVal;
		}

		public static DateTime ConvertDateTime(string utcTime)
		{
			DateTime dt = XmlConvert.ToDateTime(utcTime, dateTimeFormat);
			//DateTime dtExpires = Convert.ToDateTime(utcTime);
			return dt;
		}

		public static string ConvertDateTime(DateTime utcTime)
		{
			return utcTime.ToString(dateTimeFormat);
		}

		//optional
		[XmlAttribute(Namespace=Ns.wsu)]
		public string Id;

		//optional
		[XmlElement(Namespace=Ns.wsu)] 
		public ExpiresCreated Created; 
		//optional
		[XmlElement(Namespace=Ns.wsu)] 
		public ExpiresCreated Expires; 
		//optional
		[XmlElement(Namespace=Ns.wsu)] 
		public Received Received;

		//any
		//[XmlAnyElement]
		//public XmlElement [] anyElements;
		//[XmlAnyAttribute]
		//public XmlAttribute [] anyAttributes;
	}
 
	public class ExpiresCreated
	{ 
		public ExpiresCreated() {}

		//optional
		[XmlAttribute()]
		public string ValueType;
		//optional
		[XmlAttribute(Namespace=Ns.wsu)]
		public string Id;

		[XmlText()]
		public string text;

		/*
		private DateTime _dateTime;
		[XmlText()]
		public string text
		{
			get
			{
				return TimestampHeader.ConvertDateTime(_dateTime);
			}
			set
			{
				//strong typing
				_dateTime = TimestampHeader.ConvertDateTime(value);
				//validate
				bool retVal = TimestampHeader.IsExpired(value);
				if(retVal == true)
					throw new Exception("WS-Utility Timestamp is expired");
			}
		}
		*/
	}

	public class Received : ExpiresCreated
	{ 
		public Received(){} 
  
		//required
		[XmlAttribute()] 
		public string Actor; 
		//in milliseconds
		[XmlAttribute()] 
		public long Delay;

		//inherits the rest from ExpiresCreated
	} 
}