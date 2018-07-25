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
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Phone
{
	/// <summary>
	/// Creates new voice calls on the Pocket PC Phone Edition or Smartphone device.
	/// </summary>
	public class Phone
	{
		private Phone(){}

		/// <summary>
		/// Make a voice call to the specified number.
		/// </summary>
		/// <param name="destination">A valid phone number to be dialled.</param>
		/// <returns>True if successful else False</returns>
		public static bool MakeCall(string destination)
		{
			return MakeCall(destination, false, null);
		}
		/// <summary>
		/// Make a voice call to the specified number optionally prompting the user before dialling.
		/// </summary>
		/// <param name="destination">A valid phone number to be dialled.</param>
		/// <param name="prompt">If True user will be prompted before call is made, else call will be made without user intervention.</param>
		/// <returns>True if successful else False</returns>
		public static bool MakeCall(string destination, bool prompt)
		{
			

			return MakeCall(destination, prompt, null);
		}
		/// <summary>
		/// Make a voice call to the specified number optionally prompting the user before dialling.
		/// </summary>
		/// <param name="destination">A valid phone number to be dialled.</param>
		/// <param name="prompt">If True user will be prompted before call is made, else call will be made without user intervention.</param>
		/// <param name="calledParty">A display name for the party being called.</param>
		/// <returns>True if successful else False</returns>
		public static bool MakeCall(string destination, bool prompt, string calledParty)
		{
			//setup structure for native call
			MakeCallInfo mci = new MakeCallInfo();
			mci.cbSize = 24;
			mci.pszDestAddress = OpenNETCF.Runtime.InteropServices.Marshal2.StringToHGlobalUni(destination);

			if(calledParty!=null)
			{
				mci.pszCalledParty = OpenNETCF.Runtime.InteropServices.Marshal2.StringToHGlobalUni(calledParty);
			}

			if(prompt)
			{
				mci.dwFlags = CallFlags.PromptBeforeCalling;
			}
			else
			{
				mci.dwFlags = CallFlags.Default;
			}

			//call native function
			int result = PhoneMakeCall(ref mci);

			//free strings
			if(mci.pszDestAddress!=IntPtr.Zero)
			{
				Marshal.FreeHGlobal(mci.pszDestAddress);
			}

			if(mci.pszCalledParty!=IntPtr.Zero)
			{
				Marshal.FreeHGlobal(mci.pszCalledParty);
			}

			//check return value
			if(result==0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		[DllImport("phone.dll", SetLastError=true)]
		private static extern int PhoneMakeCall(ref MakeCallInfo ppmci);


		//used internally by PhoneMakeCallInfo
		private struct MakeCallInfo
		{
			public int cbSize;
			public CallFlags dwFlags;
			public IntPtr pszDestAddress;
			IntPtr pszAppName;
			public IntPtr pszCalledParty;
			IntPtr pszComment;
		}

		#region Emergency Calls List
		/*
		/// <summary>
		/// This function gets a list of emergency calls.
		/// </summary>
		/// <remarks>Emergency numbers are those that can be dialed any time, even when the device is locked.</remarks>
		/// <returns>An array of Strings containing emergency numbers</returns>
		public static string[] EmergencyCallList
		{
			get
			{
				byte[] buffer = new byte[256];
				int hresult = SHGetEmergencyCallList(buffer, buffer.Length);

				if(hresult != 0)
				{
					throw new Exception("Error retrieving emergency call list");
				}
				else
				{
					string output = System.Text.Encoding.Unicode.GetString(buffer, 0, buffer.Length - 2);
					return output.Split('\0');
				}
			}
		}

		[DllImport("coredll.dll", EntryPoint="#128")]
		private static extern int SHGetEmergencyCallList(byte[] buffer, int length);
		*/
		#endregion

		/// <summary>
		/// Flags which determine the behaviour of the <see cref="M:OpenNETCF.Phone.Phone.MakeCall"/> function.
		/// </summary>
		private enum CallFlags : int
		{
			/// <summary>
			/// Do not prompt, dial the supplied number.
			/// </summary>
			Default                = 0x00000001,
			/// <summary>
			/// Prompt the user whether to dial the supplied number.
			/// </summary>
			PromptBeforeCalling    = 0x00000002,
		}

	}
}
