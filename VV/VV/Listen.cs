using Microsoft.Speech.Recognition;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VV
{
    class Listen
    {
        static SpeechRecognitionEngine sre;

        public static void Do(CultureInfo culture)
        {
            var l = SpeechRecognitionEngine.InstalledRecognizers();

            sre = new SpeechRecognitionEngine(culture);
            sre.SetInputToDefaultAudioDevice();

            sre.SpeechRecognized += sre_SpeechRecognized;

            GrammarBuilder startStop = new GrammarBuilder();

            startStop.Append("Ciao");
            startStop.Append();
            startStop.Append("Addio");
            Grammar grammar = new Grammar(startStop);
            grammar.Enabled = true;
            grammar.Name = " Free-Text Dictation ";
            sre.LoadGrammarAsync(grammar);
            sre.LoadGrammarCompleted += Sre_LoadGrammarCompleted;

            sre.RecognizeAsync(RecognizeMode.Multiple); // multiple grammars
        }

        private static void Sre_LoadGrammarCompleted(object sender, LoadGrammarCompletedEventArgs e)
        {
        }

        static void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine(e.Result.Text);

            string txt = e.Result.Text;
            float confidence = e.Result.Confidence; // consider implicit cast to double
        }
    }
}
