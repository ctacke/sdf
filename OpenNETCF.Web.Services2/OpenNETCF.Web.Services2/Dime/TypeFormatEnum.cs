
using System;

namespace OpenNETCF.Web.Services2.Dime 
{
    /// <summary>
    /// Specifies possible Type formats as defined in the DIME specification. For example,
    /// if the type format is TypeFormatEnum.MediaType then a valid Type would be
    /// "plain/text; charset=utf-8" or "image/jpeg".
    /// </summary>
    //public enum TypeFormatEnum {
	public enum TypeFormatEnum:byte //mod
	{
		Unchanged = 0x0,
        MediaType = 0x10,
        AbsoluteUri = 0x20,
        Unknown = 0x30,
        None = 0x04
    }
}
