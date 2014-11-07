
using System;
using System.Net;

namespace OpenNETCF.Net.Ftp
{
	public class FtpRequestCreator : IWebRequestCreate
	{
		public FtpRequestCreator()
		{
		}

		public WebRequest Create( Uri Url )
		{
			return new FtpWebRequest( Url );
		}
	}
}
