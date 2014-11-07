using System;

namespace OpenNETCF.Security.Cryptography
{
    /// <summary>
    /// Represents the abstract base class from which all classes that derive byte sequences of a specified length inherit.
    /// </summary>
	public abstract class DeriveBytes
	{
        /// <summary>
        /// Initializes a new instance of the DeriveBytes class.
        /// </summary>
		protected DeriveBytes(){}
        /// <summary>
        /// When overridden in a derived class, returns pseudo-random key bytes.
        /// </summary>
        /// <param name="cb">The number of pseudo-random key bytes to generate</param>
        /// <returns>A byte array filled with pseudo-random key bytes.</returns>
		public abstract byte[] GetBytes(int cb);

        /// <summary>
        /// When overridden in a derived class, resets the state of the operation.
        /// </summary>
		public abstract void Reset();
	}
}