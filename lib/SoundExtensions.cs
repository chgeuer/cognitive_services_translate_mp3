namespace lib
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using NAudio.Wave;

    public enum Mp3ConversionAlgorithm
    {
        NAudio,
        FFMPEG
    }

    public static class SoundExtensions
    {
        const int audio_sample_rate = 16000;
        const int audio_bit_per_sample = 16;
        const int audio_channels = 1;

        public static Task<byte[]> ConvertMP3(this Stream mp3Stream, Mp3ConversionAlgorithm algo)
        {
            return algo switch
            {
                Mp3ConversionAlgorithm.NAudio => Task.FromResult(mp3Stream.ConvertMP3_NAudio()),
                Mp3ConversionAlgorithm.FFMPEG => mp3Stream.ConvertMP3_FFMPEG(),
                _ => throw new ArgumentException($"Unknown MP3-to-WAV algorithm {algo}", paramName: nameof(algo)),
            };
        }

        private static byte[] ConvertMP3_NAudio(this Stream mp3Stream)
        {
            using var reader = new Mp3FileReader(mp3Stream);
            using var resampled = new WaveFormatConversionStream(
                new WaveFormat(
                    rate: audio_sample_rate,
                    bits: audio_bit_per_sample,
                    channels: audio_channels), 
                reader);
            using var wavStream = new MemoryStream();
            WaveFileWriter.WriteWavFileToStream(wavStream, resampled);
            return wavStream.ToArray();
        }

        private static async Task<byte[]> ConvertMP3_FFMPEG(this Stream mp3Stream)
        {
            var audio_codec = audio_bit_per_sample switch
            {
                16 => "pcm_s16le",
                _ => throw new ArgumentException(
                    $"Unsupported bits_per_sample {audio_bit_per_sample}",
                    paramName: nameof(audio_bit_per_sample)),
            };

            string readFromStdin = "-i -";
            string produceWAV = $"-f wav -c:a {audio_codec}";
            string audioParams = $"-ar {audio_sample_rate} -ac {audio_channels}";
            string writeToStdout = "-";
            string ffmpeg_args = $"{readFromStdin} {produceWAV} {audioParams} {writeToStdout}";

            var result = await ProcessExtensions.RunAsync("ffmpeg", ffmpeg_args, mp3Stream);
            return result.Stdout.TweakWavLength();
        }

        private static byte[] CloneArray(this byte[] array)
        {
            byte[] result = new byte[array.Length];
            Buffer.BlockCopy(array, 0, result, 0, array.Length * sizeof(byte));
            return result;
        }

        private static byte[] TweakWavLength(this byte[] wavByteArray)
        {
            // un-fuck the WAV length...
            // mplayer.exe -demuxer rawaudio -rawaudio rate=16000:channels=1:samplesize=2 -ao pcm out.wav
            var bytes = wavByteArray.CloneArray();
            var byteLength = bytes.Length;
            void tweak(int index, int rotate, long subtractionOffset)
            {
                var length = byteLength - subtractionOffset;
                bytes[index] = (byte)((length >> (rotate * 8)) & 0xff);
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

            return bytes;
        }
    }
}