using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

using BenchmarkDotNet.Attributes;

using Mapping.Benchmarks.Extensions;
using Mapping.Benchmarks.Models;

namespace Mapping.Benchmarks;

[MemoryDiagnoser]
[RyuJitX64Job]
public class MappingBenchmarks
{
    IAmazonDynamoDB _dynamoDBClient;
    DynamoDBContext _dynamoDBContext;

    Device _device;
    object _deviceObj;
    string _deviceClrType;

    Document _doc;
    Dictionary<string, AttributeValue> _attributeValues;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _dynamoDBClient = new AmazonDynamoDBClient("fakeKey", "fakeSecret", Amazon.RegionEndpoint.USEast1);
        _dynamoDBContext = new DynamoDBContext(_dynamoDBClient, new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2, ConsistentRead = true });

        var dtLastPowerReadingOn = DateTime.UtcNow.AddMinutes(-2);
        var dtLastSignalStrengthReadingOn = dtLastPowerReadingOn.AddMilliseconds(-100);

        _device = new Device()
        {
            Id = "10000000000000000001",
            Interval = 10,
            ReadingOne = 9,
            ReadingOneOn = dtLastPowerReadingOn,
            ReadingTwo = 7,
            ReadingTwoOn = dtLastSignalStrengthReadingOn,
            LastReadingOn = dtLastPowerReadingOn,
        };
        _deviceObj = _device;
        _deviceClrType = typeof(Device).FullName!;

        _doc = _dynamoDBContext.ToDocumentDynamic(_deviceObj, _deviceObj.GetType())!;
        _attributeValues = _device.ToDynamoDBItem();

        _ = _dynamoDBContext.FromDocumentDynamic(_doc, _deviceObj.GetType());
        _ = _attributeValues.ToDevice();
    }

    [Benchmark]
    public void ToMappingDocumentExtension()
    {
        var dataType = _deviceObj.GetType();
        var document = _dynamoDBContext.ToDocumentDynamic(_deviceObj, dataType);
    }

    [Benchmark]
    public void ToMappingCustomObjectExtension()
    {
        var attrValues = _device.ToDynamoDBItem();
    }

    [Benchmark]
    public void FromMappingDocumentExtension()
    {
        var dataType = Type.GetType(_deviceClrType);
        var deviceObj = _dynamoDBContext.FromDocumentDynamic(_doc, dataType!);
    }

    [Benchmark]
    public void FromMappingCustomObjectExtension()
    {
        var device = _attributeValues.ToDevice();
    }
}
