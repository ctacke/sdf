using System;
using System.IO;
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class ConfigXmlDocument : XmlDocument, IConfigXmlNode
	{
		private XmlTextReader _reader;
		private int _lineOffset;
		private string _filename;

		/// <summary>
		/// 
		/// </summary>
		public string Filename
		{
			get	{ return _filename; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int LineNumber
		{
				get 
		  {
			  if (_reader == null)
			  {
				  return 0;
			  }
			  if (_lineOffset > 0)
			  {
				  return _reader.LineNumber + _lineOffset - 1;
			  }
			  else
			  {
				  return _reader.LineNumber;
			  }
		  }
		}

		string IConfigXmlNode.Filename
		{
			get { return _filename; }			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		public override void Load(string filename)
		{
			_filename = filename;
			try
			{
				_reader = new XmlTextReader(filename);
				base.Load(_reader);
			}
			finally
			{
				if (_reader != null)
				{
					_reader.Close();
					_reader = null;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="sourceReader"></param>
		/// <returns></returns>
		internal XmlNode ReadConfigNode(string filename, XmlTextReader sourceReader)
		{
			XmlNode xmlNode;

			_filename = filename;
			_reader = sourceReader;
			try
			{
				xmlNode = base.ReadNode(sourceReader);
			}
			finally
			{
				_reader = null;
			}
			return xmlNode;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="sourceReader"></param>
		public void LoadSingleElement(string filename, XmlTextReader sourceReader)
		{
			_filename = filename;
			_lineOffset = sourceReader.LineNumber;
			string str = sourceReader.ReadOuterXml();
			try
			{
				_reader = new XmlTextReader(new StringReader(str), sourceReader.NameTable);
				base.Load(_reader);
			}
			finally
			{
				if (_reader != null)
				{
					_reader.Close();
					_reader = null;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="localName"></param>
		/// <param name="namespaceUri"></param>
		/// <returns></returns>
		public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceUri)
		{
			return new ConfigXmlAttribute(_filename, LineNumber, prefix, localName, namespaceUri, this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="localName"></param>
		/// <param name="namespaceUri"></param>
		/// <returns></returns>
		public override XmlElement CreateElement(string prefix, string localName, string namespaceUri)
		{
			return new ConfigXmlElement(_filename, LineNumber, prefix, localName, namespaceUri, this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public override XmlText CreateTextNode(string text)
		{
			return new ConfigXmlText(_filename, LineNumber, text, this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public override XmlCDataSection CreateCDataSection(string data)
		{
			return new ConfigXmlCDataSection(_filename, LineNumber, data, this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public override XmlComment CreateComment(string data)
		{
			return new ConfigXmlComment(_filename, LineNumber, data, this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public override XmlSignificantWhitespace CreateSignificantWhitespace(string data)
		{
			return new ConfigXmlSignificantWhitespace(_filename, LineNumber, data, this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public override XmlWhitespace CreateWhitespace(string data)
		{
			return new ConfigXmlWhitespace(_filename, LineNumber, data, this);
		}
	}
}
