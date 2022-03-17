using System;
using PodcastManager.ItunesCrawler.Adapters;

namespace PodcastManager.ItunesCrawler.Doubles.Adapters;

public class DateTimeStub : IDateTimeAdapter
{
    public DateTime Now() => new(2020, 6, 25, 10, 0, 0);
}