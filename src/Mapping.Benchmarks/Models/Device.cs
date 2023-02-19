namespace Mapping.Benchmarks.Models;
public sealed class Device
{
    public string Id { get; set; }

    public int Interval { get; set; }

    public int ReadingOne { get; set; }
    public DateTime ReadingOneOn { get; set; }

    public int ReadingTwo { get; set; }
    public DateTime ReadingTwoOn { get; set; }

    public DateTime LastReadingOn { get; set; }
}
