using MongoDB.Driver;
using PodcastManager.Core.CrossCutting.Mongo;
using PodcastManager.ItunesCrawler.CrossCutting.Mongo;
using PodcastManager.ItunesCrawler.Domain.Factories;
using PodcastManager.ItunesCrawler.Domain.Repositories;

namespace PodcastManager.ItunesCrawler.CrossCutting.IoC;

public class RepositoryFactory : IRepositoryFactory
{
    public IPodcastRepository CreatePodcast()
    {
        var client = new MongoClient(MongoConfiguration.MongoUrl);
        var database = client.GetDatabase(MongoConfiguration.MongoDatabase);
        var repository = new MongoPodcastRepository();
        
        repository.SetDatabase(database);
        
        return repository;
    }
}