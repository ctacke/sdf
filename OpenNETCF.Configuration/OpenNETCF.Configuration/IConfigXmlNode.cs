
using System;

namespace OpenNETCF.Configuration
{
	interface IConfigXmlNode
	{
		string Filename { get; }
		int LineNumber { get; }
	}
}
