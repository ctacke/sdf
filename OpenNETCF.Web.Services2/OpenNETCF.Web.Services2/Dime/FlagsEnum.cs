
using System;

namespace OpenNETCF.Web.Services2.Dime
{
	//[FlagsAttribute] private enum FlagsEnum:byte { BeginOfMessage=0x80, EndOfMessage = 0x40, ChunkedRecord = 0x20 };
	[FlagsAttribute] 
	internal enum FlagsEnum : byte 
	{ 
		BeginOfMessage=0x04, 
		EndOfMessage = 0x02, 
		ChunkedRecord = 0x01 
	}
}
