using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.ComponentModel
{
	class LicenseException : Exception
	{
		private const string m_licenseKeyMessage = "License key format is invalid.";
		
		public LicenseException()
			: base( m_licenseKeyMessage )
		{
		}

        public LicenseException(string message)
            : base(message)
        {
        }
	}
}
