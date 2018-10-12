using System;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using VV;
// recognition

namespace ConsoleSpeech
{
    class ConsoleSpeechProgram
    {

        static bool done = false;
        static bool speechOn = true;

        static void Main(string[] args)
        {
            try
            {
                var culture = CultureInfo.GetCultureInfo("it-IT");

                //Speak.Do(culture);
                Listen.Do(culture);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

        } // Main

 // sre_SpeechRecognized

    } // Program

} // ns
