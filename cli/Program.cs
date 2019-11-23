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
            static string f(string n) => Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                "Soundaufnahmen", n);

            var inputFilename = f("input.mp3");
            var outputFilename = f("translated.wav");
            var subscriptionKey = Environment.GetEnvironmentVariable("SPEECH_API_KEY");
            var region = Environment.GetEnvironmentVariable("SPEECH_API_REGION");

            var mp3Bytes = await File.ReadAllBytesAsync(inputFilename);
            await CognitiveExtensions.Translate(mp3Bytes, subscriptionKey, region, outputFilename, Mp3ConversionAlgorithm.NAudio);
        }
    }
}