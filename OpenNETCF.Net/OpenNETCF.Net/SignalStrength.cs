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

namespace OpenNETCF.Net
{
	/// <summary>
	/// The SignalStrength class provides a repository for the
	/// signal strength indication from an RF Ethernet network
	/// adapter.  From an instance of this class, you can get
	/// the signal strength in dB, an enumerated value of 
	/// type StrengthType indicating the relative strength, or
	/// a string representing the relative strength.
	/// </summary>
    [Obsolete("This class is obsolete and will be removed in a future version of the SDF.  Consider using OpenNETCF.Net.NetworkInformation.SignalStrength instead", false)]
    public class SignalStrength
	{
		private int decibels;

		private SignalStrength() {}

		/// <summary>
		/// The Decibels property returns the signal strength
		/// in dB.
		/// </summary>
		public int Decibels
		{
			get { return decibels; }
		}

		/// <summary>
		/// The DBToStrength static conversion function can
		/// be used to take a dB strength value previously
		/// retrieved and generate a StrengthType enumeration
		/// value based on the relative strength.
		/// </summary>
		/// <param name="db">
		/// Signal strength in dB
		/// </param>
		/// <returns>
		/// StrengthType indicating strength that db parameter represents
		/// </returns>
		public static StrengthType DBToStrength( int db )
		{
			if ( db == 0 )
				return StrengthType.NotAWirelessAdapter;

			StrengthType	retval;
			if(db < -90)
				retval =  StrengthType.NoSignal;
			else if(db < -81)
				retval = StrengthType.VeryLow;
			else if(db < -71)
				retval = StrengthType.Low;
			else if(db < -67)
				retval = StrengthType.Good;
			else if(db < -57)
				retval = StrengthType.VeryGood;
			else
				retval = StrengthType.Excellent;

			return retval;
		}

		/// <summary>
		/// The DBToString static conversion function allows a
		/// signal strength value in dB previously retrieved
		/// to be converted into the string representation of
		/// its relative signal strength.
		/// </summary>
		/// <param name="db">
		/// Signal strength in dB.
		/// </param>
		/// <returns>
		/// String: "No Signal", "Very Low", "Low", "Good", 
		/// "Very Good", "Excellent", or 
		/// "Not a wireless adapter". 
		/// </returns>
		public static String DBToString( int db )
		{
			StrengthType	st = DBToStrength( db );
			return StrengthToString( st );
		}

		/// <summary>
		/// The StrengthToString static conversion function
		/// allows a previously stored signal strength 
		/// enumeration value to be converted into its string
		/// representation.
		/// </summary>
		/// <param name="st">
		/// One of the SignalStrength enumeration values.
		/// </param>
		/// <returns>
		/// String: "No Signal", "Very Low", "Low", "Good", 
		/// "Very Good", "Excellent", or 
		/// "Not a wireless adapter". 
		/// </returns>
		public static String StrengthToString( StrengthType st )
		{
			string retval = string.Empty;

			switch( st )
			{
				case StrengthType.NotAWirelessAdapter:
					return "Not a wireless adapter";
				case StrengthType.VeryLow:
					return "Very Low";
				case StrengthType.Low:
					return "Low";
				case StrengthType.Good:
					return "Good";
				case StrengthType.VeryGood:
					return "Very Good";
				case StrengthType.Excellent:
					return "Excellent";
			}

			// If we don't catch the value above, empty is
			// probably the best string to return.
			return retval;
		}

		/// <summary>
		/// Strength of signal as enumerated type.
		/// </summary>
		public StrengthType Strength
		{
			get 
			{
				return DBToStrength( decibels );
			}
		}

		internal SignalStrength(int Decibels)
		{
			this.decibels = Decibels;
		}

		/// <summary>
		/// Converts strength to string representing relative
		/// strength ("Good", "Low", etc.)
		/// </summary>
		/// <returns>
		/// String: "No Signal", "Very Low", "Low", "Good", 
		/// "Very Good", "Excellent", or 
		/// "Not a wireless adapter". 
		/// </returns>
		public override string ToString()
		{
			return DBToString( decibels );
		}
	}
}
