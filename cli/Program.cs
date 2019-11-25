namespace Translate
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using lib;

    class Program
    {
        static async Task Main(string[] _args)
        {
            static string fullPath(string n) => Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                "Soundaufnahmen", n);
            var inputFilename = fullPath("1.mp3");
            var outputFilename = fullPath("result-{}.mp3");

            var subscriptionKey = Environment.GetEnvironmentVariable("SPEECH_API_KEY");
            var region = Environment.GetEnvironmentVariable("SPEECH_API_REGION");

            await CognitiveExtensions.Translate(subscriptionKey: subscriptionKey, region: region, 
                inputFilename: inputFilename, 
                fromLanguage: "en-US",
                targetLanguages: new[] { "en", "de", "it" },
                voice: Voice.en_GB_George_Apollo,
                outputFilename: outputFilename);
        }
    }
}