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
            var inputFilename = fullPath("input.mp3");
            var outputFilename = fullPath("translated.wav");

            var subscriptionKey = Environment.GetEnvironmentVariable("SPEECH_API_KEY");
            var region = Environment.GetEnvironmentVariable("SPEECH_API_REGION");

            using var mp3stream = File.OpenRead(inputFilename);

            await CognitiveExtensions.Translate(mp3stream, subscriptionKey, region,
                outputFilename, Mp3ConversionAlgorithm.FFMPEG);

            //using var wavStream = File.OpenRead(fullPath("translated.wav"));
            //var mp3bytes = await wavStream.ConvertWAV2MP3_NAudio();
            //await File.WriteAllBytesAsync(fullPath("translated.mp3"), mp3bytes);
        }
    }
}