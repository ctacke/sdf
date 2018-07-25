
using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace OpenNETCF.Net.Ftp
{
	/// <summary>
	/// Summary description for X509CertificateCollection.
	/// </summary>
	public class X509CertificateCollection : CollectionBase
	{
		public X509CertificateCollection()
		{
		}

		public X509Certificate this[ int index ]  
		{
			get  
			{
				return (X509Certificate)List[index];
			}
			set  
			{
				List[index] = value;
			}
		}

		public int Add( X509Certificate value )  
		{
			return List.Add( value );
		}

		public int IndexOf( X509Certificate value )  
		{
			return List.IndexOf( value );
		}

		public void Insert( int index, X509Certificate value )  
		{
			List.Insert( index, value );
		}

		public void Remove( X509Certificate value )  
		{
			List.Remove( value );
		}

		public bool Contains( X509Certificate value )  
		{
			return List.Contains( value );
		}

		protected override void OnInsert( int index, object value )  
		{
			if( value.GetType() != typeof(X509Certificate) )
				throw new ArgumentException( "value must be of type X509Certificate.", "value" );
		}

		protected override void OnRemove( int index, object value )  
		{
			if( value.GetType() != typeof(X509Certificate) )
				throw new ArgumentException( "value must be of type X509Certificate.", "value" );
		}

		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
			if( newValue.GetType() != typeof(X509Certificate) )
				throw new ArgumentException( "newValue must be of type X509Certificate.", "newValue" );
		}

		protected override void OnValidate( Object value )  
		{
			if( value.GetType() != typeof(X509Certificate) )
				throw new ArgumentException( "value must be of type X509Certificate." );
		}
	}
}
