
using System;
using System.Web.Services.Protocols;

namespace OpenNETCF.Web.Services2
{
	//previous to this override it was done with sessionless cookies in the URL
	//or using the HttpWebRequest directly and building your own soap messages		
	//ultimately it should be done in a SoapHeader, as demonstated in KeithBa's WS book
	public class SessionCookiesWrap : SoapHttpClientProtocol
	{
		public SessionCookiesWrap() : base()
		{}

		//this will work on RTM
		//http://www.alexfeinman.com/download.asp?doc=SessionAwareWebSvc.zip

		/*
		private static string sessCookie = null; //TODO expired sessions
		
		//only with SP1 bits
		protected override System.Net.WebRequest GetWebRequest(Uri uri)
		{
			System.Net.HttpWebRequest hwr = (System.Net.HttpWebRequest) base.GetWebRequest (uri);
			if(sessCookie != null)
			{
				hwr.Headers.Add("Cookie", sessCookie);
			}
			return hwr;
		}
	
		protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
		{
			System.Net.HttpWebResponse hwr = (System.Net.HttpWebResponse) base.GetWebResponse (request);
			if(hwr.Headers["Set-Cookie"] != null)
			{
				sessCookie = hwr.Headers["Set-Cookie"];
				//"ASP.NET_SessionId=g3exyaihxpmbkr55rhfxwq45; path=/"
			}
			return hwr;
		}
		*/
	}
}
