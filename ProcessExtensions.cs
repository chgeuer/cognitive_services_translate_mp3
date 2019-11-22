using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public static class ProcessExtensions
{
    public class ProcessResult
    {
        public byte[] Stdout { get; set; }
        public byte[] Stderr { get; set; }
        public int? ExitCode { get; set; }
    }

    public static Task<ProcessResult> RunAsync(string filename, string arguments)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = filename,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        return processStartInfo.RunAsync();
    }

    public static Task<ProcessResult> RunAsync(this ProcessStartInfo startInfo) => startInfo.RunAsync(TimeSpan.Zero);

    public static async Task<ProcessResult> RunAsync(this ProcessStartInfo startInfo, TimeSpan timeout)
    {
        using var stdout = new MemoryStream();
        using var stderr = new MemoryStream();
        using var process = new Process
        {
            StartInfo = startInfo,
            EnableRaisingEvents = true
        };

        var tasks = new List<Task>();

        var processExitEvent = new TaskCompletionSource<object>();
        tasks.Add(processExitEvent.Task);
        process.Exited += (sender, args) => { Task.Run(() => { processExitEvent.TrySetResult(true); }); };

        var startSuccess = process.Start();
        if (!startSuccess)
        {
            return new ProcessResult { Stdout = stdout.ToArray(), Stderr = stderr.ToArray(), ExitCode = process.ExitCode };
        }

        if (process.StartInfo.RedirectStandardOutput)
        {
            tasks.Add(process.StandardOutput.BaseStream.CopyThenClose(stdout));
        }
        if (process.StartInfo.RedirectStandardError)
        {
            tasks.Add(process.StandardError.BaseStream.CopyThenClose(stderr));
        }

        var processCompletionTask = Task.WhenAll(tasks);
        Task<Task> awaitingTask = (timeout > TimeSpan.Zero)
            ? Task.WhenAny(processCompletionTask, Task.Delay(timeout))
            : Task.WhenAny(processCompletionTask);

        var finishedTask = await awaitingTask;
        if (finishedTask == processCompletionTask)
        {
            return new ProcessResult { Stdout = stdout.ToArray(), Stderr = stderr.ToArray(), ExitCode = process.ExitCode };
        }
        else
        {
            try { process.Kill(); } catch { }
            return new ProcessResult { Stdout = stdout.ToArray(), Stderr = stderr.ToArray(), ExitCode = null };
        }
    }

    public static async Task CopyThenClose(this Stream from, Stream to)
    {
        await from.CopyToAsync(to);
        to.Close();
    }
}
