using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Media.WaveAudio
{
    /// <summary>
    /// Native wave in/out methods.
    /// </summary>
    public class Wave
    {
        private Wave() { }

        

        /// <summary>
        /// Set the volume for the default waveOut device (device ID = 0)
        /// </summary>
        /// <param name="Volume"></param>
        public void SetVolume(int Volume)
        {
            WaveFormat2 format = new WaveFormat2();
            IntPtr hWaveOut = IntPtr.Zero;

            NativeMethods.waveOutOpen(out hWaveOut, 0, format.GetBytes(), IntPtr.Zero, 0, 0);
            NativeMethods.waveOutSetVolume(hWaveOut, Volume);
            NativeMethods.waveOutClose(hWaveOut);
        }

        /// <summary>
        /// Set the volume for an already-open waveOut device
        /// </summary>
        /// <param name="hWaveOut"></param>
        /// <param name="Volume"></param>
        public void SetVolume(IntPtr hWaveOut, int Volume)
        {
            NativeMethods.waveOutSetVolume(hWaveOut, Volume);
        }

        /// <summary>
        /// Get the current volume setting for the default waveOut device (device ID = 0)
        /// </summary>
        /// <returns></returns>
        public int GetVolume()
        {
            WaveFormat2 format = new WaveFormat2();
            IntPtr hWaveOut = IntPtr.Zero;
            int volume = 0;

            NativeMethods.waveOutOpen(out hWaveOut, 0, format.GetBytes(), IntPtr.Zero, 0, 0);
            NativeMethods.waveOutGetVolume(hWaveOut, ref volume);
            NativeMethods.waveOutClose(hWaveOut);

            return volume;
        }

        /// <summary>
        /// Set the current volume setting for an already-open waveOut device
        /// </summary>
        /// <param name="hWaveOut"></param>
        /// <returns></returns>
        public int GetVolume(IntPtr hWaveOut)
        {
            int volume = 0;

            NativeMethods.waveOutGetVolume(hWaveOut, ref volume);

            return volume;
        }
    }
}
