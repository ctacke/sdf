using System;

using System.Collections.Generic;
using System.Windows.Forms;
using OpenNETCF.Media.WaveAudio;
using System.IO;
using OpenNETCF.Media;

namespace TestHarness
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Tone[] scale = new Tone[]
            {
                // up fast, using MIDI
                new Tone { Duration = 10, MIDINote = 63},
                new Tone { Duration = 10, MIDINote = 65},
                new Tone { Duration = 10, MIDINote = 67},
                new Tone { Duration = 10, MIDINote = 68},
                new Tone { Duration = 10, MIDINote = 70},
                new Tone { Duration = 10, MIDINote = 72},
                new Tone { Duration = 10, MIDINote = 74},
                new Tone { Duration = 10, MIDINote = 75},

                // down slow, using freq (same notes as above)
                new Tone { Duration = 100, Frequency = 622 },
                new Tone { Duration = 100, Frequency = 587 },
                new Tone { Duration = 100, Frequency = 523 },
                new Tone { Duration = 100, Frequency = 466 },
                new Tone { Duration = 100, Frequency = 415 },
                new Tone { Duration = 100, Frequency = 391 },
                new Tone { Duration = 100, Frequency = 349 },
                new Tone { Duration = 100, Frequency = 311 },
            };

            SoundPlayer.PlayTone(scale);

            Application.Run(new Form1());
        }
    }
}