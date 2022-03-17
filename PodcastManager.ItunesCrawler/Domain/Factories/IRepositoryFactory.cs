using PodcastManager.ItunesCrawler.Domain.Repositories;

namespace PodcastManager.ItunesCrawler.Domain.Factories;

public interface IRepositoryFactory
{
    IPodcastRepository CreatePodcast();
}