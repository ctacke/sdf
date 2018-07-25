
using System;
using System.Collections;
using System.Collections.Specialized;

namespace OpenNETCF.Configuration
{
	class ReadOnlyNameValueCollection : NameValueCollection
	{

		//public ReadOnlyNameValueCollection(IHashCodeProvider hcp, IComparer comp) : base(hcp, comp)
        public ReadOnlyNameValueCollection(IEqualityComparer equalityComparer) : base(equalityComparer)
		{
		}

		public ReadOnlyNameValueCollection(ReadOnlyNameValueCollection value) : base(value)
		{
		}

		public void SetReadOnly()
		{
			base.IsReadOnly = true;
		}
	}
}
