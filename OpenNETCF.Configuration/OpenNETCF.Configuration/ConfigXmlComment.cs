using System;
using System.Xml;

namespace OpenNETCF.Configuration
{
	sealed class ConfigXmlComment : XmlComment, IConfigXmlNode
	{
		private int _line;
		private string _filename;

		public ConfigXmlComment(string filename, int line, string comment, XmlDocument doc) : base(comment, doc)
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
			ConfigXmlComment configXmlComment = xmlNode as ConfigXmlComment;
			if (configXmlComment != null)
			{
				configXmlComment._line = _line;
				configXmlComment._filename = _filename;
			}
			return xmlNode;
		}
	}
}
