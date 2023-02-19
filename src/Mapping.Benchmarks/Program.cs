using BenchmarkDotNet.Running;

using Mapping.Benchmarks;

BenchmarkRunner.Run<MappingBenchmarks>();

//var bm = new MappingBenchmarks();
//bm.GlobalSetup();

//bm.ToMappingDocumentExtension();
//bm.FromMappingDocumentExtension();

//bm.ToMappingCustomObjectExtension();
//bm.FromMappingCustomObjectExtension();
