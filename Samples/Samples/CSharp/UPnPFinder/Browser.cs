#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



//----------------------------------------------------------------------------
//  This file is part of the OpenNETCF Smart Device Framework Code Samples.
// 
//  Copyright (C) OpenNETCF Consulting, LLC.  All rights reserved.
// 
//  This source code is intended only as a supplement to Smart Device 
//  Framework and/or on-line documentation.  
// 
//  THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
//----------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UPnPFinder
{
    public partial class Browser : Form
    {
        private IUPnPService service;
        XmlNamespaceManager mgr;
        
        public Browser(IUPnPService svc)
        {
            InitializeComponent();
            service = svc;
            TreeNode root = new TreeNode("Root");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(@"<DIDL-Lite xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:upnp=""urn:schemas-upnp-org:metadata-1-0/upnp/"" xmlns=""urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/"">
<container id=""0"">
  <dc:title>Music</dc:title> 
  <upnp:class>object.container</upnp:class></container></DIDL-Lite>");
            root.Tag = doc.DocumentElement.ChildNodes[0];
            root.Nodes.Add("");
            tvItems.Nodes.Add(root);
            mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("upnp", "urn:schemas-upnp-org:metadata-1-0/upnp/");
            mgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
        }

        private void tvItems_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Tag == null && e.Node.Nodes[0].Text == "")
                e.Node.Nodes.Clear();
            else
                return;

            XmlElement elem = (XmlElement)e.Node.Tag;
            if (elem == null)
            {
                e.Cancel = true;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            //if (elem["upnp:class"].InnerText == "object.container" && elem.LocalName == "container")
            {
                object results;// = new object[3];
                object retVal;
                int cItems = 10;
                while (cItems > 0)
                {
                    cItems = 0;
                    XmlDocument doc = GetItems(service, elem.GetAttribute("id"), "BrowseDirectChildren", "*", e.Node.Nodes.Count, 10);

                    //int hr = service.InvokeAction("Browse", new object[] { elem.GetAttribute("id"), "BrowseDirectChildren", "*", e.Node.Nodes.Count, 10, "" }, out results, out retVal);
                    //if (hr == 0)
                    //{
                    //    doc.LoadXml(((object[])results)[0].ToString());
                    foreach (XmlElement el in doc.DocumentElement.ChildNodes)
                    {
                        TreeNode node = new TreeNode(el["dc:title"].InnerText);
                        node.Tag = el;
                        node.Nodes.Add("");
                        e.Node.Nodes.Add(node);
                        cItems++;
                    }

                    //}
                    //hr = 0;
                }

            }
            Cursor.Current = Cursors.Default;

        }

        private void tvItems_AfterSelect(object sender, TreeViewEventArgs e)
        {
            XmlElement elem = (XmlElement)e.Node.Tag;
            if (elem == null)
            {
                return;
            }
            if (elem.LocalName == "item")
            {
                string url = elem["res"].InnerText;

                //MessageBox.Show(elem.OuterXml);
                int size = int.MaxValue;
                int cx = 0, cy = 0;
                foreach (XmlElement elRes in elem.ChildNodes)
                {
                    if (elRes.Name != "res")
                        continue;
                    string[] parts = elRes.GetAttribute("resolution").Split('x');
                    if (parts.Length != 2)
                        continue;
                    int cxThis = int.Parse(parts[0]);
                    int cyThis = int.Parse(parts[1]);
                    parts = elRes.GetAttribute("protocolInfo").Split(':');
                    if (parts.Length < 3 || parts[0] != "http-get" || parts[2] != "image/jpeg")
                        continue;
                    if (cxThis * cyThis < Screen.PrimaryScreen.WorkingArea.Width * Screen.PrimaryScreen.WorkingArea.Height || cxThis == Screen.PrimaryScreen.WorkingArea.Width)
                    {
                        url = elRes.InnerText;
                        cx = cxThis;
                        cy = cyThis;
                        break;
                    }
                }
                url = url.Replace("%7b", "{");
                url = url.Replace("%7d", "}");
                if (elem["upnp:class"].InnerText == "object.item.audioItem.musicTrack")
                {
                    System.Diagnostics.Process.Start(url, "");
                }

                if (elem["upnp:class"].InnerText == "object.item.imageItem.photo")
                {
                    Cursor.Current = Cursors.WaitCursor;
                    PictureViewer viewer = new PictureViewer(url, cx, cy);
                    viewer.ShowDialog();
                    viewer.Dispose();
                }
            }

        }

        public static XmlDocument GetItems(IUPnPService svc, string containerID, string criteria, string filter, int startItem, int count)
        {
            XmlDocument doc = new XmlDocument();

            object results;
            object retVal;
            int hr = svc.InvokeAction("Browse", new object[] { containerID, criteria, filter, startItem, count, "" }, out results, out retVal);
            if (hr == 0)
                doc.LoadXml(((object[])results)[0].ToString());
            return doc;
        }


    }

}