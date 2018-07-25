#Region "--- Copyright Information ---"
' *******************************************************************
'|                                                                   |
'|           OpenNETCF Smart Device Framework 2.2                    |
'|                                                                   |
'|                                                                   |
'|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
'|       ALL RIGHTS RESERVED                                         |
'|                                                                   |
'|   The entire contents of this file is protected by U.S. and       |
'|   International Copyright Laws. Unauthorized reproduction,        |
'|   reverse-engineering, and distribution of all or any portion of  |
'|   the code contained in this file is strictly prohibited and may  |
'|   result in severe civil and criminal penalties and will be       |
'|   prosecuted to the maximum extent possible under the law.        |
'|                                                                   |
'|   RESTRICTIONS                                                    |
'|                                                                   |
'|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
'|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
'|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
'|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
'|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
'|                                                                   |
'|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
'|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
'|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
'|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
'|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
'|                                                                   |
'|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
'|   ADDITIONAL RESTRICTIONS.                                        |
'|                                                                   |
' ******************************************************************* 
#End Region



<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.start = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.lastTick = New System.Windows.Forms.Label
        Me.started = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'start
        '
        Me.start.Location = New System.Drawing.Point(47, 19)
        Me.start.Name = "start"
        Me.start.Size = New System.Drawing.Size(141, 31)
        Me.start.TabIndex = 0
        Me.start.Text = "Start"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.Label1.Location = New System.Drawing.Point(3, 107)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(234, 20)
        Me.Label1.Text = "Last Tick Was At"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lastTick
        '
        Me.lastTick.Location = New System.Drawing.Point(3, 127)
        Me.lastTick.Name = "lastTick"
        Me.lastTick.Size = New System.Drawing.Size(234, 20)
        Me.lastTick.Text = "<not fired>"
        Me.lastTick.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'started
        '
        Me.started.Location = New System.Drawing.Point(3, 82)
        Me.started.Name = "started"
        Me.started.Size = New System.Drawing.Size(234, 20)
        Me.started.Text = "<not started>"
        Me.started.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.Label3.Location = New System.Drawing.Point(3, 62)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(234, 20)
        Me.Label3.Text = "Test Started At"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.started)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lastTick)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.start)
        Me.Menu = Me.mainMenu1
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents start As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lastTick As System.Windows.Forms.Label
    Friend WithEvents started As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label

End Class
