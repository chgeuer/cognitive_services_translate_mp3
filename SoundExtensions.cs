using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech.Translation;
using NAudio.Wave;

public static class SoundExtensions
{
    public static byte[] ConvertMP3_NAudio(this byte[] mp3Bytes)
    {
        var waveFormat = new WaveFormat(rate: 16000, bits: 16, channels: 1);
        using var mp3Stream = new MemoryStream(mp3Bytes);
        using var reader = new Mp3FileReader(mp3Stream);
        using var resampler = new WaveFormatConversionStream(waveFormat, reader);
        using var wavStream = new MemoryStream();
        WaveFileWriter.WriteWavFileToStream(wavStream, resampler);
        return wavStream.ToArray();
    }

    public static async Task<byte[]> ConvertMP3_FFMPEG(string filename)
    {
        var result = await ProcessExtensions.RunAsync(
            filename: "ffmpeg",
            arguments: $"-i \"{filename}\" -f wav -c:a pcm_s16le -ar 16000 -ac 1 -");
        return result.Stdout.TweakWavLengthInMemory();
    }

    private static byte[] TweakWavLengthInMemory(this byte[] wavByteArray)
    {
        // un-fuck the WAV length...
        // mplayer.exe -demuxer rawaudio -rawaudio rate=16000:channels=1:samplesize=2 -ao pcm out.wav
        // This modifies wavByteArray in memory
        void tweak(int index, int rotate, long subtractionOffset)
        {
            var length = wavByteArray.Length - subtractionOffset;
            wavByteArray[index] = (byte)((length >> (rotate * 8)) & 0xff);
        }
        void patchLong(int startIndex, long subtractionOffset)
        {
            for (var i = 0; i < 4; i++)
            {
                tweak(startIndex + i, i, subtractionOffset);
            }
        }

        patchLong(0x04, 8);
        patchLong(0x74, 120);

        return wavByteArray;
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
