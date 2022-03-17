using PodcastManager.ItunesCrawler.Messages;

namespace PodcastManager.ItunesCrawler.Adapters;

public interface IItunesCrawlerEnqueuerAdapter
{
    void EnqueueLetter(IEnumerable<Letter> letters);
    void EnqueuePage(IEnumerable<Page> pages);
    void EnqueueStart();
}