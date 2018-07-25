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



using System.Windows.Forms;

namespace OpenNETCF.Windows.Forms
{
    partial class ProgressBar2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ProgressBar2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Name = "ProgressBar2";
            this.Size = new System.Drawing.Size(220, 21);
            this.ResumeLayout(false);

        }

        #endregion

        private void ResetBarColor()
        {
            this.BarColor = System.Drawing.SystemColors.Highlight; 
        }
        private bool ShouldSerializeBarColor()
        {
            return (this.barColor.ToArgb() != System.Drawing.SystemColors.Highlight.ToArgb());
        }

        private void ResetBarGradientColor()
        {
            this.BarGradientColor = System.Drawing.Color.White;
        }
        private bool ShouldSerializeBarGradientColor()
        {
            return (this.barGradientColor.ToArgb() != System.Drawing.Color.White.ToArgb());
        }

        private void ResetBorderColor()
        {
            this.BorderColor = System.Drawing.Color.Black;
        }
        private bool ShouldSerializeBorderColor()
        {
            return (this.borderColor.ToArgb() != System.Drawing.Color.Black.ToArgb());
        }

        private void ResetBorderPadding()
        {
            this.BorderPadding = new System.Drawing.Point(2, 2);
        }
        private bool ShouldSerializeBorderPadding()
        {
            bool t = this.borderPadding.Equals(new System.Drawing.Point(2, 2));
            return !t;
        }

        private void ResetBorderStyle()
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }
        private bool ShouldSerializeBorderStyle()
        {
            return this.borderStyle != System.Windows.Forms.BorderStyle.FixedSingle;
        }

        private void ResetDrawStyle()
        {
            this.DrawStyle = DrawStyle.Gradient;
        }
        private bool ShouldSerializeDrawStyle()
        {
            return this.drawStyle != DrawStyle.Gradient;
        }

        private void ResetGradientStyle()
        {
            this.GradientStyle = GradientStyle.Normal;
        }
        private bool ShouldSerializeGradientStyle()
        {
            return this.gradientStyle != GradientStyle.Normal;
        }
        
    }
}
