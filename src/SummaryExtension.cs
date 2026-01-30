using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Soenneker.Benchmarking.Extensions.Summary;

/// <summary>
/// Providing helper methods for BenchmarkDotNet's Summary object.
/// </summary>
public static class SummaryExtension
{
    public static async ValueTask OutputSummaryToLog(
        this BenchmarkDotNet.Reports.Summary summary,
        ITestOutputHelper outputHelper,
        CancellationToken cancellationToken = default)
    {
        if (summary is null) throw new ArgumentNullException(nameof(summary));
        if (outputHelper is null) throw new ArgumentNullException(nameof(outputHelper));

        string? path = summary.LogFilePath;

        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
        {
            outputHelper.WriteLine("BenchmarkDotNet log file path was null/empty or the file does not exist.");

            if (!string.IsNullOrWhiteSpace(path))
                outputHelper.WriteLine($"LogFilePath: {path}");
            return;
        }

        cancellationToken.ThrowIfCancellationRequested();

        // Open with async + sequential scan (helpful on Windows; harmless elsewhere)
        await using var stream = new FileStream(
            path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite,
            bufferSize: 64 * 1024,
            options: FileOptions.Asynchronous | FileOptions.SequentialScan);

        using var reader = new StreamReader(stream);

        string content = await reader.ReadToEndAsync(cancellationToken).NoSync();

        if (content.Length == 0)
        {
            outputHelper.WriteLine("(BenchmarkDotNet log file was empty.)");
            return;
        }

        // xUnit output is slow; one call is usually much faster than thousands.
        outputHelper.WriteLine(content);
    }
}
