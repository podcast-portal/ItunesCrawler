using System.Collections.Generic;
using System.Threading.Tasks;
using PodcastManager.ItunesCrawler.Adapters;
using PodcastManager.ItunesCrawler.Messages;

namespace PodcastManager.ItunesCrawler.Doubles.Adapters.Enqueuer;

public class ItunesCrawlerEnqueuerSpy : IItunesCrawlerEnqueuerAdapter
{
    public SpyHelper<IEnumerable<Letter>> EnqueueLetterSpy { get; } = new();
    public SpyHelper<IEnumerable<Page>> EnqueuePageSpy { get; } = new();

    public void EnqueueLetter(IEnumerable<Letter> letters) => EnqueueLetterSpy.Call(letters);

    public void EnqueuePage(IEnumerable<Page> pages) => EnqueuePageSpy.Call(pages);
    
    public void EnqueueStart()
    {
        throw new System.NotImplementedException();
    }
}