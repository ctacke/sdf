using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF
{
  /// <summary>
  /// An implementation of the Fast Fourier Transform based on the work of Laurent de Soras
  /// </summary>
  public class FFTReal
  {
    private int m_length = 0;
    private int m_bitcount = 0;
    private BitReversedLUT m_brlookup = null;
    private TrigonometricLUT m_tlookup = null;
    private double[] m_data = null;

    private double sqrt2_2 = System.Math.Sqrt(2) / 2;

    public FFTReal(int dataLength)
    {
      if (dataLength <= 0) throw new ArgumentException("Invalid dataLength - must be a positive power of 2");
      if (!Math2.IsPowerOf2(dataLength)) throw new ArgumentException("Invalid dataLength - must be power of 2");

      // pre calculate the look up tables
      Prepare(dataLength);
    }

    private void Prepare(int length)
    {
      m_length = length;
      m_bitcount = (int)System.Math.Floor(System.Math.Log(length) / System.Math.Log(2) + 0.5);
      m_brlookup = new BitReversedLUT(m_bitcount);
      m_tlookup = new TrigonometricLUT(m_bitcount);

      if ((1L << m_bitcount) != length)
      {
        throw new Exception("Invalid length - must be power of 2");
      }

      // allocate our buffer
      m_data = new double[length];
    }

    private unsafe void RunFFT(double[] x, double* sf, double* df)
    {

      /* Do the transformation in several passes */
      int pass;
      long nbr_coef;
      long h_nbr_coef;
      long d_nbr_coef;
      long coef_index = 0;

      /* First and second passes at once */
      fixed (long* bit_rev_lut_ptr = m_brlookup.m_data)
      {
        do
        {
          long rev_index_0 = bit_rev_lut_ptr[coef_index];
          long rev_index_1 = bit_rev_lut_ptr[coef_index + 1];
          long rev_index_2 = bit_rev_lut_ptr[coef_index + 2];
          long rev_index_3 = bit_rev_lut_ptr[coef_index + 3];

          double* df2 = df + coef_index;
          df2[1] = x[rev_index_0] - x[rev_index_1];
          df2[3] = x[rev_index_2] - x[rev_index_3];

          double sf_0 = x[rev_index_0] + x[rev_index_1];
          double sf_2 = x[rev_index_2] + x[rev_index_3];

          df2[0] = sf_0 + sf_2;
          df2[2] = sf_0 - sf_2;

          coef_index += 4;
        } while (coef_index < m_length);

        /* Third pass */
        coef_index = 0;
        do
        {
          double v;
          sf[coef_index] = df[coef_index] + df[coef_index + 4];
          sf[coef_index + 4] = df[coef_index] - df[coef_index + 4];
          sf[coef_index + 2] = df[coef_index + 2];
          sf[coef_index + 6] = df[coef_index + 6];

          v = (df[coef_index + 5] - df[coef_index + 7]) * sqrt2_2;
          sf[coef_index + 1] = df[coef_index + 1] + v;
          sf[coef_index + 3] = df[coef_index + 1] - v;

          v = (df[coef_index + 5] + df[coef_index + 7]) * sqrt2_2;
          sf[coef_index + 5] = v + df[coef_index + 3];
          sf[coef_index + 7] = v - df[coef_index + 3];

          coef_index += 8;
        } while (coef_index < m_length);


        /* Next pass */
        for (pass = 3; pass < m_bitcount; ++pass)
        {
          coef_index = 0;
          nbr_coef = 1 << pass;
          h_nbr_coef = nbr_coef >> 1;
          d_nbr_coef = nbr_coef << 1;

          fixed (double* cos_ptr = &m_tlookup.m_data[(1L << (pass - 1)) - 4])
          {
            do
            {
              long i;
              double* sf1r = sf + coef_index;
              double* sf2r = sf1r + nbr_coef;
              double* dfr = df + coef_index;
              double* dfi = dfr + nbr_coef;

              /* Extreme coefficients are always real */
              dfr[0] = sf1r[0] + sf2r[0];
              dfi[0] = sf1r[0] - sf2r[0];	// dfr [nbr_coef] =
              dfr[h_nbr_coef] = sf1r[h_nbr_coef];
              dfi[h_nbr_coef] = sf2r[h_nbr_coef];

              /* Others are conjugate complex numbers */
              double* sf1i = sf1r + h_nbr_coef;
              double* sf2i = sf1i + nbr_coef;
              for (i = 1; i < h_nbr_coef; ++i)
              {
                double c = cos_ptr[i];					// cos (i*PI/nbr_coef);
                double s = cos_ptr[h_nbr_coef - i];	// sin (i*PI/nbr_coef);
                double v;

                v = sf2r[i] * c - sf2i[i] * s;
                dfr[i] = sf1r[i] + v;
                dfi[-i] = sf1r[i] - v;	// dfr [nbr_coef - i] =

                v = sf2r[i] * s + sf2i[i] * c;
                dfi[i] = v + sf1i[i];
                dfi[nbr_coef - i] = v - sf1i[i];
              }

              coef_index += d_nbr_coef;
            } while (coef_index < m_length);
          }

          /* Prepare to the next pass */

          double* temp_ptr = df;
          df = sf;
          sf = temp_ptr;

        }
      }
    }

    /// <summary>
    /// Compute an FFT on a 2^n-length data set
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Complex[] Compute(double[] input)
    {
      double[] output = new double[input.Length];

      Compute(output, input);

      Complex[] complex = new Complex[input.Length / 2];
      for (int c = 0; c < complex.Length; c++)
      {
        complex[c].real = output[c];
        complex[c].imaginary = output[c + complex.Length];
      }

      return complex;
    }

    private unsafe void Compute(double[] output, double[] input)
    {
      if (input.Length != m_length)
        throw new Exception("data length must match prepared length");

      if ((m_bitcount & 1) > 0)
      {
        fixed (double* df = m_data, sf = output)
        {
          RunFFT(input, sf, df);
        }
      }
      else
      {
        fixed (double* df = output, sf = m_data)
        {
          RunFFT(input, sf, df);
        }
      }
    }

    private unsafe void RunFFTI(double[] f, double* df, double* df_temp, double* x)
    {
      /* Do the transformation in several pass */
      fixed (double* psf = f)
      {
        double* sf = psf;

        int pass;
        long nbr_coef;
        long h_nbr_coef;
        long d_nbr_coef;
        long coef_index = 0;

        /* First pass */
        for (pass = m_bitcount - 1; pass >= 3; --pass)
        {
          nbr_coef = 1 << pass;
          h_nbr_coef = nbr_coef >> 1;
          d_nbr_coef = nbr_coef << 1;
          fixed (double* cos_ptr = &m_tlookup.m_data[(1L << (pass - 1)) - 4])
          {
            do
            {
              long i;
              double* sfr = sf + coef_index;
              double* sfi = sfr + nbr_coef;
              double* df1r = df + coef_index;
              double* df2r = df1r + nbr_coef;

              /* Extreme coefficients are always real */
              df1r[0] = sfr[0] + sfi[0];		// + sfr [nbr_coef]
              df2r[0] = sfr[0] - sfi[0];		// - sfr [nbr_coef]
              df1r[h_nbr_coef] = sfr[h_nbr_coef] * 2;
              df2r[h_nbr_coef] = sfi[h_nbr_coef] * 2;

              /* Others are conjugate complex numbers */
              double* df1i = df1r + h_nbr_coef;
              double* df2i = df1i + nbr_coef;
              for (i = 1; i < h_nbr_coef; ++i)
              {
                df1r[i] = sfr[i] + sfi[-i];		// + sfr [nbr_coef - i]
                df1i[i] = sfi[i] - sfi[nbr_coef - i];

                double c = cos_ptr[i];					// cos (i*PI/nbr_coef);
                double s = cos_ptr[h_nbr_coef - i];	// sin (i*PI/nbr_coef);
                double vr = sfr[i] - sfi[-i];		// - sfr [nbr_coef - i]
                double vi = sfi[i] + sfi[nbr_coef - i];

                df2r[i] = vr * c + vi * s;
                df2i[i] = vi * c - vr * s;
              }

              coef_index += d_nbr_coef;
            } while (coef_index < m_length);
          }

          /* Prepare to the next pass */
          if (pass < m_bitcount - 1)
          {
            double* temp_ptr = df;
            df = sf;
            sf = temp_ptr;
          }
          else
          {
            sf = df;
            df = df_temp;
          }


          /* Antepenultimate pass */
          coef_index = 0;
          do
          {
            df[coef_index] = sf[coef_index] + sf[coef_index + 4];
            df[coef_index + 4] = sf[coef_index] - sf[coef_index + 4];
            df[coef_index + 2] = sf[coef_index + 2] * 2;
            df[coef_index + 6] = sf[coef_index + 6] * 2;

            df[coef_index + 1] = sf[coef_index + 1] + sf[coef_index + 3];
            df[coef_index + 3] = sf[coef_index + 5] - sf[coef_index + 7];

            double vr = sf[coef_index + 1] - sf[coef_index + 3];
            double vi = sf[coef_index + 5] + sf[coef_index + 7];

            df[coef_index + 5] = (vr + vi) * sqrt2_2;
            df[coef_index + 7] = (vi - vr) * sqrt2_2;

            coef_index += 8;
          }
          while (coef_index < m_length);


          /* Penultimate and last pass at once */
          coef_index = 0;
          fixed (long* bit_rev_lut_ptr = m_brlookup.m_data)
          {
            long* lut = bit_rev_lut_ptr;
            double* sf2 = df;
            do
            {
              double b_0 = sf2[0] + sf2[2];
              double b_2 = sf2[0] - sf2[2];
              double b_1 = sf2[1] * 2;
              double b_3 = sf2[3] * 2;

              x[lut[0]] = b_0 + b_1;
              x[lut[1]] = b_0 - b_1;
              x[lut[2]] = b_2 + b_3;
              x[lut[3]] = b_2 - b_3;

              b_0 = sf2[4] + sf2[6];
              b_2 = sf2[4] - sf2[6];
              b_1 = sf2[5] * 2;
              b_3 = sf2[7] * 2;

              x[lut[4]] = b_0 + b_1;
              x[lut[5]] = b_0 - b_1;
              x[lut[6]] = b_2 + b_3;
              x[lut[7]] = b_2 - b_3;

              sf2 += 8;
              coef_index += 8;
              lut += 8;

            } while (coef_index < m_length);
          }
        }
      }
    }

    /// <summary>
    /// Compute an inverse FFT on a 2^n-length data set of complex numbers
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public double[] ComputeInverse(Complex[] input)
    {
      double[] output = new double[input.Length * 2];
      double[] indata = new double[input.Length * 2];

      for (int c = 0; c < input.Length; c++)
      {
        indata[c * 2] = input[c].real;
        indata[(c * 2) + 1] = input[c].imaginary;
      }

      ComputeInverse(indata, output);

      return output;
    }

    private unsafe void ComputeInverse(double[] input, double[] output)
    {
      if (input.Length != m_length)
        throw new Exception("data length must match prepared length");

      if ((m_bitcount & 1) > 0)
      {
        fixed (double* df = m_data, dftemp = output, px = output)
        {
          RunFFTI(input, df, dftemp, px);
        }
      }
      else
      {
        fixed (double* dftemp = m_data, df = output, px = output)
        {
          RunFFTI(input, df, dftemp, px);
        }
      }

    }
  } // end class FFTReal
}
