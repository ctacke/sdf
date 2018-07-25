
using System;
using System.Web.Services.Protocols;

namespace OpenNETCF.Web.Services2
{
	public class UnknownHeaderWrap : SoapHttpClientProtocol
	{
		public UnknownHeaderWrap() : base()
		{}

		//declare member and attribute in auto gen'd proxy 
		//public SoapUnknownHeader [] unknownHeaders; 
		//[SoapHeaderAttribute("unknownHeaders",Direction=SoapHeaderDirection.Out)] 
  
		/*
		//and in client code with 'webRef' being the proxy 
		SoapUnknownHeader [] suha = webRef.unknownHeaders; 
		foreach(SoapUnknownHeader suh in suha) 
		{ 
			MessageBox.Show(suh.Element.Name); 
		} 
		*/
	}
}
