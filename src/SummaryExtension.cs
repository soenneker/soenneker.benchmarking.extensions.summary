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
        string[] logs = await File.ReadAllLinesAsync(summary.LogFilePath, cancellationToken).ConfigureAwait(false);

        for (var i = 0; i < logs.Length; i++)
        {
            string log = logs[i];
            outputHelper.WriteLine(log);
        }
    }
}
