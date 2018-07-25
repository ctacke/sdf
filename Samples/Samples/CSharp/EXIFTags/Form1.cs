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
using OpenNETCF.Drawing.Imaging;

namespace OpenNETCF.Drawing.Imaging.Samples
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamOnFile st = new StreamOnFile(openFileDialog1.FileName);
                IImageDecoder decoder = null;
                ImagingFactory factory = new ImagingFactoryClass();
                factory.CreateImageDecoder(st, DecoderInitFlag.DecoderInitFlagNone, out decoder);
                ImageProperty[] props = ImageUtils.GetAllProperties(decoder);
                foreach (ImageProperty prop in props)
                {
                    //For Specific tags see ImageTag enum
                    textBox1.Text = prop.Id.ToString() + ": " + GetValue(prop) + "\r\n" + textBox1.Text;
                }
                decoder.TerminateDecoder();
            }
        }

        private string GetValue(ImageProperty value)
        {
            if(value.GetValue().GetType() == typeof(byte[]))
            {
                //Modification may be needed here depending on your requirments since an ASCII string
                //is not always in the tag value.
                string s = Encoding.ASCII.GetString((byte[])value.GetValue(), 0, ((byte[])value.GetValue()).Length);
                if (s.IndexOf('\0') > 0)
                    return s.Substring(0, s.IndexOf('\0'));
                else
                    return s;
            }
            else
                return value.GetValue().ToString().Replace("\0", "");
        }
    }
}