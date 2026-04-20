using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Benchmarking.Extensions.Summary;

/// <summary>
/// Providing helper methods for BenchmarkDotNet's Summary object.
/// </summary>
public static class SummaryExtension
{
    public static async ValueTask OutputSummaryToLog(this BenchmarkDotNet.Reports.Summary summary, CancellationToken cancellationToken = default)
    {
        if (summary is null)
            throw new ArgumentNullException(nameof(summary));

        TestContext? context = TestContext.Current;

        if (context is null)
            return;

        string? path = summary.LogFilePath;

        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
        {
            context.Output.WriteLine("BenchmarkDotNet log file path was null/empty or the file does not exist.");

            if (!string.IsNullOrWhiteSpace(path))
                context.Output.WriteLine($"LogFilePath: {path}");

            return;
        }

        cancellationToken.ThrowIfCancellationRequested();

        await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize: 64 * 1024,
            options: FileOptions.Asynchronous | FileOptions.SequentialScan);

        using var reader = new StreamReader(stream);

        string content = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);

        if (content.Length == 0)
        {
            context.Output.WriteLine("(BenchmarkDotNet log file was empty.)");
            return;
        }

        context.Output.WriteLine(content);
    }
}