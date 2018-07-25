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
Imports System.Reflection
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("KeyRecorder")>
<Assembly: AssemblyDescription("")>
<Assembly: AssemblyCompany("Hound Dog Enterprises, LLC")>
<Assembly: AssemblyProduct("KeyRecorder")>
<Assembly: AssemblyCopyright("Copyright © Hound Dog Enterprises, LLC 2008")>
<Assembly: AssemblyTrademark("")>

<Assembly: CLSCompliant(True)>

<Assembly: ComVisible(False)>

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("6cb1a5ca-06db-419c-95b4-13abd7416993")>

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers
' by using the '*' as shown below:
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("1.0.0.0")>

'Below attribute is to suppress FxCop warning "CA2232 : Microsoft.Usage : Add STAThreadAttribute to assembly"
' as Device app does not support STA thread.
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2232:MarkWindowsFormsEntryPointsWithStaThread")>
