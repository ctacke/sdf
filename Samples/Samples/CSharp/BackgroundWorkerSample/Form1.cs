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
using OpenNETCF.ComponentModel;
using System.Threading;

namespace BackgroundWorkerSample
{
  public partial class Form1 : Form
  {
    private BackgroundWorker m_worker;

    public Form1()
    {
      InitializeComponent();

      m_worker = new BackgroundWorker();
      m_worker.RunWorkerCompleted += RunWorkerCompletedProc;
      m_worker.DoWork += DoWorkProc;
      m_worker.ProgressChanged += ProgressChangedProc;
      m_worker.WorkerReportsProgress = true;
      progressLabel.Text = "0%";
    }

    void ProgressChangedProc(object sender, ProgressChangedEventArgs e)
    {
      progress.Value = e.ProgressPercentage;
      progressLabel.Text = string.Format("{0}%", e.ProgressPercentage);
    }

    void DoWorkProc(object sender, DoWorkEventArgs e)
    {
      for(int i = 0 ; i < 100 ; i++)
      {
        m_worker.ReportProgress(i);
        Thread.Sleep(100);
      }
    }

    void RunWorkerCompletedProc(object sender, RunWorkerCompletedEventArgs e)
    {
      Thread.Sleep(400);
      start.Enabled = true;
      progress.Value = 0;
      progressLabel.Text = "0%";
    }

    private void start_Click(object sender, EventArgs e)
    {
      start.Enabled = false;
      m_worker.RunWorkerAsync();
    }
  }
}