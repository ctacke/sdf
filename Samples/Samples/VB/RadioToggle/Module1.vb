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



Imports System
Imports System.Collections.Generic
Imports System.Text
Imports OpenNETCF.WindowsMobile
Imports System.Threading
Imports System.Diagnostics

Namespace RadioToggle

    Module Module1

        Sub Main()
            Dim radios As Radios = radios.GetRadios()

            Debug.WriteLine("" & Chr(10) & "Before" & Chr(13) & Chr(10) & "--------")
            For Each radio As IRadio In radios
                Debug.WriteLine(String.Format("Name: {0}, Type: {1}, State: {2}", radio.DeviceName, radio.RadioType.ToString(), radio.RadioState.ToString()))

                ' toggle all radio states 
                radio.RadioState = IIf((radio.RadioState = RadioState.On), RadioState.Off, RadioState.On)
            Next

            ' give the radios enough time to change state - some (like BT) seem to be slow 
            Thread.Sleep(1000)

            radios.Refresh()

            ' display again 
            Debug.WriteLine("" & Chr(13) & Chr(10) & "After" & Chr(13) & Chr(10) & "--------")
            For Each radio As IRadio In radios
                Debug.WriteLine(String.Format("Name: {0}, Type: {1}, State: {2}", radio.DeviceName, radio.RadioType.ToString(), radio.RadioState.ToString()))
            Next
            Debug.WriteLine("" & Chr(13) & Chr(10) & Chr(10))
            Thread.Sleep(100)

        End Sub

    End Module
End Namespace

