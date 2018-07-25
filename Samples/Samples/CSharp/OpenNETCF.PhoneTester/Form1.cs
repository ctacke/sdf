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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Phone.Sms;
namespace OpenNETCF.PhoneTester
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            this.txtdeviceNumber.Text = "Our number is: " + new Sms().PhoneNumber.Address;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Microsoft.WindowsMobile.PocketOutlook.SmsMessage s = new Microsoft.WindowsMobile.PocketOutlook.SmsMessage(this.textboxNumber.Text, this.textboxMessage.Text);
            s.Send();
       }
    
      
        private void btnProperties_Click(object sender, EventArgs e)
        {
            //Get list of numbers on SIM card
            OpenNETCF.Phone.Sim.Sim sim = new OpenNETCF.Phone.Sim.Sim();
            for (int x = 0; x < sim.Phonebook.Count; x++)
                this.listBox1.Items.Add(sim.Phonebook[x].Text + "(" + sim.Phonebook[x].Address + ")");

            //Get the properties of the sim card
            this.MaximumPhonebookAddressLength.Text = sim.MaximumPhonebookAddressLength.ToString();
            this.MaximumPhonebookIndex.Text = sim.MaximumPhonebookIndex.ToString();
            this.MaximumPhonebookTextLength.Text = sim.MaximumPhonebookTextLength.ToString();
            this.MinimumPhonebookIndex.Text = sim.MinimumPhonebookIndex.ToString();
        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            Microsoft.WindowsMobile.Telephony.Phone p = new Microsoft.WindowsMobile.Telephony.Phone();
            p.Talk(this.txtCalledParty.Text, this.chkPrompt.Checked);
        }

        private void btnCallLog_Click(object sender, EventArgs e)
        {
            OpenNETCF.Phone.CallLog cl = new OpenNETCF.Phone.CallLog();
            this.lbCallLog.Items.Clear();
            for (int x = 0; x < cl.Count; x++)
            {
                if (cl[x] != null)
                {
                    string s = cl[x].Outgoing ? "<-" : "->";
                    s += cl[x].Name == null ? "Unknown" : cl[x].Name;
                    s += " (" + cl[x].Number == null ? "Number Unknown" : cl[x].Number;
                    this.lbCallLog.Items.Add(s);
                }
            }
        }
    }
}