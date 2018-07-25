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

namespace FileSystemWatcherSample
{
    public partial class Form1 : Form
    {
        private OpenNETCF.IO.FileSystemWatcher fsw;

        public Form1()
        {
            InitializeComponent();
            fsw = new OpenNETCF.IO.FileSystemWatcher("\\", "*.*");
            fsw.IncludeSubdirectories = true;
            fsw.NotifyFilter = OpenNETCF.IO.NotifyFilters.CreationTime | OpenNETCF.IO.NotifyFilters.DirectoryName | OpenNETCF.IO.NotifyFilters.FileName | OpenNETCF.IO.NotifyFilters.LastAccess;
            fsw.EnableRaisingEvents = true;
            fsw.Changed += new OpenNETCF.IO.FileSystemEventHandler(fsw_Changed);
            fsw.Created += new OpenNETCF.IO.FileSystemEventHandler(fsw_Created);
            fsw.Deleted += new OpenNETCF.IO.FileSystemEventHandler(fsw_Deleted);
            fsw.Renamed += new OpenNETCF.IO.RenamedEventHandler(fsw_Renamed);
        }

        void fsw_Renamed(object sender, OpenNETCF.IO.RenamedEventArgs e)
        {
            this.Text = e.OldName + "->" + e.Name;
            this.BackColor = Color.Orange;
        }

        void fsw_Deleted(object sender, OpenNETCF.IO.FileSystemEventArgs e)
        {
            this.Text = e.Name;
            this.BackColor = Color.Red;
        }

        void fsw_Created(object sender, OpenNETCF.IO.FileSystemEventArgs e)
        {
            this.Text = e.Name;
            this.BackColor = Color.Green;
        }

        void fsw_Changed(object sender, OpenNETCF.IO.FileSystemEventArgs e)
        {
            this.Text = e.Name;
            this.BackColor = Color.Yellow;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.F1))
            {
                // Soft Key 1
                // Not handled when menu is present.
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.F2))
            {
                // Soft Key 2
                // Not handled when menu is present.
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                // Up
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                // Down
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Left))
            {
                // Left
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Right))
            {
                // Right
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                // Enter
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D1))
            {
                // 1
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D2))
            {
                // 2
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D3))
            {
                // 3
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D4))
            {
                // 4
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D5))
            {
                // 5
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D6))
            {
                // 6
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D7))
            {
                // 7
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D8))
            {
                // 8
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D9))
            {
                // 9
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.F8))
            {
                // *
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.D0))
            {
                // 0
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.F9))
            {
                // #
            }

        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            System.IO.StreamWriter sw = System.IO.File.CreateText("\\My Documents\\test.txt");
            sw.WriteLine("Test");
            sw.Flush();
            sw.Close();
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            System.IO.File.Move("\\My Documents\\test.txt", "\\My Documents\\rename.txt");
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            System.IO.File.Delete("\\My Documents\\rename.txt");
        }

       
    }
}