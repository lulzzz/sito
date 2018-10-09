using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace VV
{
    class Speak
    {
        static SpeechSynthesizer ss = new SpeechSynthesizer();

        public static void Do(CultureInfo culture)
        {
            var k = ss.GetInstalledVoices();

            ss.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen, 0, culture);
            ss.SetOutputToDefaultAudioDevice();
            var v = ss.GetInstalledVoices().ToArray();

            Console.WriteLine("\n(Speaking: I am awake)");
            ss.Speak("Ciao patato, sono Maddalena");

        }
    }
}
