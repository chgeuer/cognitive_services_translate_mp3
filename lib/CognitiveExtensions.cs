namespace lib
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.CognitiveServices.Speech;
    using Microsoft.CognitiveServices.Speech.Audio;
    using Microsoft.CognitiveServices.Speech.Translation;
    using MicrosoftSpeechSDKSamples;

    public static class CognitiveExtensions
    {
        public static async Task Translate(Stream mp3stream, string subscriptionKey, string region, string outputFilename, Mp3ConversionAlgorithm algo)
        {
            var config = SpeechTranslationConfig.FromSubscription(subscriptionKey, region);
            var wavBytes = await mp3stream.ConvertMP3(algo);
            await config.TranslationWithFileAsync(wavBytes, outputFilename);
        }

        public static async Task TranslationWithFileAsync(this SpeechTranslationConfig config, byte[] wavBytes, string outputFilename)
        {
            const string fromLanguage = "en-US";
            config.SpeechRecognitionLanguage = fromLanguage;
            // https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support
            config.VoiceName = "Microsoft Server Speech Text to Speech Voice (de-DE, Stefan, Apollo)";
            config.AddTargetLanguage("de");
            config.AddTargetLanguage("fr");
            config.AddTargetLanguage("en");

            var stopTranslation = new TaskCompletionSource<int>();

            using var audioInput = AudioConfig.FromStreamInput(
                AudioInputStream.CreatePullStream(
                    new BinaryAudioStreamReader(
                        new MemoryStream(
                            wavBytes))));

            using var recognizer = new TranslationRecognizer(config, audioInput);

            recognizer.Recognizing += (s, e) =>
            {
                Console.WriteLine($"RECOGNIZING in '{fromLanguage}': Text={e.Result.Text}");
                foreach (var element in e.Result.Translations)
                {
                    Console.WriteLine($"    TRANSLATING into '{element.Key}': {element.Value}");
                }
            };
            // DE/FR/EN

            recognizer.Recognized += (s, e) => {
                if (e.Result.Reason == ResultReason.TranslatedSpeech)
                {
                    Console.WriteLine($"RECOGNIZED in '{fromLanguage}': Text={e.Result.Text}");
                    foreach (var element in e.Result.Translations)
                    {
                        Console.WriteLine($"    TRANSLATED into '{element.Key}': {element.Value}");
                    }
                }
                else if (e.Result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"RECOGNIZED: Text={e.Result.Text}");
                    Console.WriteLine($"    Speech not translated.");
                }
                else if (e.Result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                }
            };

            recognizer.Canceled += (s, e) =>
            {
                Console.WriteLine($"CANCELED: Reason={e.Reason}");

                if (e.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                    Console.WriteLine($"CANCELED: Did you update the subscription info?");
                }

                stopTranslation.TrySetResult(0);
            };

            recognizer.SpeechStartDetected += (s, e) => { Console.WriteLine("\nSpeech start detected event."); };
            recognizer.SpeechEndDetected += (s, e) => { Console.WriteLine("\nSpeech end detected event."); };
            recognizer.SessionStarted += (s, e) => { Console.WriteLine("\nSession started event."); };
            recognizer.SessionStopped += (s, e) => {
                Console.WriteLine("\nSession stopped event.");
                Console.WriteLine($"\nStop translation.");
                stopTranslation.TrySetResult(0);
            };

            recognizer.OnSynthesisWriteToFile(outputFilename);

            // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
            Console.WriteLine("Start translation...");
            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

            await Task.WhenAny(new[] { stopTranslation.Task });

            // Stops translation.
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
        }

        public static void OnSynthesisWriteToFile(this TranslationRecognizer recognizer, string filename)
        {
            recognizer.Synthesizing += (s, e) => {
                byte[] audioBytes = e.Result.GetAudio();
                using var fs = File.OpenWrite(filename);
                fs.Write(audioBytes, 0, audioBytes.Length);
            };
        }
    }
}
