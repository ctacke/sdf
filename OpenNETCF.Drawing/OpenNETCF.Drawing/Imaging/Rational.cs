using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Drawing.Imaging
{
    /// <summary>
    /// Holds a Rational number used by imaging properties
    /// </summary>
    [CLSCompliant(false)]
    public struct Rational
    {
        [CLSCompliant(false)]
        public Rational(uint num1, uint num2)
        {
            Numerator = num1;
            Denominator = num2;
        }

        public Rational(uint val)
        {
            Numerator = val;
            Denominator = 1;
        }

        public uint Numerator;
        public uint Denominator;

        public override int GetHashCode()
        {
            return (int)(Numerator ^ Denominator);
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", Numerator, Denominator);
        }
    }
    /// <summary>
    /// Holds a Signed Rational number used by imaging properties
    /// </summary>
    [CLSCompliant(false)]
    public struct SRational
    {
        public SRational(int num1, uint num2)
        {
            Numerator = num1;
            Denominator = num2;
        }

        public SRational(int val)
        {
            Numerator = val;
            Denominator = 1;
        }

        public int Numerator;
        public uint Denominator;

        public override int GetHashCode()
        {
            return (int)(Numerator ^ Denominator);
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", Numerator, Denominator);
        }
    }
}
