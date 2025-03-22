using BenchmarkDotNet.Running;
using LookupEngine.Tests.Performance;

BenchmarkRunner.Run<ResolveTypeBenchmark>();
// BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly());