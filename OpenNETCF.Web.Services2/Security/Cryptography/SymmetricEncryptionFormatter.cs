using System;
using System.Security.Cryptography;
using System.Text;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class SymmetricEncryptionFormatter: EncryptionFormatter
    {
        private SymmetricAlgorithm _key;

        public abstract byte[] KeyBytes { get; set; }

        public virtual SymmetricAlgorithm Key
        {
            get { return _key; }
            set { _key = value; }
        }


        protected SymmetricEncryptionFormatter()
        {
        }

        protected SymmetricEncryptionFormatter(SymmetricAlgorithm key)
        {
            this.Key = key;
        }

        public override byte[] Encrypt(byte[] plainText)
        {
            if (plainText == null)
            {
                throw new ArgumentNullException("plainText");
            }
            if (_key == null)
            {
                throw new InvalidOperationException("No cryptographic _keyorithm was available");
            }
            _key.Mode = CipherMode.CBC;
            _key.Padding = PaddingMode.None;
            _key.GenerateIV();
            byte[] ivBuffer = _key.IV;
            int blockSizeBytes = _key.BlockSize >> 3;
            int paddingSize = blockSizeBytes - (plainText.Length % blockSizeBytes);
            byte[] encodingBuffer = new byte[plainText.Length + paddingSize];
            Array.Copy(plainText, 0, encodingBuffer, 0, plainText.Length);
            for (int i = plainText.Length; i < encodingBuffer.Length; i++)
            {
                encodingBuffer[i] = (byte)paddingSize;
            }
            ICryptoTransform transform = _key.CreateEncryptor(_key.Key, ivBuffer);
            int ivLength = ivBuffer.Length;
            int outBufLength = (encodingBuffer.Length / transform.InputBlockSize) * transform.OutputBlockSize;
            byte[] outputBuffer = new byte[ivLength + outBufLength];
            Array.Copy(ivBuffer, 0, outputBuffer, 0, ivLength);
            int cb = 0;
            while (cb < (encodingBuffer.Length - blockSizeBytes))
            {
                transform.TransformBlock(encodingBuffer, cb, blockSizeBytes, outputBuffer, ivLength + cb);
                cb += blockSizeBytes;
            }
            byte[] finalBuffer = transform.TransformFinalBlock(encodingBuffer, cb, blockSizeBytes);
            Array.Copy(finalBuffer, 0, outputBuffer, ivLength + cb, finalBuffer.Length);
            return outputBuffer;
        }

        public override byte[] Decrypt(byte[] cipherText)
        {
            if (cipherText == null)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (_key == null)
            {
                throw new InvalidOperationException("No cryptographic _keyorithm was available");
            }
            _key.Mode = CipherMode.CBC;
            _key.Padding = PaddingMode.None;
            int blockSizeBytes = _key.BlockSize >> 3;
            byte[] inputBuffer = new byte[blockSizeBytes];
            Array.Copy(cipherText, 0, inputBuffer, 0, blockSizeBytes);
            ICryptoTransform transform = _key.CreateDecryptor(_key.Key, inputBuffer);
            byte[] outputBuffer = new byte[((cipherText.Length - blockSizeBytes) / transform.InputBlockSize) * transform.OutputBlockSize];
            int cb = blockSizeBytes;
            while (cb < (cipherText.Length - blockSizeBytes))
            {
                transform.TransformBlock(cipherText, cb, blockSizeBytes, outputBuffer, cb - blockSizeBytes);
                cb += blockSizeBytes;
            }
            byte[] finalBuffer = transform.TransformFinalBlock(cipherText, cb, blockSizeBytes);
            Array.Copy(finalBuffer, 0, outputBuffer, cb - blockSizeBytes, finalBuffer.Length);
            int dataSize = outputBuffer[outputBuffer.Length - 1];
            if (outputBuffer.Length < dataSize)
            {
                throw new CryptographicException("Invalid data size");
            }
            if (dataSize > blockSizeBytes)
            {
                throw new CryptographicException("Invalid data size");
            }
            byte[] decodedBuffer = new byte[outputBuffer.Length - dataSize];
            Array.Copy(outputBuffer, 0, decodedBuffer, 0, outputBuffer.Length - dataSize);
            return decodedBuffer;
        }


    }
}
