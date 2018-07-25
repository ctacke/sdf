using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace OpenNETCF.Media.WaveAudio
{
    public delegate int ACMDRIVERENUMCB(IntPtr hadid, int dwInstance, int fdwSupport );
    public delegate int ACMFORMATENUMCB(IntPtr hadid, ref ACMFORMATDETAILS afd, int dwInstance, int fdwSupport);
    public delegate int ACMFORMATTAGENUMCB(IntPtr hadid, /*ref ACMFORMATTAGDETAILS*/ IntPtr afd, int dwInstance, int fdwSupport);

    public class ACMSupport
    {
        private static List<AcmDriverInfo> _listDetails;
        public static AcmDriverInfo[] SupportedDrivers
        {
            get
            {
                if (_listDetails == null)
                {
                    _listDetails = new List<AcmDriverInfo>();
                    AcmNativeMethods.acmDriverEnum(AcmDriverEnumFunc, 0, 0);
                }
                return _listDetails.ToArray();
            }
        }

        private static int AcmDriverEnumFunc(IntPtr hadid, int dwInstance, int fdwSupport)
        {
            ACMDRIVERDETAILS details = new ACMDRIVERDETAILS();
            details.cbStruct = Marshal.SizeOf(details);
            AcmNativeMethods.acmDriverDetails(hadid, ref details, 0);
            _listDetails.Add(AcmDriverInfo.FromDriverDetails(details, hadid.ToInt32()));
            return 1;
        }
    }

    public struct AcmFormatInfo
    {
        public int DriverId;
        public WaveFormat2 Format;
        public string FormatName;
        public FormatTag FormatTag;

        internal static AcmFormatInfo FromFormatTagDetails(ACMFORMATTAGDETAILS details, IntPtr hDriver, int driverId)
        {
            if (driverId == 0)
                AcmNativeMethods.acmDriverID(hDriver, out driverId, 0);
            AcmFormatInfo info = new AcmFormatInfo();
            info.DriverId = driverId;
            //info.Format = new WaveFormat2(
            info.FormatName = details.szFormatTag;
            info.FormatTag = (FormatTag)details.dwFormatTag;

            for (int i = 0; i < details.cStandardFormats; i++)
            {
                ACMFORMATDETAILS fd = new ACMFORMATDETAILS();
                fd.cbStruct = Marshal.SizeOf(fd);
                fd.cbwfx = details.cbFormatSize;
                WaveFormat2 fmt = new WaveFormat2(new byte[fd.cbwfx]);
                fmt.FormatTag = (FormatTag)details.dwFormatTag;
                fmt.Size = (short)(fd.cbwfx == 16 ? 0 : fd.cbwfx - 18);
                fmt.Extra = new byte[fmt.Size];
                fd.pwfx = Marshal.AllocHGlobal(fd.cbwfx);
                Marshal.Copy(fmt.GetBytes(), 0, fd.pwfx, fd.cbwfx);
                fd.dwFormatIndex = i;
                fd.dwFormatTag = (int)info.FormatTag;
                /*MMResult mmr = */
                int mmr = AcmNativeMethods.acmFormatDetails(hDriver, ref fd, AcmFormatDetailsFlags.INDEX);
                byte[] fmtData = new byte[fd.cbwfx];
                Marshal.Copy(fd.pwfx, fmtData, 0, fd.cbwfx);
                info.Format = new WaveFormat2(fmtData);
                Marshal.FreeHGlobal(fd.pwfx);
            }
            return info;
        }

    }

    public struct AcmDriverInfo
    {
        public int DriverId;
        public ACMDriverDetailsSupportFlags Flags;
        public string ShortName;
        public string LongName;
        public string Description;
        private List<AcmFormatInfo> _formats;

        public AcmFormatInfo[] Formats { get { return _formats.ToArray(); } }

        internal static AcmDriverInfo FromDriverDetails(ACMDRIVERDETAILS details, int driverId)
        {
            AcmDriverInfo info = new AcmDriverInfo();
            info.DriverId = driverId;
            info.Flags = details.fdwSupport;
            info.ShortName = details.szShortName;
            info.LongName = details.szLongName;
            info.Description = details.szFeatures;
            info._formats = new List<AcmFormatInfo>();
            IntPtr hDrv;
            AcmNativeMethods.acmDriverOpen(out hDrv, driverId, 0);
            ACMFORMATTAGDETAILS afdt = new ACMFORMATTAGDETAILS();
            afdt.cbStruct = Marshal.SizeOf(afdt);
            IntPtr pafd = Marshal.AllocHGlobal(afdt.cbStruct);
            Marshal.StructureToPtr(afdt, pafd, false);
            AcmNativeMethods.acmFormatTagEnum(hDrv, /*ref afdt*/pafd, info.AcmFormatTagEnumCB, (int)hDrv, 0);
            Marshal.FreeHGlobal(pafd);
            AcmNativeMethods.acmDriverClose(hDrv, 0);
            return info;
        }

        internal int AcmFormatTagEnumCB(IntPtr hadid, /*ref ACMFORMATTAGDETAILS*/ IntPtr pafd, int dwInstance, int fdwSupport)
        {
            try
            {
                ACMFORMATTAGDETAILS afd = (ACMFORMATTAGDETAILS)Marshal.PtrToStructure(pafd, typeof(ACMFORMATTAGDETAILS));
                AcmFormatInfo info = AcmFormatInfo.FromFormatTagDetails(afd, (IntPtr)dwInstance, (int)hadid);
                _formats.Add(info);
            }
            catch(Exception ex) 
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
                System.Diagnostics.Debug.WriteLine("AcmFormatTagEnumCB error: " + ex.Message);
            }
            return 1;
        }
    }

    [Flags]
    public enum ACMDriverDetailsSupportFlags
    {
        CODEC = 0x00000001,
        CONVERTER = 0x00000002,
        FILTER = 0x00000004,
        HARDWARE = 0x00000008,
        ASYNC = 0x00000010,
        LOCAL = 0x40000000,
        DISABLED = -0x7fffffff,
    }

    public enum AcmFormatDetailsFlags
    {
        INDEX = 0x00000000,
        FORMAT = 0x00000001,
        QUERYMASK = 0x0000000F,
    }

    public enum AcmStreamSizeFlags
    {
        SOURCE = 0x00000000,
        DESTINATION = 0x00000001,
        QUERYMASK = 0x0000000F,
    }

    [Flags]
    public enum AcmStreamOpenFlags
    {
        QUERY = 0x00000001,
        ASYNC = 0x00000002,
        NONREALTIME = 0x00000004,
        CALLBACK_TYPEMASK = 0x00070000,    /* callback type mask */
        CALLBACK_NULL = 0x00000000,    /* no callback */
        CALLBACK_WINDOW = 0x00010000,    /* dwCallback is a HWND */
        CALLBACK_TASK = 0x00020000,    /* dwCallback is a HTASK */
        CALLBACK_FUNCTION = 0x00030000,    /* dwCallback is a FARPROC */
        CALLBACK_THREAD = (CALLBACK_TASK),/* thread ID replaces 16 bit task */
        CALLBACK_EVENT = 0x00050000,    /* dwCallback is an EVENT Handle */
        CALLBACK_MSGQUEUE = 0x00060000,    /* dwCallback is HANDLE returned by CreateMsgQueue or OpenMsgQueue (new in Windows CE 5.0) */
    }

    [Flags]
    public enum AcmStreamConvertFlags
    {
        BLOCKALIGN = 0x00000004,
        START = 0x00000010,
        END = 0x00000020,
    }

    [Flags]
    public enum AcmStreamHeaderStatus
    {
        DONE = 0x00010000,
        PREPARED = 0x00020000,
        INQUEUE = 0x00100000,
    }

    [Flags]
    public enum AcmFormatSuggestFlags
    {
        WFORMATTAG = 0x00010000,
        NCHANNELS = 0x00020000,
        NSAMPLESPERSEC = 0x00040000,
        WBITSPERSAMPLE = 0x00080000,

        TYPEMASK = 0x00FF0000,
    }
    public enum MM
    {
        STREAM_OPEN = 0x3D4,
        STREAM_CLOSE = 0x3D5,
        STREAM_DONE = 0x3D6,
        STREAM_ERROR = 0x3D7,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ACMDRIVERDETAILS
    {
        const int ACMDRIVERDETAILS_SHORTNAME_CHARS = 32;
        const int ACMDRIVERDETAILS_LONGNAME_CHARS = 128;
        const int ACMDRIVERDETAILS_COPYRIGHT_CHARS = 80;
        const int ACMDRIVERDETAILS_LICENSING_CHARS = 128;
        const int ACMDRIVERDETAILS_FEATURES_CHARS = 512;

        public int cbStruct;
        public FourCC fccType;
        public FourCC fccComp;
        public short wMid;
        public short wPid;
        public int vdwACM;
        public int vdwDriver;
        public ACMDriverDetailsSupportFlags fdwSupport;
        public int cFormatTags;
        public int cFilterTags;
        int hicon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ACMDRIVERDETAILS_SHORTNAME_CHARS)]
        public string szShortName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ACMDRIVERDETAILS_LONGNAME_CHARS)]
        public string szLongName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ACMDRIVERDETAILS_COPYRIGHT_CHARS)]
        public string szCopyright;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ACMDRIVERDETAILS_LICENSING_CHARS)]
        public string szLicensing;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ACMDRIVERDETAILS_FEATURES_CHARS)]
        public string szFeatures;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ACMFORMATDETAILS
    {
        const int ACMFORMATDETAILS_FORMAT_CHARS = 128;
        public int cbStruct;
        public int dwFormatIndex;
        public int dwFormatTag;
        public ACMDriverDetailsSupportFlags fdwSupport;
        public IntPtr pwfx;
        public int cbwfx;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ACMFORMATDETAILS_FORMAT_CHARS)]
        public string szFormat;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ACMFORMATTAGDETAILS
    {
        const int ACMFORMATTAGDETAILS_FORMATTAG_CHARS = 48;
        public int cbStruct;
        public int dwFormatTagIndex;
        public int dwFormatTag;
        public int cbFormatSize;
        public int fdwSupport;
        public int cStandardFormats;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ACMFORMATTAGDETAILS_FORMATTAG_CHARS)]
        public string szFormatTag;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ACMSTREAMHEADER
    {
        public int cbStruct;
        public AcmStreamHeaderStatus fdwStatus;
        public int dwUser;
        public IntPtr pbSrc;
        public int cbSrcLength;
        public int cbSrcLengthUsed;
        public int dwSrcUser;
        public IntPtr pbDst;
        public int cbDstLength;
        public int cbDstLengthUsed;
        public int dwDstUser;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        int[] dwReservedDriver;
    }
}
