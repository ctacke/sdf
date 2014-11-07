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
