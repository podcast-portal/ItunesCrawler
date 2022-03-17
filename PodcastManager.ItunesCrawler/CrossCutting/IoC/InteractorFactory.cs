using PodcastManager.ItunesCrawler.Adapters;
using PodcastManager.ItunesCrawler.Application.Services;
using PodcastManager.ItunesCrawler.CrossCutting.Http;
using PodcastManager.ItunesCrawler.CrossCutting.Rabbit;
using PodcastManager.ItunesCrawler.Domain.Factories;
using PodcastManager.ItunesCrawler.Domain.Interactors;
using RabbitMQ.Client;
using Serilog;

namespace PodcastManager.ItunesCrawler.CrossCutting.IoC;

public class InteractorFactory : IInteractorFactory
{
    private IRepositoryFactory repositoryFactory = null!;
    private IConnectionFactory connectionFactory = null!;
    private ILogger logger = null!;

    public void SetRepositoryFactory(IRepositoryFactory repositoryFactory) => 
        this.repositoryFactory = repositoryFactory;
    public void SetConnectionFactory(IConnectionFactory connectionFactory) => 
        this.connectionFactory = connectionFactory;
    public void SetLogger(ILogger logger) => this.logger = logger;

    public IGenreInteractor CreateGenre()
    {
        var service = new GenreService();
        service.SetEnqueuer(CreateEnqueuerAdapter());
        service.SetItunes(CreateItunesAdapter());
        service.SetLogger(logger);
        return service;
    }

    public ILetterInteractor CreateLetter()
    {
        var service = new LetterService();
        service.SetEnqueuer(CreateEnqueuerAdapter());
        service.SetItunes(CreateItunesAdapter());
        service.SetLogger(logger);
        return service;
    }

    public IPageInteractor CreatePage()
    {
        var service = new PageService();
        service.SetItunes(CreateItunesAdapter());
        service.SetRepository(repositoryFactory.CreatePodcast());
        service.SetLogger(logger);
        return service;
    }
    
    private IItunesAdapter CreateItunesAdapter()
    {
        var adapter = new ItunesAdapter();
        adapter.SetFactory(new HttpClientFactory());
        adapter.SetLogger(logger);
        return adapter;
    }

    private IItunesCrawlerEnqueuerAdapter CreateEnqueuerAdapter()
    {
        var adapter = new RabbitItunesCrawlerEnqueuerAdapter();
        adapter.SetConnection(connectionFactory.CreateConnection());
        return adapter;
    }
}