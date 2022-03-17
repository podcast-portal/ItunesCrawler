using System.Threading.Tasks;
using PodcastManager.ItunesCrawler.Domain.Repositories;
using PodcastManager.ItunesCrawler.Models;

namespace PodcastManager.ItunesCrawler.Doubles.Repositories;

public class PodcastRepositorySpy : IPodcastRepository
{
    public SpyHelper<Podcast[]> UpsertSpy { get; } = new();
    
    public Task<(int total, int newPodcasts, int updated)> Upsert(Podcast[] podcasts)
    {
        UpsertSpy.Call(podcasts);
        return Task.FromResult((10, 0, 0));
    }
}