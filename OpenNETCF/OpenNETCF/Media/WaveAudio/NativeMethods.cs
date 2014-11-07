using System;
using OpenNETCF.Win32;
using System.Runtime.InteropServices;

namespace OpenNETCF.Media.WaveAudio
{
    internal static class NativeMethods
    {
        [DllImport("coredll.dll", EntryPoint = "MessageBeep", SetLastError = true)]
        internal static extern void MessageBeep(int type);

        [DllImport("coredll.dll", EntryPoint = "PlaySoundW", SetLastError = true)]
        internal extern static bool PlaySound(string lpszName, IntPtr hModule, SoundFlags dwFlags);

        [DllImport("coredll.dll", EntryPoint = "PlaySoundW", SetLastError = true)]
        internal extern static bool PlaySound(IntPtr lpszName, IntPtr hModule, SoundFlags dwFlags);

        #region Sound Flags
        // PlaySound flags
        [Flags()]
        internal enum SoundFlags : int
        {
            /// <summary>
            /// <b>name</b> is a WIN.INI [sounds] entry
            /// </summary>
            Alias = 0x00010000,
            /// <summary>
            /// <b>name</b> is a file name
            /// </summary>
            FileName = 0x00020000,
            /// <summary>
            /// <b>name</b> is a resource name or atom
            /// </summary>   
            Resource = 0x00040004,
            /// <summary>   
            /// Play synchronously (default)   
            /// </summary>   
            Sync = 0x00000000,
            /// <summary>   
            ///  Play asynchronously   
            /// </summary>   
            Async = 0x00000001,
            /// <summary>   
            /// Silence not default, if sound not found   
            /// </summary>   
            NoDefault = 0x00000002,
            /// <summary>   
            /// <b>name</b> points to a memory file   
            /// </summary>   
            Memory = 0x00000004,
            /// <summary>   
            /// Loop the sound until next sndPlaySound   
            /// </summary>   
            Loop = 0x00000008,
            /// <summary>   
            /// Don't stop any currently playing sound   
            /// </summary>   
            NoStop = 0x00000010,
            /// <summary>   
            /// Don't wait if the driver is busy   
            /// </summary>   
            NoWait = 0x00002000
        }
        #endregion
        [DllImport("coredll.dll", EntryPoint = "waveOutGetNumDevs", SetLastError = true)]
        public static extern int waveOutGetNumDevs();

        [DllImport("coredll.dll", EntryPoint = "waveInGetNumDevs", SetLastError = true)]
        public static extern int waveInGetNumDevs();

        [DllImport("coredll.dll", EntryPoint = "waveOutOpen", SetLastError = true)]
        public static extern int waveOutOpen(out IntPtr t, int id, byte[] pwfx, IntPtr dwCallback, int dwInstance, int fdwOpen);

        [DllImport("coredll.dll", EntryPoint = "waveOutGetVolume", SetLastError = true)]
        public static extern int waveOutGetVolume(IntPtr hwo, ref int pdwVolume);

        [DllImport("coredll.dll", EntryPoint = "waveOutSetVolume", SetLastError = true)]
        public static extern int waveOutSetVolume(IntPtr hwo, int dwVolume);

        [DllImport("coredll.dll", EntryPoint = "waveOutPrepareHeader", SetLastError = true)]
        public static extern int waveOutPrepareHeader(IntPtr hwo, byte[] pwh, int cbwh);
        [DllImport("coredll.dll", EntryPoint = "waveOutPrepareHeader", SetLastError = true)]
        public static extern int waveOutPrepareHeader(IntPtr hwo, IntPtr lpHdr, int cbwh);

        [DllImport("coredll.dll", EntryPoint = "waveOutWrite", SetLastError = true)]
        public static extern int waveOutWrite(IntPtr hwo, byte[] pwh, int cbwh);
        [DllImport("coredll.dll", EntryPoint = "waveOutWrite", SetLastError = true)]
        public static extern int waveOutWrite(IntPtr hwo, IntPtr lpHdr, int cbwh);

        [DllImport("coredll.dll", EntryPoint = "waveOutUnprepareHeader", SetLastError = true)]
        public static extern int waveOutUnprepareHeader(IntPtr hwo, byte[] pwh, int cbwh);
        [DllImport("coredll.dll", EntryPoint = "waveOutUnprepareHeader", SetLastError = true)]
        public static extern int waveOutUnprepareHeader(IntPtr hwo, IntPtr lpHdr, int cbwh);

        [DllImport("coredll.dll", EntryPoint = "waveOutClose", SetLastError = true)]
        public static extern int waveOutClose(IntPtr hwo);

        [DllImport("coredll.dll", EntryPoint = "waveOutPause", SetLastError = true)]
        public static extern int waveOutPause(IntPtr hwo);

        [DllImport("coredll.dll", EntryPoint = "waveOutRestart", SetLastError = true)]
        public static extern int waveOutRestart(IntPtr hwo);

        [DllImport("coredll.dll", EntryPoint = "waveOutReset", SetLastError = true)]
        public static extern int waveOutReset(IntPtr hwo);

        [DllImport("coredll.dll", EntryPoint = "waveInOpen", SetLastError = true)]
        internal static extern int waveInOpen(out IntPtr t, uint id, byte[] pwfx, IntPtr dwCallback, int dwInstance, int fdwOpen);

        [DllImport("coredll.dll", EntryPoint = "waveInClose", SetLastError = true)]
        public static extern int waveInClose(IntPtr hDev);

        [DllImport("coredll.dll", EntryPoint = "waveInPrepareHeader", SetLastError = true)]
        public static extern int waveInPrepareHeader(IntPtr hwi, byte[] pwh, int cbwh);
        [DllImport("coredll.dll", EntryPoint = "waveInPrepareHeader", SetLastError = true)]
        public static extern int waveInPrepareHeader(IntPtr hwi, IntPtr lpHdr, int cbwh);

        [DllImport("coredll.dll", EntryPoint = "waveInStart", SetLastError = true)]
        public static extern int waveInStart(IntPtr hwi);

        [DllImport("coredll.dll", EntryPoint = "waveInUnprepareHeader", SetLastError = true)]
        public static extern int waveInUnprepareHeader(IntPtr hwi, byte[] pwh, int cbwh);
        [DllImport("coredll.dll", EntryPoint = "waveInUnprepareHeader", SetLastError = true)]
        public static extern int waveInUnprepareHeader(IntPtr hwi, IntPtr lpHdr, int cbwh);

        [DllImport("coredll.dll", EntryPoint = "waveInStop", SetLastError = true)]
        public static extern int waveInStop(IntPtr hwi);

        [DllImport("coredll.dll", EntryPoint = "waveInReset", SetLastError = true)]
        public static extern int waveInReset(IntPtr hwi);

        [DllImport("coredll.dll", EntryPoint = "waveInGetDevCaps", SetLastError = true)]
        public static extern int waveInGetDevCaps(int uDeviceID, byte[] pwic, int cbwic);

        [DllImport("coredll.dll", EntryPoint = "waveInAddBuffer", SetLastError = true)]
        public static extern int waveInAddBuffer(IntPtr hwi, byte[] pwh, int cbwh);
        [DllImport("coredll.dll", EntryPoint = "waveInAddBuffer", SetLastError = true)]
        public static extern int waveInAddBuffer(IntPtr hwi, IntPtr lpHdr, int cbwh);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int waveOutSetPlaybackRate(IntPtr hwo, uint dwRate);
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int waveOutSetPitch(IntPtr hwo, uint dwPitch);
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int waveOutGetPlaybackRate(IntPtr hwo, out uint dwRate);
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int waveOutGetPitch(IntPtr hwo, out uint dwPitch);

    }
}
