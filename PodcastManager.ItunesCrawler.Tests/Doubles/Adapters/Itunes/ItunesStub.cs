using System.Threading.Tasks;
using PodcastManager.ItunesCrawler.Adapters;
using PodcastManager.ItunesCrawler.Messages;
using PodcastManager.ItunesCrawler.Models;

namespace PodcastManager.ItunesCrawler.Doubles.Adapters.Itunes;

public class ItunesStub : IItunesAdapter
{
    public int[] PodcastCodes { get; } = {1, 2, 3, 4};

    public ItunesPodcast[] Podcasts { get; } =
    {
        new(1, "PC 1", "feed1", new[] {1, 2, 3},
            new[] {"Genre 1", "Genre 2", "Genre 3"}, "image 1", "Genre 1"),
        new(2, "PC 2", "feed2", new[] {1, 2, 3},
            new[] {"Genre 1", "Genre 2", "Genre 3"}, "image 2", "Genre 2"),
        new(3, "PC 3", "feed3", new[] {1, 2, 3},
            new[] {"Genre 1", "Genre 2", "Genre 3"}, "image 3", "Genre 3"),
        new(4, "PC 4", "feed4", new[] {1, 2, 3},
            new[] {"Genre 1", "Genre 2", "Genre 3"}, "image 4", "Genre 2"),
    };

    public virtual Task<AppleGenre[]> GetGenres()
    {
        return Task.FromResult(new[]
        {
            new AppleGenre(1, "Genre 1"),
            new AppleGenre(2, "Genre 2"),
            new AppleGenre(3, "Genre 3")
        });
    }

    public virtual Task<short> GetTotalPages(Letter letter) => 
        Task.FromResult((short) 4);

    public virtual Task<int[]> PodcastsFromPage(Page page) =>
        Task.FromResult(PodcastCodes);
    
    public virtual Task<ItunesPodcast[]> GetPodcasts(int[] codes) =>
        Task.FromResult(Podcasts);
}