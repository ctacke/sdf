using System;
using System.Xml;

namespace OpenNETCF.Configuration
{
	sealed class ConfigXmlAttribute : XmlAttribute, IConfigXmlNode
	{
		private int _line;
		private string _filename;

		public ConfigXmlAttribute(string filename, int line, string prefix, string localName, string namespaceUri, XmlDocument doc) : base(prefix, localName, namespaceUri, doc)
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
			XmlNode cloneNode = base.CloneNode(deep);
			ConfigXmlAttribute clone = cloneNode as ConfigXmlAttribute;
			if (clone != null)
			{
				clone._line = _line;
				clone._filename = _filename;
			}
			return clone;
		}
	}
}
