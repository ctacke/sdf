using System;
using System.IO;
using System.Globalization;
using System.Security;
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// The exception that is thrown when an error occurs in a configuration setting.
	/// </summary>
	public class ConfigurationException : SystemException
	{
		private string _filename;
		private int _line;

		/// <summary>
		/// Initializes a new instance of the System.Configuration.ConfigurationException class.
		/// </summary>
		public ConfigurationException() : base() 
		{
		}

		/// <summary>
		/// Initializes a new instance of the System.Configuration.ConfigurationException class with the specified error message. 
		/// </summary>
		/// <param name="message">The message to display to the client when the exception is thrown.</param>
        public ConfigurationException(string message) :  base(message) 
		{  
        }

		/// <summary>
		/// Initializes a new instance of the System.Configuration.ConfigurationException class with the specified error message and System.Exception.InnerException  property.
		/// </summary>
		/// <param name="message">The message to display to the client when the exception is thrown.</param>
		/// <param name="inner">The System.Exception.InnerException , if any, that threw the current exception.</param>
		public ConfigurationException(String message, Exception inner) : base(message, inner) 
		{
		}

		/// <summary>
		/// Initializes a new instance of the System.Configuration.ConfigurationException class with the specified error message and System.Exception.InnerException and the name of the configuration section node that contains the error.  
		/// </summary>
		/// <param name="message">The message to display to the client when the exception is thrown.</param>
		/// <param name="inner">The System.Exception.InnerException , if any, that threw the current exception.</param>
		/// <param name="node">The System.Xml.XmlNode that contains the error.</param>
		public ConfigurationException(string message, Exception inner, XmlNode node) : this(message, inner, GetXmlNodeFilename(node), GetXmlNodeLineNumber(node))
		{
		}

		/// <summary>
		/// Initializes a new instance of the System.Configuration.ConfigurationException class with the specified error message, the name of the configuration file that contains the error, and the line number in the file.
		/// </summary>
		/// <param name="message">The message to display to the client when the exception is thrown.</param>
		/// <param name="filename">The name of the configuration file that contains the error.</param>
		/// <param name="line">The number of the line that contains the error.</param>
		public ConfigurationException(string message, string filename, int line) : base(message)
		{
			_filename = filename;
			_line = line;
		}

		/// <summary>
		/// Initializes a new instance of the System.Configuration.ConfigurationException with the specified error message and System.Exception.InnerException, the name of the file containing the error, and the line number of the error in the file. 
		/// </summary>
		/// <param name="message">The message to display to the client when the exception is thrown.</param>
		/// <param name="inner">The System.Exception.InnerException , if any, that threw the current exception.</param>
		/// <param name="filename">The name of the configuration file that contains the error.</param>
		/// <param name="line">The number of the line that contains the error.</param>
		public ConfigurationException(string message, Exception inner, string filename, int line) : base(message, inner)
		{
			_filename = filename;
			_line = line;
		}

		/// <summary>
		/// Initializes a new instance of the System.Configuration.ConfigurationException with the specified error message and the name of the configuration section containing the error.  
		/// </summary>
		/// <param name="message">The message to display to the client when the exception is thrown.</param>
		/// <param name="node">The System.Xml.XmlNode that contains the error.</param>
		public ConfigurationException(string message, XmlNode node) : this(message, GetXmlNodeFilename(node), GetXmlNodeLineNumber(node))
		{
		}

		/// <summary>
		/// Returns the line number of the configuration section node that contains the error.
		/// </summary>
		/// <param name="node">The name of the configuration section node that contains the error.</param>
		/// <returns>The line number that contains the error.</returns>
		public static int GetXmlNodeLineNumber(XmlNode node)
		{
			IConfigXmlNode configNode = node as IConfigXmlNode;
			if (configNode != null)
			{
				return configNode.LineNumber;
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		///  Returns the name of the file that contains the configuration section node that contains the error.
		/// </summary>
		/// <param name="node">The name of the configuration section node that contains the error.</param>
		/// <returns>The name of the configuration file.</returns>
		public static string GetXmlNodeFilename(XmlNode node)
		{
			IConfigXmlNode configNode = node as IConfigXmlNode;

			if (configNode != null)
			{
				return configNode.Filename;
			}
			else
			{
				return string.Empty;
			}
		}
	}
}
