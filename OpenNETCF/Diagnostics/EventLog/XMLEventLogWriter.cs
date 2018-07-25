using System;
using System.Xml;
using System.IO;
using System.Collections;
using OpenNETCF;

namespace OpenNETCF.Diagnostics
{
	/// <summary>
	/// Implements writing to the event log using XML
	/// </summary>
	public class XMLEventLogWriter:IEventLogWriter
	{

		public event EntryWrittenEventHandler EntryWritten;
		public event EventHandler EventLogCollectionUpdated;

		private string source="";
		private string log="";
		private string logFileName="";		
		private string logPath="";
		private string logDisplayName="";
		private EventLogEntryCollection eventLogEntryCollection = new EventLogEntryCollection();
		private ArrayList eventLogCollection = null;

		/// <summary>
		/// The Xml document representing the log
		/// </summary>
		private XmlDocument xmlLog=null;
		/// <summary>
		/// The current eventlog node which contains all Log nodes
		/// </summary>
		private XmlNode nodeEventLog = null;
		/// <summary>
		/// The current log node that is being written to
		/// </summary>
		private XmlNode nodeLog = null;
		/// <summary>
		/// The default Xml for a new log
		/// </summary>
		private readonly string EVENTLOG_ROOT = "<eventLog></eventLog>";

		internal XMLEventLogWriter(string log, string source, string logPath, string logFileName) 
		{
			this.log = log;
			this.source = source;
			this.logPath = logPath;
			this.logFileName = logFileName;
			this.InitializeLogFile();
		}
        		
		#region Private Methods
		/// <summary>
		/// Creates the XML document to store the event logs
		/// </summary>
		private void InitializeLogFile()
		{
			//Create the log file
			this.CreateLogFile();

			//Load the nodes
			this.LoadNodes();

			//Save the file
			this.SaveLogFile();

			//Load the eventLogEntryColelction
			this.LoadEventLogEntryCollection();

		}

		/// <summary>
		/// Loads the eventlog collection
		/// </summary>
		private void LoadEventLogCollection()
		{
			XmlNodeList list = this.xmlLog.GetElementsByTagName("log");
			this.eventLogCollection = new ArrayList();
			foreach(XmlNode node in list)
			{
				string log = this.NodeAttributeValue(node,"name");
				if(log.Length>0)
				{
					eventLogCollection.Add(new EventLog(log,"",this.logPath,this.logFileName, EventLogWriterType.XML));
				}
			}
		}

		/// <summary>
		/// Creates a log file on the system
		/// </summary>
		private void CreateLogFile()
		{
			//Create a new log file
			this.xmlLog = new XmlDocument();

			//Check to see if the file exists
			if(File.Exists(this.logPath+this.logFileName))
			{
				//The file is available so load it into the XML
				try
				{
					this.xmlLog.Load("file://"+this.logPath+this.logFileName);
				}
				catch(Exception e)
				{
					throw new Exception("Unable to open log file as XML!!",e);
				}
			}
			else
			{
				//The file does not exist so load the default XML
				this.xmlLog.LoadXml(this.EVENTLOG_ROOT);
			}
		}

		/// <summary>
		/// Loads the source, log and EventLog nodes
		/// </summary>
		private void LoadNodes()
		{
			//Get the event Log node
			this.nodeEventLog = this.xmlLog.GetElementsByTagName("eventLog")[0];

			//Get the current log node
			this.nodeLog = this.EventLogNode(this.log);
			if(this.nodeLog==null)
			{
				//Create the log node
				this.nodeLog = this.CreateLogNode(this.log);			
			}

			//Set the log display name
			this.logDisplayName = this.NodeAttributeValue(nodeLog,"logDisplayName");
		}
		

		/// <summary>
		/// Loads the eventlog entry collection
		/// </summary>
		private void LoadEventLogEntryCollection()
		{
			//This is only called when the log is changed and when the class is instantiated so it's pretty safe
			//to re-init the collection
			this.eventLogEntryCollection = new EventLogEntryCollection();
			string[] attributes = new string[]{"machineName","userName","timeGenerated",
												  "source","message","eventLogEntryType",
												  "eventID","category","rawData","id","timeWritten","index"};

			//Get all the eventLog nodes
			foreach(XmlNode node in this.nodeLog.ChildNodes)
			{
				string machineName = node.Attributes.GetNamedItem(attributes[0]).Value;
				string userName = node.Attributes.GetNamedItem(attributes[1]).Value;
				DateTime timeGenerated = DateTime.Parse(node.Attributes.GetNamedItem(attributes[2]).Value);
				string source = node.Attributes.GetNamedItem(attributes[3]).Value;
				string message = node.Attributes.GetNamedItem(attributes[4]).Value;
				EventLogEntryType type = (EventLogEntryType)Int32.Parse(node.Attributes.GetNamedItem(attributes[5]).Value);
				int eventID = Int32.Parse(node.Attributes.GetNamedItem(attributes[6]).Value);
				short category = Int16.Parse(node.Attributes.GetNamedItem(attributes[7]).Value);
				string srawData = node.Attributes.GetNamedItem(attributes[8]).Value;
				byte[] rawData = new byte[0];
				if(srawData.Length > 0)
					rawData = Convert.FromBase64String(srawData);
				string id = node.Attributes.GetNamedItem(attributes[9]).Value;
				DateTime timeWritten = DateTime.Parse(node.Attributes.GetNamedItem(attributes[10]).Value);
				int index = Int32.Parse(node.Attributes.GetNamedItem(attributes[11]).Value);
			
				//Add to the collection
				EventLogEntry eventLogEntry = new EventLogEntry(category,rawData,type,eventID,index,machineName,message,source,timeGenerated,timeWritten,userName,id);
				this.eventLogEntryCollection.Add(eventLogEntry);
			}
		}
		/// <summary>
		/// Create the log node and adds it to the nodeEventLog
		/// </summary>
		private XmlNode CreateLogNode(string logName)
		{
			XmlNode newNode = this.xmlLog.CreateNode(XmlNodeType.Element,"log","");
			XmlAttribute att = this.xmlLog.CreateAttribute("name");
			att.Value = this.log;
			newNode.Attributes.Append(att);
			att = this.xmlLog.CreateAttribute("logDisplayName");
			att.Value = this.logDisplayName;
			newNode.Attributes.Append(att);
			this.nodeEventLog.AppendChild(newNode);
			return newNode;
		}

		/// <summary>
		/// Retrieves and event log node by logName
		/// </summary>
		/// <param name="logName">The name of the log node</param>
		/// <returns>The XmlNode containing the log items or null if not found</returns>
		private XmlNode EventLogNode(string logName)
		{
			XmlNodeList list = this.xmlLog.GetElementsByTagName("log");
			foreach(XmlNode node in list)
			{
				XmlAttribute att = (XmlAttribute)node.Attributes.GetNamedItem("name");
				if(att!=null)
					if(att.Value == logName)
						return node;
			}

			//Return null;
			return null;
		}

		/// <summary>
		/// Changes an attribute in the specified node
		/// </summary>
		private void NodeAttributeValue(XmlNode node, string attributeName, string newAttributeValue)
		{
			XmlAttribute att = (XmlAttribute)node.Attributes.GetNamedItem(attributeName);
			if(att!=null)
			{
				att.Value = newAttributeValue;		
				this.SaveLogFile();
			}	
			else
				throw new Exception("Attribute " + attributeName + " not found in node \"" + node.Name + "\"");
		}

		/// <summary>
		/// Retreives the attribute in the specified node
		/// </summary>
		/// <param name="node"></param>
		/// <param name="attributeName"></param>
		/// <returns>The value of the attribute or null if not found</returns>
		private string NodeAttributeValue(XmlNode node, string attributeName)
		{
			XmlAttribute att = (XmlAttribute)node.Attributes.GetNamedItem(attributeName);
			if(att!=null)
				return att.Value;
			else
				return "";
		}

		/// <summary>
		/// Saves the logfile
		/// </summary>
		private void SaveLogFile()
		{
			this.xmlLog.Save(this.LogPath+this.LogFileName);
		}	
	
		/// <summary>
		/// Writes an entry to the log file
		/// </summary>
		/// <param name="source"></param>
		/// <param name="message"></param>
		/// <param name="type"></param>
		/// <param name="eventID"></param>
		/// <param name="category"></param>
		/// <param name="rawData"></param>
		private void WriteEntryToLog(string source, string message, EventLogEntryType type, int eventID, short category, byte[] rawData)
		{
			if(this.nodeLog!=null)
			{
				//Create the event node
				XmlNode nodeEventLog = this.xmlLog.CreateNode(XmlNodeType.Element,"eventLogEntry","");

				//Now create all the attributes
				DateTime timeGenerated = DateTime.Now;
				string machineName = ".";
				string userName = "";

				//Machine Name
				XmlAttribute att = this.xmlLog.CreateAttribute("machineName");
				att.Value = machineName;
				nodeEventLog.Attributes.Append(att);

				//user name
				att = this.xmlLog.CreateAttribute("userName");
				att.Value = userName;
				nodeEventLog.Attributes.Append(att);

				//DateTime generate
				att = this.xmlLog.CreateAttribute("timeGenerated");
				att.Value = DateTime.Now.ToString();
				nodeEventLog.Attributes.Append(att);

				//source
				att = this.xmlLog.CreateAttribute("source");
				att.Value = source;
				nodeEventLog.Attributes.Append(att);

				//message
				att = this.xmlLog.CreateAttribute("message");
				att.Value = message;
				nodeEventLog.Attributes.Append(att);

				//EventLogEntryType
				att = this.xmlLog.CreateAttribute("eventLogEntryType");
				att.Value = ((int)type).ToString();
				nodeEventLog.Attributes.Append(att);

				//EventID
				att = this.xmlLog.CreateAttribute("eventID");
				att.Value = eventID.ToString();
				nodeEventLog.Attributes.Append(att);

				//categor
				att = this.xmlLog.CreateAttribute("category");
				att.Value = category.ToString();
				nodeEventLog.Attributes.Append(att);

				//rawData
				att = this.xmlLog.CreateAttribute("rawData");
				if(rawData == null)
					rawData = new byte[0];
				att.Value = Convert.ToBase64String(rawData,0,rawData.Length);
				nodeEventLog.Attributes.Append(att);

				//Add a GUID 
				Guid id = Guid.NewGuid();
				att = this.xmlLog.CreateAttribute("id");
				att.Value = id.ToString();
				nodeEventLog.Attributes.Append(att);				

				//add index
				int index = this.eventLogEntryCollection.Count+1;
				att = this.xmlLog.CreateAttribute("index");
				att.Value = index.ToString();
				nodeEventLog.Attributes.Append(att);

				//Time written
				DateTime timeWritten = DateTime.Now;
				att = this.xmlLog.CreateAttribute("timeWritten");
				att.Value = timeWritten.ToString();
				nodeEventLog.Attributes.Append(att);

				//Add the eventLog to the xml
				this.nodeLog.AppendChild(nodeEventLog);

				//Add the eventlog to the collection
				int lastEntry = this.eventLogEntryCollection.Add(new EventLogEntry(category,rawData,type,eventID,
					index,machineName,message,
					source,timeGenerated,timeWritten,userName,id.ToString()));

				//Save the log file
				this.SaveLogFile();

				//Raise the event to listers
				this.OnEntryWritten(this.eventLogEntryCollection[lastEntry]);
			}
		}
		
		private void RemoveEventLog(string logName)
		{
			//Remove from the collection
			if(this.eventLogCollection!=null)
			{
				foreach(EventLog e in this.eventLogCollection)
				{
					if(e.Log == logName)
					{
						this.eventLogCollection.Remove(e);
						this.OnEventLogCollectionUpdated();
						break;
					}
				}
			}
		}
		/// <summary>
		/// Raises the eventLogEntryWritten event to listeners
		/// </summary>
		/// <param name="e"></param>
		private void OnEntryWritten(EventLogEntry e)
		{
			if(this.EntryWritten!=null)
				this.EntryWritten(this,e);
		}

		private void OnEventLogCollectionUpdated()
		{
			if(this.EventLogCollectionUpdated!=null)
				this.EventLogCollectionUpdated(this,EventArgs.Empty);
		}
		#endregion

		#region IEventLogWriter Members

		public string Source
		{
			get
			{
				return this.source;
			}
			set
			{
				if(value!=null && value!= "" && value!=this.source)
					this.source = value;
			}
		}

		public string Log
		{
			get
			{
				return this.log;
			}
			set
			{
				if(value!=null && value!= "" && value!=this.log)
				{
					this.log = value;
					this.source = "";
					this.logDisplayName = "";

					//Check to see if the log node exists
					XmlNode newLogNode = this.EventLogNode(value);
					if(newLogNode==null)
					{
						//the node does not exists
						this.nodeLog = this.CreateLogNode(this.log);

						//add to the eventlog collection
						if(this.eventLogCollection!=null)
						{
							EventLog e = new EventLog(log,"",this.logPath,this.logFileName, EventLogWriterType.XML);
							if(!this.eventLogCollection.Contains(e))
							{
								this.eventLogCollection.Add(e);
								this.OnEventLogCollectionUpdated();
							}
						}
					}
					else
					{
						//the node exists so set the new node
						this.nodeLog = newLogNode;
						this.logDisplayName = this.nodeLog.Attributes.GetNamedItem("logDisplayName").Value;
					}
				}
				
				//Save the log file
				this.SaveLogFile();

				//Load the eventlog collection
				this.LoadEventLogEntryCollection();
			}
		}

		public string LogDisplayName
		{
			get
			{
				return this.logDisplayName;
			}
			set
			{
				if(value!=null && value!= "" && value!=this.logDisplayName)
				{
					this.logDisplayName = value;
					//Set the logdisplayname in the node
					this.NodeAttributeValue(this.nodeLog,"logDisplayName",value);
				}
			}
		}

		public string LogFileName
		{
			get
			{
				return this.logFileName;
			}
		}

		public string LogPath
		{
			get
			{
				return this.logPath;
			}
		}

		public void Delete(string logName)
		{

			XmlNodeList list = this.xmlLog.GetElementsByTagName("log");
			foreach(XmlNode node in list)
			{
				string log = this.NodeAttributeValue(node,"name");
				if(log.Equals(logName))
				{
					//remove eventlog from collection
					this.RemoveEventLog(logName);
					this.nodeEventLog.RemoveChild(node);
					this.SaveLogFile();					
					break;
				}
			}
			
		}

		public void Clear()
		{
			//Clear all the log items from the xml
			int y = this.nodeLog.ChildNodes.Count;
			for(int x = 0; x<y;x++)
				this.nodeLog.RemoveChild(this.nodeLog.ChildNodes[0]);

			//Save the xml file
			this.SaveLogFile();

			//Re-init the eventLogCollection
			this.eventLogEntryCollection = new EventLogEntryCollection();
		}

		public void Close()
		{
			this.SaveLogFile();
			this.nodeLog = null;
			this.log = "";
			this.source ="";
			this.logDisplayName = "";
			this.eventLogEntryCollection = new EventLogEntryCollection();
		}

		public EventLog[] GetEventLogs()
		{
			if(this.eventLogCollection==null)
				this.LoadEventLogCollection();

			EventLog[] ret = (EventLog[])this.eventLogCollection.ToArray(typeof(EventLog));
			return ret;			
		}

		public EventLogEntryCollection Entries
		{
			get
			{
				return this.eventLogEntryCollection;
			}
		}

		public bool Exists(string logName)
		{
			XmlNodeList list = this.xmlLog.GetElementsByTagName("log");
			foreach(XmlNode node in list)
			{
				XmlAttribute att = (XmlAttribute)node.Attributes.GetNamedItem("name");
				if(att!=null)
					if(att.Value == logName)
						return true;
			}
			
			return false;
		}


		public void WriteEntry(string message)
		{
			this.WriteEntry(this.source,message,EventLogEntryType.Information,0,0,null);
		}
		public void WriteEntry(string message, EventLogEntryType type)
		{
			this.WriteEntry(this.source,message,type,0,0,null);
		}

		public void WriteEntry(string source, string message)
		{
			this.WriteEntry(source,message,EventLogEntryType.Information,0,0,null);
		}

		public void WriteEntry(string message, EventLogEntryType type, Int32 eventID)
		{
			this.WriteEntry(this.source,message,type,eventID,0,null);
		}

		public void WriteEntry(string source, string message, EventLogEntryType type)
		{
			this.WriteEntry(source,message,type,0,0,null);
		}

		public void WriteEntry(string message, EventLogEntryType type, int eventID, short category)
		{
			this.WriteEntry(this.source,message,type,eventID,category,null);
		}

		public void WriteEntry(string source, string message, EventLogEntryType type, int eventID)
		{
			this.WriteEntry(source,message,type,eventID,0,null);
		}

		public void WriteEntry(string message, EventLogEntryType type, int eventID, short category, byte[] rawData)
		{
			this.WriteEntry(this.source,message,type,eventID,category,rawData);
		}

		public void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category)
		{
			this.WriteEntry(source,message,type,eventID,category,null);
		}

		public void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category, byte[] rawData)
		{
			this.WriteEntryToLog(source, message, type, eventID, category, rawData);			
		}

		#endregion
	}
}
