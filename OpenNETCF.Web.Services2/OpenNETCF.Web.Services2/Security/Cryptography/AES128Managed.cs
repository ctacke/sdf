using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public class AES128Managed: SymmetricAlgorithm
    {
        private RijndaelManaged _rm;
        public AES128Managed()
        {
            _rm = new RijndaelManaged();
            //_rm.Padding = PaddingMode.None;
            _rm.KeySize = 0x80;
        }

        public static implicit operator Rijndael(AES128Managed aes)
        {
            return aes._rm;
        }

        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return _rm.CreateDecryptor(rgbKey, rgbIV);
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return _rm.CreateEncryptor(rgbKey, rgbIV);
        }

        public override void GenerateIV()
        {
            _rm.GenerateIV();
        }

        public override void GenerateKey()
        {
            _rm.GenerateKey();
        }

        protected override void Dispose(bool disposing)
        {
        }

        public override KeySizes[] LegalKeySizes
        {
            get
            {
                return _rm.LegalKeySizes;
            }
        }

        public override int KeySize
        {
            get
            {
                return 0x80;
            }
        }

        public override int BlockSize
        {
            get
            {
                return _rm.BlockSize;
            }
            set
            {
                _rm.BlockSize = value;
            }
        }

        public override ICryptoTransform CreateDecryptor()
        {
            return CreateDecryptor(Key, IV);
        }

        public override ICryptoTransform CreateEncryptor()
        {
            return CreateEncryptor(Key, IV);
        }

        public override int FeedbackSize
        {
            get
            {
                return _rm.FeedbackSize;
            }
            set
            {
                _rm.FeedbackSize = value;
            }
        }

        public override byte[] IV
        {
            get
            {
                return _rm.IV;
            }
            set
            {
                _rm.IV = value;
            }
        }

        public override byte[] Key
        {
            get
            {
                return _rm.Key;
            }
            set
            {
                _rm.Key = value;
            }
        }

        public override KeySizes[] LegalBlockSizes
        {
            get
            {
                return _rm.LegalBlockSizes;
            }
        }

        public override CipherMode Mode
        {
            get
            {
                return _rm.Mode;
            }
            set
            {
                _rm.Mode = value;
            }
        }

        public override PaddingMode Padding
        {
            get
            {
                return _rm.Padding;
            }
            set
            {
                _rm.Padding = value;
            }
        }
    }
}
