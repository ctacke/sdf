//i modified this to work in a class library

//********************************************************************************
//
// DecodeCertKey
// X.509 Certificate to Public Key Decoder
//
// Copyright (C) 2003.  Michel I. Gallant
//
//*********************************************************************************
//
// DecodeCertKey.cs
//
// This C# utility for .NET Framework 1.0/1.1
// Displays the ASN.1 encoded public key and CryptoAPI PUBLICKEYBLOB for any X.509 certificate
// Allows saving both encoded and decoded public keys to file.
// Decodes the PUBLICKEYBLOB into fields and displays exponent and modulus bytes
// in big-endian form suitable for RSAParameters fields.
//
// Handles either binary DER or BASE64 encoded X.509 v3 certificates.
// 
//**********************************************************************************


using System;
using System.IO;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;

using OpenNETCF.Security.Cryptography;

namespace OpenNETCF.Web.Services2 
{
	public class Win32 
	{

		/*
		BOOL WINAPI CryptDecodeObject(
		  DWORD dwCertEncodingType,
		  LPCSTR lpszStructType,
		  const BYTE* pbEncoded,
		  DWORD cbEncoded,
		  DWORD dwFlags,
		  void* pvStructInfo,
		  DWORD* pcbStructInfo
		);
		*/

		/*
		[DllImport("crypt32.dll")]
		public static extern bool CryptDecodeObject(
			uint CertEncodingType,
			uint lpszStructType,
			byte[] pbEncoded,
			uint cbEncoded,
			uint flags,
			[In, Out] byte[] pvStructInfo,
			ref uint cbStructInfo);
		*/

		[CLSCompliant(false)]
		[DllImport("crypt32.dll", EntryPoint="CryptDecodeObjectEx", SetLastError=true)]
		public static extern bool CryptDecodeObject(UInt32 CertEncodingType, UInt32 lpszStructType, byte[] pbEncoded, UInt32 cbEncoded, UInt32 flags, IntPtr pDecodePara, byte[] pvStructInfo, ref UInt32 cbStructInfo);
		//BOOL WINAPI CryptDecodeObjectEx(DWORD dwCertEncodingType, LPCSTR lpszStructType, const BYTE* pbEncoded, DWORD cbEncoded, DWORD dwFlags, PCRYPT_DECODE_PARA pDecodePara, void* pvStructInfo, DWORD* pcbStructInfo);
	}


	[StructLayout(LayoutKind.Sequential)]
	public struct PUBKEYBLOBHEADERS 
	{
		public byte bType;	//BLOBHEADER
		public byte bVersion;	//BLOBHEADER
		public short reserved;	//BLOBHEADER
		[CLSCompliant(false)]
		public UInt32 aiKeyAlg;	//BLOBHEADER
		[CLSCompliant(false)]
		public UInt32 magic;	 //RSAPUBKEY
		[CLSCompliant(false)]
		public UInt32 bitlen;	 //RSAPUBKEY
		[CLSCompliant(false)]
		public UInt32 pubexp;	 //RSAPUBKEY
	}

	public class DecodeCertKey
	{
		const UInt32 X509_ASN_ENCODING 		= 0x00000001;
		const UInt32 PKCS_7_ASN_ENCODING 	= 0x00010000;
		const UInt32 CERT_FIND_SUBJECT_STR	= 0x00080007;
		static UInt32 ENCODING_TYPE 		= PKCS_7_ASN_ENCODING | X509_ASN_ENCODING ;
		const UInt32 RSA_CSP_PUBLICKEYBLOB	= 19;

		public static void GetPublicRsaParams(X509Certificate cert, out byte[] exponent, out byte[] modulus)
		{
			exponent = null;
			modulus = null;
			//X509Certificate cert = null;
			byte[] encodedpubkey = null;
			byte[] publickeyblob = null;

			/*
			if(args.Length<1)
			{
				usage();
				return;
			}
			String CERTFILE_NAME = args[0] ;
			if (!File.Exists(CERTFILE_NAME))
			{
				Console.WriteLine("File '{0}' not found.", CERTFILE_NAME);
				return;
			}

			String basefname = Path.GetFileNameWithoutExtension(CERTFILE_NAME);

			// Try loading certificate as binary DER into an X509Certificate object.
			try
			{
				cert = X509Certificate.CreateFromCertFile(CERTFILE_NAME);
			}
			catch(System.Security.Cryptography.CryptographicException) 
			{ //not binary DER; try BASE64 format
				StreamReader sr = File.OpenText(CERTFILE_NAME);
				String filestr = sr.ReadToEnd();
				sr.Close();
				StringBuilder sb = new StringBuilder(filestr) ;
				sb.Replace("-----BEGIN CERTIFICATE-----", "") ;
				sb.Replace("-----END CERTIFICATE-----", "") ;
				//Decode 
				try
				{        //see if the file is a valid Base64 encoded cert
					byte[] certBytes = Convert.FromBase64String(sb.ToString()) ;
					cert =  new X509Certificate(certBytes);
				}
				catch(System.FormatException) 
				{
					Console.WriteLine("Not valid binary DER or Base64 X509 certificate format");
					return;
				}
				catch(System.Security.Cryptography.CryptographicException) 
				{
					Console.WriteLine("Not valid binary DER or Base64 X509 certificate format");
					return;
				}
			}
			*/

			// Get the asn.1 encoded publickey bytes.
			encodedpubkey = cert.GetPublicKey(); //140
       
			UInt32 blobbytes=0;
			// Display the value to the console.
			//Console.WriteLine();
			//showBytes("Encoded publickey", encodedpubkey);
			//Console.WriteLine();
			IntPtr pDecodePara = IntPtr.Zero;
			if(Win32.CryptDecodeObject(ENCODING_TYPE, RSA_CSP_PUBLICKEYBLOB, encodedpubkey, (UInt32)encodedpubkey.Length, 0, pDecodePara, null, ref blobbytes))
			{
				publickeyblob = new byte[blobbytes]; //148
				bool retVal = Win32.CryptDecodeObject(ENCODING_TYPE, RSA_CSP_PUBLICKEYBLOB, encodedpubkey, (UInt32)encodedpubkey.Length, 0, pDecodePara, publickeyblob, ref blobbytes);
				//if(Win32.CryptDecodeObject(ENCODING_TYPE, RSA_CSP_PUBLICKEYBLOB, encodedpubkey, (uint)encodedpubkey.Length, 0, publickeyblob, ref blobbytes))
					//showBytes("CryptoAPI publickeyblob", publickeyblob);
			}
			else
			{
				throw new Exception("Couldn't decode publickeyblob from certificate publickey");
				//Console.WriteLine("Couldn't decode publickeyblob from certificate publickey") ;
				//return;
			}
	 
			PUBKEYBLOBHEADERS pkheaders = new PUBKEYBLOBHEADERS() ;
			int headerslength = Marshal.SizeOf(pkheaders); //20
			IntPtr buffer = Marshal.AllocHGlobal( headerslength);
			Marshal.Copy( publickeyblob, 0, buffer, headerslength );
			pkheaders = (PUBKEYBLOBHEADERS) Marshal.PtrToStructure( buffer, typeof(PUBKEYBLOBHEADERS) );
            Marshal.FreeHGlobal(buffer);

			/*
			Console.WriteLine("\n ---- PUBLICKEYBLOB headers ------");
			String magicstring = (new ASCIIEncoding()).GetString(BitConverter.GetBytes(pkheaders.magic)) ;
			Console.WriteLine("  magic     0x{0:x4}     '{1}'", pkheaders.magic, magicstring);
			Console.WriteLine("  bitlen    {0}", pkheaders.bitlen);
			Console.WriteLine("  pubexp    {0}", pkheaders.pubexp);
			Console.WriteLine("  btype     {0}", pkheaders.bType);
			Console.WriteLine("  bversion  {0}", pkheaders.bVersion);
			Console.WriteLine("  reserved  {0}", pkheaders.reserved);
			Console.WriteLine("  aiKeyAlg  0x{0}", pkheaders.aiKeyAlg);
			Console.WriteLine(" --------------------------------");
			*/

			//-----  Get public exponent in big-endian byte array, suitable for RSAParameters.Exponent -------------
			exponent = BitConverter.GetBytes(pkheaders.pubexp); //returns bytes in little-endian order
			Array.Reverse(exponent, 0, exponent.Length);    //PUBLICKEYBLOB stores in LITTLE-endian order; convert to BIG-endian order
			//showBytes("\nPublic key exponent (big-endian order):", exponent);

			//-----  Get modulus in big-endian byte array, suitable for RSAParameters.Modulus -------------
			int modulusbytes = (int)pkheaders.bitlen/8 ; //128
			modulus = new byte[modulusbytes];
			try
			{
				Array.Copy(publickeyblob, headerslength, modulus, 0, modulusbytes);
				Array.Reverse(modulus, 0, modulus.Length);   //convert from little to big-endian ordering.
				//showBytes("\nPublic key modulus  (big-endian order):", modulus);
			}
			catch(Exception)
			{
				throw new Exception("Problem getting modulus from publickeyblob");
				//Console.WriteLine("Problem getting modulus from publickeyblob");
			}

			/*
			Console.Write("\nWrite public key to encoded-key and PUBLICKEYBLOB files?  [y|n]  ");
			string ans = Console.ReadLine();
			if(ans.Trim().StartsWith("Y") || ans.Trim().StartsWith("y"))
			{
				WriteKeyBlob("encodedpubkey_" + basefname,  encodedpubkey);
				WriteKeyBlob("publickeyblob_" + basefname, publickeyblob);
			}

			//-------  Try to instantiate an RSACryptoServiceProvider  ---------
			Console.WriteLine("\nTrying to instantiate RSACryptoServiceProvider with certificate file data ..");
			//Create a new instance of RSACryptoServiceProvider.
			RSACryptoServiceProvider oRSA = new RSACryptoServiceProvider();

			//Create a new instance of RSAParameters.
			RSAParameters RSAKeyInfo = new RSAParameters();

			//Set RSAKeyInfo to the public key values. 
			RSAKeyInfo.Modulus = modulus;
			RSAKeyInfo.Exponent = exponent;

			//Import key parameters into RSA.
			oRSA.ImportParameters(RSAKeyInfo);
			Console.WriteLine(" RSACryptoServiceProvider.KeySize: {0}", oRSA.KeySize);
			Console.WriteLine(" RSACryptoServiceProvider.PersistKeyInCsp: {0}", oRSA.PersistKeyInCsp);
			Console.WriteLine(" RSACryptoServiceProvider.KeyExchangeAlgorithm: {0}", oRSA.KeyExchangeAlgorithm);
			Console.WriteLine(" RSACryptoServiceProvider.SignatureAlgorithm: {0}", oRSA.SignatureAlgorithm);
			*/
		}

		/*
		public static void Main(String[] args)
		{
			X509Certificate cert=null;
			byte[] encodedpubkey = null;
			byte[] publickeyblob = null;

			if(args.Length<1)
			{
				usage();
				return;
			}
			String CERTFILE_NAME = args[0] ;
			if (!File.Exists(CERTFILE_NAME))
			{
				Console.WriteLine("File '{0}' not found.", CERTFILE_NAME);
				return;
			}

			String basefname = Path.GetFileNameWithoutExtension(CERTFILE_NAME);

			// Try loading certificate as binary DER into an X509Certificate object.
			try
			{
				cert = X509Certificate.CreateFromCertFile(CERTFILE_NAME);
			}
			catch(System.Security.Cryptography.CryptographicException) 
			{ //not binary DER; try BASE64 format
				StreamReader sr = File.OpenText(CERTFILE_NAME);
				String filestr = sr.ReadToEnd();
				sr.Close();
				StringBuilder sb = new StringBuilder(filestr) ;
				sb.Replace("-----BEGIN CERTIFICATE-----", "") ;
				sb.Replace("-----END CERTIFICATE-----", "") ;
				//Decode 
				try
				{        //see if the file is a valid Base64 encoded cert
					byte[] certBytes = Convert.FromBase64String(sb.ToString()) ;
					cert =  new X509Certificate(certBytes);
				}
				catch(System.FormatException) 
				{
					Console.WriteLine("Not valid binary DER or Base64 X509 certificate format");
					return;
				}
				catch(System.Security.Cryptography.CryptographicException) 
				{
					Console.WriteLine("Not valid binary DER or Base64 X509 certificate format");
					return;
				}
			}


			// Get the asn.1 encoded publickey bytes.
			encodedpubkey = cert.GetPublicKey();
       
			uint blobbytes=0;
			// Display the value to the console.
			Console.WriteLine();
			showBytes("Encoded publickey", encodedpubkey);
			Console.WriteLine();
			if(Win32.CryptDecodeObject(ENCODING_TYPE, RSA_CSP_PUBLICKEYBLOB, encodedpubkey, (uint)encodedpubkey.Length, 0, null, ref blobbytes))
			{
				publickeyblob = new byte[blobbytes];
				if(Win32.CryptDecodeObject(ENCODING_TYPE, RSA_CSP_PUBLICKEYBLOB, encodedpubkey, (uint)encodedpubkey.Length, 0, publickeyblob, ref blobbytes))
					showBytes("CryptoAPI publickeyblob", publickeyblob);
			}
			else
			{
				Console.WriteLine("Couldn't decode publickeyblob from certificate publickey") ;
				return;}
	 
			PUBKEYBLOBHEADERS pkheaders = new PUBKEYBLOBHEADERS() ;
			int headerslength = Marshal.SizeOf(pkheaders);
			IntPtr buffer = Marshal.AllocHGlobal( headerslength);
			Marshal.Copy( publickeyblob, 0, buffer, headerslength );
			pkheaders = (PUBKEYBLOBHEADERS) Marshal.PtrToStructure( buffer, typeof(PUBKEYBLOBHEADERS) );
			Marshal.FreeHGlobal( buffer );

			Console.WriteLine("\n ---- PUBLICKEYBLOB headers ------");
			String magicstring = (new ASCIIEncoding()).GetString(BitConverter.GetBytes(pkheaders.magic)) ;
			Console.WriteLine("  magic     0x{0:x4}     '{1}'", pkheaders.magic, magicstring);
			Console.WriteLine("  bitlen    {0}", pkheaders.bitlen);
			Console.WriteLine("  pubexp    {0}", pkheaders.pubexp);
			Console.WriteLine("  btype     {0}", pkheaders.bType);
			Console.WriteLine("  bversion  {0}", pkheaders.bVersion);
			Console.WriteLine("  reserved  {0}", pkheaders.reserved);
			Console.WriteLine("  aiKeyAlg  0x{0}", pkheaders.aiKeyAlg);
			Console.WriteLine(" --------------------------------");

			//-----  Get public exponent in big-endian byte array, suitable for RSAParameters.Exponent -------------
			byte[] exponent = BitConverter.GetBytes(pkheaders.pubexp); //returns bytes in little-endian order
			Array.Reverse(exponent);    //PUBLICKEYBLOB stores in LITTLE-endian order; convert to BIG-endian order
			showBytes("\nPublic key exponent (big-endian order):", exponent);

			//-----  Get modulus in big-endian byte array, suitable for RSAParameters.Modulus -------------
			int modulusbytes = (int)pkheaders.bitlen/8 ;
			byte[] modulus = new byte[modulusbytes];
			try
			{
				Array.Copy(publickeyblob, headerslength, modulus, 0, modulusbytes);
				Array.Reverse(modulus);   //convert from little to big-endian ordering.
				showBytes("\nPublic key modulus  (big-endian order):", modulus);
			}
			catch(Exception)
			{
				Console.WriteLine("Problem getting modulus from publickeyblob");
			}

			Console.Write("\nWrite public key to encoded-key and PUBLICKEYBLOB files?  [y|n]  ");
			string ans = Console.ReadLine();
			if(ans.Trim().StartsWith("Y") || ans.Trim().StartsWith("y"))
			{
				WriteKeyBlob("encodedpubkey_" + basefname,  encodedpubkey);
				WriteKeyBlob("publickeyblob_" + basefname, publickeyblob);
			}



			//-------  Try to instantiate an RSACryptoServiceProvider  ---------
			Console.WriteLine("\nTrying to instantiate RSACryptoServiceProvider with certificate file data ..");
			//Create a new instance of RSACryptoServiceProvider.
			RSACryptoServiceProvider oRSA = new RSACryptoServiceProvider();

			//Create a new instance of RSAParameters.
			RSAParameters RSAKeyInfo = new RSAParameters();

			//Set RSAKeyInfo to the public key values. 
			RSAKeyInfo.Modulus = modulus;
			RSAKeyInfo.Exponent = exponent;

			//Import key parameters into RSA.
			oRSA.ImportParameters(RSAKeyInfo);
			Console.WriteLine(" RSACryptoServiceProvider.KeySize: {0}", oRSA.KeySize);
			Console.WriteLine(" RSACryptoServiceProvider.PersistKeyInCsp: {0}", oRSA.PersistKeyInCsp);
			Console.WriteLine(" RSACryptoServiceProvider.KeyExchangeAlgorithm: {0}", oRSA.KeyExchangeAlgorithm);
			Console.WriteLine(" RSACryptoServiceProvider.SignatureAlgorithm: {0}", oRSA.SignatureAlgorithm);

			//------------------------------------------------------------



		}

		private static void showBytes(String info, byte[] data)
		{
			Console.WriteLine("{0}  [{1} bytes]", info, data.Length);
			for(int i=1; i<=data.Length; i++)
			{	
				Console.Write("{0:X2}  ", data[i-1]) ;
				if(i%16 == 0)
					Console.WriteLine();
			}
			Console.WriteLine();
		}
		
		private static void WriteKeyBlob(String keyblobfile, byte[] keydata) 
		{
			FileStream fs = null;
			if (File.Exists(keyblobfile)) 
			{
				Console.WriteLine("File '{0}' already exists!", keyblobfile);
				return;
			}	 
			try
			{
				fs = new FileStream(keyblobfile, FileMode.CreateNew);
				fs.Write(keydata, 0, keydata.Length);
				Console.WriteLine("Wrote public key file '{0}'", keyblobfile) ;
			}
			catch(Exception e) 
			{
				throw e;
				Console.WriteLine(e.Message) ; 
			}
			finally 
			{
				fs.Close();
			}
		}

		private static void usage() 
		{
			Console.WriteLine("\nUsage:\nDecodeCertKey.exe <X.509 certificate file>");
		}
		*/
	}
}

