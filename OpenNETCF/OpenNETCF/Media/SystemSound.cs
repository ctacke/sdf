using System;
using OpenNETCF.Media.WaveAudio;

namespace OpenNETCF.Media
{
    /// <summary>
    /// Represents a standard system sound.
    /// </summary>
    public sealed class SystemSound
    {
        //type of sound
        private int mSoundType;

        internal SystemSound(int soundType)
        {
            //set type
            mSoundType = soundType;
        }

        /// <summary>
        /// Play this sound.
        /// </summary>
        public void Play()
        {
            //play
            OpenNETCF.Media.WaveAudio.NativeMethods.MessageBeep(mSoundType);
        }

        /// <summary>
        /// Set the volume for the default waveOut device (device ID = 0)
        /// </summary>
        /// <param name="Volume"></param>
        public static void SetVolume(int Volume)
        {
            WaveFormat2 format = new WaveFormat2();
            IntPtr hWaveOut = IntPtr.Zero;

            OpenNETCF.Media.WaveAudio.NativeMethods.waveOutOpen(out hWaveOut, 0, format.GetBytes(), IntPtr.Zero, 0, 0);
            OpenNETCF.Media.WaveAudio.NativeMethods.waveOutSetVolume(hWaveOut, Volume);
            OpenNETCF.Media.WaveAudio.NativeMethods.waveOutClose(hWaveOut);
        }

        /// <summary>
        /// Get the current volume setting for the default waveOut device (device ID = 0)
        /// </summary>
        /// <returns></returns>
        public static int GetVolume()
        {
            WaveFormat2 format = new WaveFormat2();
            IntPtr hWaveOut = IntPtr.Zero;
            int volume = 0;

            OpenNETCF.Media.WaveAudio.NativeMethods.waveOutOpen(out hWaveOut, 0, format.GetBytes(), IntPtr.Zero, 0, 0);
            OpenNETCF.Media.WaveAudio.NativeMethods.waveOutGetVolume(hWaveOut, ref volume);
            OpenNETCF.Media.WaveAudio.NativeMethods.waveOutClose(hWaveOut);

            return volume;
        }
    }
}
