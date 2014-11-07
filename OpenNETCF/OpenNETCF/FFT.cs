using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF
{
  public static class FFT
  {
    private static int ReverseBits(int index, int NumBits)
    {
      int i;
      int rev;

      for (i = rev = 0; i < NumBits; i++)
      {
        rev = (rev << 1) | (index & 1);
        index >>= 1;
      }

      return (int)rev;
    }

    private static int NumberOfBitsNeeded(int PowerOfTwo)
    {
      for (int i = 0; ; i++)
      {
        if ((PowerOfTwo & (1 << i)) > 0)
          return i;
      }
    }

    public static unsafe Complex[] Calculate(Complex[] inData, bool Inverse)
    {
      if (inData == null) throw new ArgumentNullException("inData");
      if (inData.Length == 0) throw new ArgumentException("inData cannot be zero length");

      int NumSamples = (int)inData.Length;
      Complex[] outData = new Complex[NumSamples];

      int NumBits = 0;
      int i;
      int j;
      int k;
      int n;
      int BlockSize;
      int BlockEnd;

      double angle_numerator = 2.0 * System.Math.PI;
      double tr, ti;     // temp real, temp imaginary

      // check power of 2
      // check we have >2 data points

      if (Inverse)
        angle_numerator = angle_numerator * -1;

      NumBits = NumberOfBitsNeeded(NumSamples);

      /*
      **   Do simultaneous data copy and bit-reversal ordering into outputs...
      */

      for (i = 0; i < NumSamples; i++)
      {
        j = ReverseBits(i, NumBits);
        outData[j].real = inData[i].real;
        outData[j].imaginary = inData[i].imaginary;
      }

      /*
      **   Do the FFT itself...
      */

      BlockEnd = 1;
      double[] ar = new double[3];
      double[] ai = new double[3];

      for (BlockSize = 2; BlockSize <= NumSamples; BlockSize <<= 1)
      {
        double delta_angle = angle_numerator / (double)BlockSize;
        double sm2 = System.Math.Sin(-2 * delta_angle);
        double sm1 = System.Math.Sin(-delta_angle);
        double cm2 = System.Math.Cos(-2 * delta_angle);
        double cm1 = System.Math.Cos(-delta_angle);
        double w = 2 * cm1;
        // double temp;

        for (i = 0; i < NumSamples; i += BlockSize)
        {
          ar[2] = cm2;
          ar[1] = cm1;

          ai[2] = sm2;
          ai[1] = sm1;

          for (j = i, n = 0; n < BlockEnd; j++, n++)
          {
            ar[0] = w * ar[1] - ar[2];
            ar[2] = ar[1];
            ar[1] = ar[0];

            ai[0] = w * ai[1] - ai[2];
            ai[2] = ai[1];
            ai[1] = ai[0];

            k = j + BlockEnd;
            tr = ar[0] * outData[k].real - ai[0] * outData[k].imaginary;
            ti = ar[0] * outData[k].imaginary + ai[0] * outData[k].real;

            outData[k].real = outData[j].real - tr;
            outData[k].imaginary = outData[j].imaginary - ti;

            outData[j].real += tr;
            outData[j].imaginary += ti;
          }
        }

        BlockEnd = BlockSize;
      }


      /*
      **   Need to normalize if inverse transform...
      */

      if (Inverse)
      {
        double denom = (double)NumSamples;

        for (i = 0; i < NumSamples; i++)
        {
          outData[i].real /= denom;
          outData[i].imaginary /= denom;
        }
      }
      return outData;
    }
  } // end class FFT
}
