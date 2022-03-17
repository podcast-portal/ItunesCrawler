using PodcastManager.ItunesCrawler.Models;

namespace PodcastManager.ItunesCrawler.Domain.Repositories;

public interface IPodcastRepository
{
    Task<(int total, int newPodcasts, int updated)> Upsert(Podcast[] podcasts);
}