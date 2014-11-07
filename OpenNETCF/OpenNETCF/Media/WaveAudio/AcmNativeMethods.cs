using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Media.WaveAudio
{
    public delegate void acmStreamConvertCallback(IntPtr has, MM uMsg, int dwInstance, int lParam1, int lParam2);

    public class AcmNativeMethods
    {
        [DllImport("coredll")]
        extern static public int acmDriverOpen(out IntPtr phad, int hadid, int fdwOpen);
        [DllImport("coredll")]
        extern static public int acmDriverClose(IntPtr had, int fdwClose);
        [DllImport("coredll")]
        extern static public int acmDriverEnum(ACMDRIVERENUMCB fnCallback, int dwInstance, int fdwEnum);
        [DllImport("coredll")]
        extern static public int acmFormatEnum(IntPtr had, ref ACMFORMATDETAILS pafd, ACMFORMATENUMCB fnCallback, int dwInstance, int fdwEnum);
        [DllImport("coredll")]
        extern static public int acmDriverDetails(IntPtr hadid, ref ACMDRIVERDETAILS details, int fdwDetails);
        [DllImport("coredll")]
        extern static public int acmFormatTagEnum(IntPtr had, /*ref ACMFORMATTAGDETAILS*/ IntPtr paftd, ACMFORMATTAGENUMCB fnCallback, int dwInstance, int fdwEnum);
        [DllImport("coredll")]
        extern static public int acmFormatDetails(IntPtr had, ref ACMFORMATDETAILS pafd, AcmFormatDetailsFlags fdwDetails);
        [DllImport("coredll")]
        extern static public int acmFormatDetails(IntPtr had, IntPtr pafd, AcmFormatDetailsFlags fdwDetails);
        [DllImport("coredll")]
        extern static public int acmStreamOpen(out IntPtr phas, IntPtr had, byte[] pwfxSrc, byte[] pwfxDst, IntPtr pwfltr, int dwCallback, int dwInstance, AcmStreamOpenFlags fdwOpen);
        [DllImport("coredll")]
        extern static public int acmStreamClose(IntPtr has, int fdwClose);
        [DllImport("coredll")]
        extern static public int acmStreamSize(IntPtr has, int cbInput, out int pdwOutputBytes, AcmStreamSizeFlags fdwSize);
        [DllImport("coredll")]
        extern static public int acmStreamConvert(IntPtr has, ref ACMSTREAMHEADER pash, AcmStreamConvertFlags fdwConvert);
        [DllImport("coredll")]
        extern static public int acmStreamPrepareHeader(IntPtr has, ref ACMSTREAMHEADER pash, int fdwPrepare);
        [DllImport("coredll")]
        extern static public int acmStreamUnprepareHeader(IntPtr has, ref ACMSTREAMHEADER pash, int fdwUnprepare);
        [DllImport("coredll")]
        extern static public int acmFormatSuggest(IntPtr had, byte[] pwfxSrc, byte[] pwfxDst, int cbwfxDst, AcmFormatSuggestFlags fdwSuggest);
        [DllImport("coredll")]
        extern static public int acmDriverID(IntPtr hao, out int phadid, int fdwDriverID);
    }
}
