[![](https://img.shields.io/nuget/v/soenneker.benchmarking.extensions.summary.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.benchmarking.extensions.summary/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.benchmarking.extensions.summary/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.benchmarking.extensions.summary/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.benchmarking.extensions.summary.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.benchmarking.extensions.summary/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.benchmarking.extensions.summary/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.benchmarking.extensions.summary/actions/workflows/codeql.yml)

# Soenneker.Benchmarking.Extensions.Summary

Providing helper methods for BenchmarkDotNet's Summary object.

## Install

```bash
dotnet add package Soenneker.Benchmarking.Extensions.Summary
```

## Quick start

```csharp
using Soenneker.Benchmarking.Extensions.Summary;

BenchmarkDotNet.Reports.Summary summary = /* obtain from your application */;
await summary.OutputSummaryToLog(default);
```

Writes the benchmark summary and key statistics to the log.

## What you get

- `SummaryExtension` — Providing helper methods for BenchmarkDotNet's Summary object.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `SummaryExtension.OutputSummaryToLog(summary, cancellationToken)` | Writes the benchmark summary and key statistics to the log. | A task that completes when the output summary to log operation is complete. |

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
