using PodcastManager.ItunesCrawler.Messages;
using PodcastManager.ItunesCrawler.Models;

namespace PodcastManager.ItunesCrawler.Adapters;

public interface IItunesAdapter
{
    Task<AppleGenre[]> GetGenres();
    Task<short> GetTotalPages(Letter letter);
    Task<int[]> PodcastsFromPage(Page page);
    Task<ItunesPodcast[]> GetPodcasts(int[] codes);
}