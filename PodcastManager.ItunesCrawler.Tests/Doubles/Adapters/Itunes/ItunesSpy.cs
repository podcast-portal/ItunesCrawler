using System.Threading.Tasks;
using PodcastManager.ItunesCrawler.Messages;
using PodcastManager.ItunesCrawler.Models;

namespace PodcastManager.ItunesCrawler.Doubles.Adapters.Itunes;

public class ItunesSpy : ItunesStub
{
    public SpyHelper ListGenresSpy { get; } = new();
    public SpyHelper<Letter> GetTotalPagesSpy { get; } = new();
    public SpyHelper<Page> PodcastsFromPageSpy { get; } = new();
    public SpyHelper<int[]> GetPodcastsSpy { get; } = new();

    public override Task<AppleGenre[]> GetGenres()
    {
        ListGenresSpy.Call();
        return base.GetGenres();
    }

    public override Task<short> GetTotalPages(Letter letter)
    {
        GetTotalPagesSpy.Call(letter);
        return base.GetTotalPages(letter);
    }

    public override Task<ItunesPodcast[]> GetPodcasts(int[] codes)
    {
        GetPodcastsSpy.Call(codes);
        return base.GetPodcasts(codes);
    }

    public override Task<int[]> PodcastsFromPage(Page page)
    {
        PodcastsFromPageSpy.Call(page);
        return base.PodcastsFromPage(page);
    }
}