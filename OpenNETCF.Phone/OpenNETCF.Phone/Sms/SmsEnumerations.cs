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

namespace OpenNETCF.Phone.Sms
{
	
	#region Data Encoding Enumeration
	/// <summary>
	/// The data encoding that is primarily used for outgoing text message types.
	/// </summary>
	/// <preliminary/>
	enum DataEncoding 
	{
		/// <summary>
		/// Chooses the data encoding that fully represents all of the characters in the least amount of space.
		/// </summary>
		Optimal=0,
		/// <summary>
		/// Use the default GSM 7-bit encoding specified in GSM specification 03.38 "Digital cellular telecommunications system (Phase 2+); Alphabets and language-specific information." Characters that cannot be represented by this encoding will not be transmitted correctly.
		/// </summary>
		Gsm,
		/// <summary>
		/// Use the Unicode UCS2 encoding.
		/// </summary>
		Ucs2,
	};
	#endregion

	#region Sms Mode Enumeration
	/// <summary>
	/// Specifies mode of the SMS object.
	/// </summary>
	/// <preliminary/>
	[Flags()]
	public enum SmsMode : int
	{
		/// <summary>
		/// Sms is opened for receiving incoming messages.
		/// </summary>
		Receive              = 0x00000001,
		/// <summary>
		/// Sms is opened for sending outgoing messages.
		/// </summary>
		Send                 = 0x00000002,
	}
	#endregion

	#region Delivery Options Enumeration
	/// <summary>
	/// Specifies options for delivery of an SMS Message.
	/// </summary>
	/// <preliminary/>
	public enum DeliveryOptions
	{
		/// <summary>
		/// No special options
		/// </summary>
		None	= 0x00000000,
		/// <summary>
		/// Unless this option is specified, the router will retry sending the SMS message according to a pre-defined short-term retry schedule.
		/// If this option is specified, no retries will be attempted.
		/// </summary>
		NoRetry	= 0x00000001,
	}
	#endregion

	#region Message Status Enumeration
	/// <summary>
	/// Specifies the state of an SMS Message.
	/// </summary>
	/// <preliminary/>
	[CLSCompliant(false),Flags()]
	public enum MessageStatus : uint
	{
		Unknown                  =(0x00000000),
		// Valid bits for dwMessageStatus0
		ReceivedBySME            =(0x00000001),
		ForwardedToSME           =(0x00000002),
		ReplacedBySC             =(0x00000004),
		Congestion_Trying        =(0x00000008),
		SMEBusy_Trying           =(0x00000010),
		SMENotResponding_Trying  =(0x00000020),
		SVCRejected_Trying       =(0x00000040),
		QualityUnavail_Trying    =(0x00000080),
		SMEError_Trying          =(0x00000100),
		Congestion               =(0x00000200),
		SMEBusy                  =(0x00000400),
		SMENotResponding         =(0x00000800),
		SVCRejected              =(0x00001000),
		QualityUnavail_Temp      =(0x00002000),
		SMEError                 =(0x00004000),
		RemoteProcError          =(0x00008000),
		IncompatibleDest         =(0x00010000),
		ConnectionRejected       =(0x00020000),
		NotObtainable            =(0x00040000),
		NoInternetworking        =(0x00080000),
		VPExpired                =(0x00100000),
		DeletedByOrigSME         =(0x00200000),
		DeletedBySC              =(0x00400000),
		NoLongerExists           =(0x00800000),
		QualityUnavail           =(0x01000000),
		Reserved_Completed       =(0x02000000),
		Reserved_Trying          =(0x04000000),
		Reserved_Error           =(0x08000000),
		Reserved_TmpError        =(0x10000000),
		SCSpecific_Completed     =(0x20000000),
		SCSpecific_Trying        =(0x40000000),
		SCSpecific_Error         =(0x80000000),
		// Valid bits for dwMessageStatus1,
		SCSpecific_TmpError      =(0x00000001),

	}
	#endregion

	#region Broadcast Language Enumeration
	/// <summary>
	/// Indicates the languages that the mobile device provides.
	/// </summary>
	/// <preliminary/>
	public enum BroadcastLanguage : int
	{
		Unknown			= 0x00000001,
		German          = 0x00000002,
		English         = 0x00000004,
		Italian         = 0x00000008,
		French          = 0x00000010,
		Spanish         = 0x00000020,
		Dutch           = 0x00000040,
		Swedish         = 0x00000080,
		Danish          = 0x00000100,
		Portuguese      = 0x00000200,
		Finnish         = 0x00000400,
		Norwegian       = 0x00000800,
		Greek           = 0x00001000,
		Turkish         = 0x00002000,
		Hungarian       = 0x00004000,
		Polish          = 0x00008000,
		Czech           = 0x00010000,
		All             = 0x0001ffff,

	}
	#endregion

	#region Message Class Enumeration
	/// <summary>
	/// Determines the class of the SMS Message.
	/// </summary>
	/// <preliminary/>
	public enum MessageClass : int
	{
		/// <summary>
		/// The message should be displayed immediately but not stored.
		/// </summary>
		Class0 = 0,
		/// <summary>
		/// The message should be stored and an acknowledgement should be sent to the Service Center when it is stored.
		/// </summary>
		Class1,
		/// <summary>
		/// The message should be transferred to the SMS data field in the subscriber identity module (SIM) before an acknowledgement is sent to the Service Center.
		/// If the message cannot be stored in the SIM and there is other short message storage available, an unspecified protocol error message is returned to the Service Center.
		/// See GSM specification 04.11 "Digital cellular telecommunications system (Phase 2+); Point-to-Point (PP) Short Message Service (SMS) support on mobile radio interface" for more detail.
		/// If all the short message storage is already in use, a memory error message is returned to the Service Center.
		/// </summary>
		Class2,
		/// <summary>
		/// 
		/// </summary>
		Class3,
		/// <summary>
		/// When the message has successfully reached the destination and can be stored, an acknowledgement is sent to the Service Center.
		/// See GSM specification 07.05 "Digital cellular telecommunications system (Phase 2+); Use of Data Terminal Equipment - Data Circuit terminating; Equipment (DTE - DCE) interface for Short Message Service (SMS) and Cell Broadcast Service (CBS)" for more detail.
		/// </summary>
		ClassUnspecified,
	}
	#endregion

	#region Replace Option Enumeration
	/// <summary>
	/// 
	/// </summary>
	public enum ReplaceOption : int
	{
		None = 0,
		ReplaceType1,
		ReplaceType2,
		ReplaceType3,
		ReplaceType4,
		ReplaceType5,
		ReplaceType6,
		ReplaceType7,
		ReturnCall,
		DePersonalization,
	}
	
	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum MessageOptions : int
	{
		/// <summary>
		/// No options are specified.
		/// </summary>
		None          = 0x00000000,
		/// <summary>
		/// 
		/// </summary>
		ReplyPath     = 0x00000001,
		/// <summary>
		/// Requests a delivery status report.
		/// </summary>
		StatusReport  = 0x00000002,
		/// <summary>
		/// Sets the Discard bit for the message.
		/// </summary>
		Discard       = 0x00000004,
	}
	#endregion

	#region Sms Errors Enumeration
	
	#endregion

	#region Notification Message Waiting Type Enumeration
	/// <summary>
	/// </summary>
	public enum NotificationMessageWaitingType : int
	{
		None = 0,
		Generic,
		VoiceMail,
		Fax,
		EMail,
		Other,
	}
	#endregion

	#region Notification Indicator Type Enumeration
	/// <summary>
	/// </summary>
	public enum NotificationIndicatorType  : int
	{
		None = 0,
		Line1 = 1,
		Line2 = 2,
	}
	#endregion

}
