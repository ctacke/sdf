
using System;
using System.Xml;
using System.Collections;

namespace OpenNETCF.Web.Services2
{
	public class LameXpath
	{
		//NOTE these are not namespace aware
		//doesnt return XmlNodeList either
		public static XmlElement SelectSingleNode(XmlNode xn, string nodeName)
		{
			return SelectSingleNode(xn, nodeName, null);
		}

		public static XmlElement SelectSingleNode(string id, XmlNode xn)
		{
			XmlElement retElem = null;
			if(xn.Attributes != null)
			{
				foreach(XmlAttribute xa in xn.Attributes)
				{
					if(xa.LocalName == "Id")
						if(xa.Value == id)
							return (XmlElement) xn;
				}
			}
			XmlNodeList xnl = xn.ChildNodes;
			foreach(XmlNode xnIter in xnl)
			{
				retElem = SelectSingleNode(id, xnIter);
				if(retElem != null) break;
			}
			return retElem;
		}

		public static XmlElement SelectSingleNode(XmlNode xn, string nodeName, string Id)
		{
			int count = 0; //ignore
			return SelectSingleNode(xn, nodeName, Id, ref count);
		}

		public static XmlElement SelectSingleNode(XmlNode xn, string nodeName, string Id, ref int count)
		{
			XmlElement retElem = null;
			if(xn.LocalName == nodeName)
			{
				if(Id == null)
					return (XmlElement) xn;
				else //check Id
				{
                    if(xn.Attributes["Id"] != null)
						if(xn.Attributes["Id"].Value == Id)
							return (XmlElement) xn;
				}
			}
			XmlNodeList xnl = xn.ChildNodes;
			foreach(XmlNode xnIter in xnl)
			{
				count++;
				//retElem = SelectSingleNode(xnIter, nodeName); //is this breaking?
				retElem = SelectSingleNode(xnIter, nodeName, null, ref count);
				if(retElem != null) break;
			}
			return retElem;
		}

		public static ArrayList SelectChildNodes(XmlNode xn, string nodeName)
		{
			ArrayList alXnl = new ArrayList();
			foreach (XmlNode xnc in xn.ChildNodes)
			{
				if (xnc.LocalName == nodeName)
					alXnl.Add(xnc);
			}
			return alXnl;
		}

		public static ArrayList SelectChildNodes(XmlNode xn, string nodeName, string childrenName)
		{
			XmlElement xe = SelectSingleNode(xn, nodeName);
			return SelectChildNodes(xe, childrenName);
		}
	}
}
