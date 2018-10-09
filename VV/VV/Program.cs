using System;
using System.Globalization;
using System.Linq;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;

// recognition

namespace ConsoleSpeech
{
    class ConsoleSpeechProgram
    {
        static SpeechSynthesizer ss = new SpeechSynthesizer();
        static SpeechRecognitionEngine sre;
        static bool done = false;
        static bool speechOn = true;

        static void Main(string[] args)
        {
            try
            {
                var l = SpeechRecognitionEngine.InstalledRecognizers();

                ss.SetOutputToDefaultAudioDevice();
                var v = ss.GetInstalledVoices().ToArray();

                var culture = CultureInfo.GetCultureInfo("it-IT");
                ss.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen, 0, culture);

                Console.WriteLine("\n(Speaking: I am awake)");
                ss.Speak("Ciao patato, sono Maddalena");

                sre = new SpeechRecognitionEngine(culture);
                sre.SetInputToDefaultAudioDevice();
                
                sre.SpeechRecognized += sre_SpeechRecognized;

                var gBuilder = new GrammarBuilder();
                gBuilder.Append("Maddi");

                var grammar = new Grammar(gBuilder);
                sre.LoadGrammarAsync(grammar);

                sre.RecognizeAsync(RecognizeMode.Multiple); // multiple grammars

                while (done == false) {; }

                Console.WriteLine("\nHit <enter> to close shell\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

        } // Main

        static void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string txt = e.Result.Text;
            float confidence = e.Result.Confidence; // consider implicit cast to double
            Console.WriteLine("\nRecognized: " + txt);


        } // sre_SpeechRecognized

    } // Program

} // ns
