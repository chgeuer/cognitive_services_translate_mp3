namespace lib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.CognitiveServices.Speech;
    using Microsoft.CognitiveServices.Speech.Audio;
    using Microsoft.CognitiveServices.Speech.Translation;
    using MicrosoftSpeechSDKSamples;

    public static class CognitiveExtensions
    {
        private static async Task<byte[]> GetWAVFromFile(string inputFilename)
        {
            if (!inputFilename.EndsWith(".wav") && !inputFilename.EndsWith(".mp3")) { throw new ArgumentOutOfRangeException(paramName: nameof(inputFilename), message: "Input filename must have '.wav' or '.mp3' extension"); }
            if (inputFilename.EndsWith(".wav"))
            {
                return await File.ReadAllBytesAsync(inputFilename);
            }
            else
            {
                var mp3bytes = await File.ReadAllBytesAsync(inputFilename);
                using var mp3stream = new MemoryStream(mp3bytes);
                return mp3stream.ConvertMP3();
            }
        }

        public static async Task Translate(string subscriptionKey, string region, string inputFilename, string fromLanguage,  IEnumerable<string> targetLanguages, Voice voice, string outputFilename)
        {
            if (!outputFilename.EndsWith(".wav") && !outputFilename.EndsWith(".mp3")) { throw new ArgumentOutOfRangeException(paramName: nameof(outputFilename), message: "Output filename must have '.wav' or '.mp3' extension"); }

            var config = SpeechTranslationConfig.FromSubscription(subscriptionKey, region);
            var wavBytes = await GetWAVFromFile(inputFilename);
            await config.TranslationWithFileAsync(wavBytes, fromLanguage, targetLanguages, voice, outputFilename);
        }

        public static async Task TranslationWithFileAsync(this SpeechTranslationConfig config, byte[] wavBytes, string fromLanguage, IEnumerable<string> targetLanguages, Voice voice, string outputFilename)
        {
            config.SpeechRecognitionLanguage = fromLanguage;
            config.VoiceName = voice.ToString();
            targetLanguages.ToList().ForEach(config.AddTargetLanguage);

            using var audioInput = AudioConfig.FromStreamInput(
                AudioInputStream.CreatePullStream(
                    new BinaryAudioStreamReader(
                        new MemoryStream(
                            wavBytes))));

            using var recognizer = new TranslationRecognizer(config, audioInput);
            var stopTranslation = new TaskCompletionSource<int>();

            recognizer.Recognizing += (s, e) =>
            {
                Console.WriteLine($"RECOGNIZING in '{fromLanguage}': Text={e.Result.Text}");
                foreach (var element in e.Result.Translations)
                {
                    Console.WriteLine($"    TRANSLATING into '{element.Key}': {element.Value}");
                }
            };

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
            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

            await Task.WhenAny(new[] { stopTranslation.Task });

            // Stops translation.
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(continueOnCapturedContext: false);
        }

        public static void OnSynthesisWriteToFile(this TranslationRecognizer recognizer, string outputFilename)
        {
            recognizer.Synthesizing += (s, e) => {
                byte[] audioBytes = e.Result.GetAudio();
                if (audioBytes.Length == 0) { return; }

                if (outputFilename.EndsWith(".wav"))
                {
                    using var fs = File.OpenWrite(outputFilename);
                    fs.Write(audioBytes, 0, audioBytes.Length);
                }
                else
                {
                    using var ms = new MemoryStream(audioBytes);
                    ms.SaveWavToMp3File(outputFilename);
                }
            };
        }
    }
}
