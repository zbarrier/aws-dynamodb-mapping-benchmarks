using Amazon.DynamoDBv2.Model;

using Mapping.Benchmarks.Models;

namespace Mapping.Benchmarks.Extensions;
public static class DeviceExtensions
{
    const int PropertyCount = 7;

    const string IdKey = "Id";
    const string IntervalKey = "Interval";
    const string ReadingOneKey = "ReadingOne";
    const string ReadingOneOnKey = "ReadingOneOn";
    const string ReadingTwoKey = "ReadingTwoKey";
    const string ReadingTwoOnKey = "ReadingTwoOnKey";
    const string LastReadingOnKey = "LastReadingOn";

    public static Device ToDevice(this Dictionary<string, AttributeValue> item)
    {
        var device = new Device()
        {
            Id = item[IdKey].S,
            Interval = int.Parse(item[IntervalKey].N),
            ReadingOne = int.Parse(item[ReadingOneKey].N),
            ReadingOneOn = new DateTime(long.Parse(item[ReadingOneOnKey].N)),
            ReadingTwo = int.Parse(item[ReadingTwoKey].N),
            ReadingTwoOn = new DateTime(long.Parse(item[ReadingTwoOnKey].N)),
            LastReadingOn = new DateTime(long.Parse(item[LastReadingOnKey].N)),
        };

        return device;
    }

    public static Dictionary<string, AttributeValue> ToDynamoDBItem(this Device device)
    {
        var item = new Dictionary<string, AttributeValue>(PropertyCount)
        {
            { IdKey, new AttributeValue { S = device.Id } },
            { IntervalKey, new AttributeValue { N = device.Interval.ToString() } },
            { ReadingOneKey, new AttributeValue { N = device.ReadingOne.ToString() } },
            { ReadingOneOnKey, new AttributeValue { N = device.ReadingOneOn.Ticks.ToString() } },
            { ReadingTwoKey, new AttributeValue { N = device.ReadingTwo.ToString() } },
            { ReadingTwoOnKey, new AttributeValue { N = device.ReadingTwoOn.Ticks.ToString() } },
            { LastReadingOnKey, new AttributeValue { N = device.LastReadingOn.Ticks.ToString() } },
        };

        return item;
    }
}
