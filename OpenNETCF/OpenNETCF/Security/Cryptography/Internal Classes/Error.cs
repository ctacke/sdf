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
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace OpenNETCF.Security.Cryptography.Internal
{
	internal class Error
	{
		private Error() {}

		//[DllImport("coredll.dll", EntryPoint="GetLastError", SetLastError=true)]
		//internal static extern uint GetLastError();
		/*
		public const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

		[DllImport("coredll.dll", EntryPoint="FormatMessage", SetLastError=true)]
		internal static extern uint FormatMessage(uint dwFlags, string lpSource, uint dwMessageId, 
			uint dwLanguageId, StringBuilder lpBuffer, uint nSize, string [] Arguments);
		*/
		public static ErrCode HandleRetVal(bool retVal)
		{
			ErrCode [] eca = new ErrCode[0];
			return HandleRetVal(retVal, eca);
		}

		public static ErrCode HandleRetVal(bool retVal, ErrCode expected)
		{
			ErrCode [] eca = new ErrCode[1];
			eca[0] = expected;
			return HandleRetVal(retVal, eca);
		}

		public static ErrCode HandleRetVal(bool retVal, ErrCode [] expected)
		{
			ErrCode ec = ErrCode.SUCCESS;
			if(retVal == false)
			{
				uint lastErr = (uint) Marshal.GetLastWin32Error();
				ec = (ErrCode) lastErr;
				bool isExpected = false;
				foreach(ErrCode expect in expected)
				{
					if(ec == expect)
						isExpected = true;
				}
				if(isExpected == false)
					throw new Exception("bNb.Sec: " + ec.ToString());						
			}
			return ec;
		}
	}
}
