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



using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Net.NetworkInformation;

namespace NICViewer
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();

      nicList.Columns.Add("Name", -1, HorizontalAlignment.Left);
      nicList.Columns.Add("IP", -1, HorizontalAlignment.Left);

      nicList.ContextMenu = nicContext;

      RefreshNICList();
    }


    private void RefreshNICList()
    {
      nicList.BeginUpdate();
      nicList.Items.Clear();

      foreach (INetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
      {
        ListViewItem lvi = new ListViewItem(
          new string[] {
            ni.Name,
            ni.CurrentIpAddress.ToString()
          });

        lvi.Tag = ni;
        nicList.Items.Add(lvi);
      }
      nicList.EndUpdate();
    }

    NicProperties m_propsDialog = null;

    private void nicPropertiesMenuItem_Click(object sender, EventArgs e)
    {
      if (nicList.SelectedIndices.Count != 1) return;

      if (m_propsDialog == null)
      {
        // lazy load
        m_propsDialog = new NicProperties();
      }

      m_propsDialog.NIC = nicList.Items[nicList.SelectedIndices[0]].Tag as INetworkInterface;
      m_propsDialog.ShowDialog();

      // refresh list on return
      RefreshNICList();
    }
  }
}