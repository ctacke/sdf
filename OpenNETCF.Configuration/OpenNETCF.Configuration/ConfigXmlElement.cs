using System;
using System.Xml;

namespace OpenNETCF.Configuration
{
	sealed class ConfigXmlElement : XmlElement, IConfigXmlNode
	{
		private int _line;
		private string _filename;

		public ConfigXmlElement(string filename, int line, string prefix, string localName, string namespaceUri, XmlDocument doc) : base(prefix, localName, namespaceUri, doc)
		{
			_line = line;
			_filename = filename;
		}

		int IConfigXmlNode.LineNumber
		{
			get { return _line; }
		}

		string IConfigXmlNode.Filename
		{
			get { return _filename; }
		}

		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlElement configXmlElement = xmlNode as ConfigXmlElement;
			if (configXmlElement != null)
			{
				configXmlElement._line = _line;
				configXmlElement._filename = _filename;
			}
			return xmlNode;
		}
	}
}
