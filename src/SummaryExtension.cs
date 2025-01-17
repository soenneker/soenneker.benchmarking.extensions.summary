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
    public static async ValueTask OutputSummaryToLog(this BenchmarkDotNet.Reports.Summary summary, ITestOutputHelper outputHelper, CancellationToken cancellationToken = default)
    {
        await using var stream = new FileStream(summary.LogFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            string? log = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);

            if (log is not null)
            {
                outputHelper.WriteLine(log);
            }
        }
    }
}