using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Phone.Sms
{
	/// <summary>
	/// Represents an individual SMS message.
	/// </summary>
	/// <preliminary/>
	public class SmsMessage
	{		
		public SmsMessage()
		{
			
		}

		public SmsAddress PhoneNumber
		{
			get
			{
				SmsAddress buffer = new SmsAddress();

				int result = SmsGetPhoneNumber(buffer);

				if(result!=0)
				{
					throw new ExternalException("Error Retrieving Phone Number");
				}
				return buffer;
			}
		}

		[DllImport("sms.dll")]
		private static extern int SmsGetPhoneNumber(byte[] psmsaAddress);

		
	}
}
