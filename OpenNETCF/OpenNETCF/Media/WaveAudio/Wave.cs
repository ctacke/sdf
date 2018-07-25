#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



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
