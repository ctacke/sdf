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



Imports OpenNETCF.WindowsCE

Public Class Form1
    ' create the timer object
    Dim WithEvents timer As New LargeIntervalTimer

    Private Sub start_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles start.Click
        ' disable the start button so it can be clicked only once
        start.Enabled = False

        ' update a label to show when we started
        started.Text = DateTime.Now.ToString("HH:mm:ss")

        ' set the timer's properties
        timer.OneShot = False                               'run forever
        timer.FirstEventTime = DateTime.Now.AddSeconds(15)  ' start in 15 seconds
        timer.Interval = New TimeSpan(0, 0, 30)             ' fire every 30 seconds after that

        ' start the timer
        timer.Enabled = True
    End Sub

    Private Sub OnTick(ByVal sender As Object, ByVal args As EventArgs) Handles timer.Tick
        ' update a label to show we've ticked
        lastTick.Text = DateTime.Now.ToString("HH:mm:ss")
    End Sub
End Class

