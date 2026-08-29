[![](https://img.shields.io/nuget/v/soenneker.benchmarking.extensions.summary.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.benchmarking.extensions.summary/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.benchmarking.extensions.summary/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.benchmarking.extensions.summary/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.benchmarking.extensions.summary.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.benchmarking.extensions.summary/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.benchmarking.extensions.summary/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.benchmarking.extensions.summary/actions/workflows/codeql.yml)

# Soenneker.Benchmarking.Extensions.Summary

A BenchmarkDotNet `Summary` extension that copies the generated benchmark log into the current TUnit test output.

## Installation

```bash
dotnet add package Soenneker.Benchmarking.Extensions.Summary
```

## Usage in a TUnit test

```csharp
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;

public sealed class BenchmarkTests
{
    [Test]
    public async Task Run_benchmarks()
    {
        BenchmarkDotNet.Reports.Summary summary =
            BenchmarkRunner.Run<SerializationBenchmarks>();

        await summary.OutputSummaryToLog();
    }
}
```

`OutputSummaryToLog()` reads `Summary.LogFilePath` and writes the full file contents to `TestContext.Current.Output`. This makes BenchmarkDotNet output visible in the TUnit report and CI test logs.

## Behavior

- Outside an active TUnit test, `TestContext.Current` is `null` and the method returns without output.
- A missing, blank, or nonexistent `LogFilePath` writes a diagnostic message instead of throwing.
- An empty log file writes an explicit empty-file message.
- The file is opened with shared read access, so BenchmarkDotNet or another process can still hold it open.
- Cancellation is checked before and during the asynchronous file read.
- Passing a null `Summary` throws `ArgumentNullException`.

BenchmarkDotNet logs can contain machine, runtime, path, and benchmark metadata. Review that content before publishing TUnit output from a public CI job.
