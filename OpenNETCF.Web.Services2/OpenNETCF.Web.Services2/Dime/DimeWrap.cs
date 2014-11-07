
using System;
using System.Web.Services.Protocols;
using OpenNETCF.Web.Services2.Dime;

namespace OpenNETCF.Web.Services2.Dime
{
	public class DimeWrap : SoapHttpClientProtocol, IDimeAttachmentContainer
	{
		public DimeWrap() : base()
		{}

		DimeAttachmentCollection requestAttachments;
		DimeAttachmentCollection responseAttachments;

		// IDimeAttachmentContainer.RequestAttachments
		public DimeAttachmentCollection RequestAttachments 
		{ 
			get 
			{
				if (requestAttachments == null) 
					requestAttachments = new DimeAttachmentCollection();
				return requestAttachments;
			}
		}

		// IDimeAttachmentContainer.ResponseAttachments
		public DimeAttachmentCollection ResponseAttachments 
		{ 
			get 
			{
				if (responseAttachments == null) 
					responseAttachments = new DimeAttachmentCollection();
				return responseAttachments;
			}
		}
	}
}
