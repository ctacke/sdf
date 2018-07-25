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

namespace OpenNETCF.Media
{
    /// <summary>
    /// Represents a note for the SoundPlayer to play.
    /// </summary>
    /// <remarks>
    /// Internally this class stores data as the MIDINote value, which is a whole number. Due to mathematical rounding, if you set a 
    /// specific frequency, you may not get the exact same frequency back on a read.  Instead you will get the frequency associated with the nearest
    /// MIDI note to the frequency.
    /// </remarks>
    public class Tone
    {
        /// <summary>
        /// frequency in Hertz
        /// </summary> 
        public float Frequency
        {
            set { MIDINote = GetNote(value); }
            get { return GetFrequency(MIDINote); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="midiNote"></param>
        /// <returns></returns>
        /// <remarks>
        /// freq = 440 * 2^((n-69)/12)
        /// </remarks>
        public static int GetFrequency(int midiNote)
        {
            return (int)(440f * Math.Pow(2f, ((float)midiNote - 69f) / 12f));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frequency"></param>
        /// <returns></returns>
        /// <remarks>
        /// n = 69 + 12*log(freq/440)/log(2)
        /// </remarks>
        public static int GetNote(float frequency)
        {            
            return (int)(69f + 12f * Math.Log(frequency / 440f) / Math.Log(2f));
        }

        /// <summary>
        /// MIDI note value
        /// </summary>
        public int MIDINote { get; set; }

        /// <summary>
        /// Duration in tenths of a second
        /// </summary>
        public int Duration { get; set; }
    }
}
