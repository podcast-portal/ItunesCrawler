using PodcastManager.Core.CrossCutting.Mongo;
using PodcastManager.ItunesCrawler.CrossCutting.IoC;
using PodcastManager.ItunesCrawler.CrossCutting.Rabbit;
using RabbitMQ.Client;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("ApplicationName", "iTunes Crawler")
    .MinimumLevel.Information()
    .CreateLogger();

Log.Logger.Information("iTunes Crawler service starting");

var closing = new AutoResetEvent(false);

MongoConfiguration.SetConventions();
var repositoryFactory = new RepositoryFactory();
var connectionFactory = new ConnectionFactory {Uri = new Uri(RabbitConfiguration.Host)};

var interactorFactory = new InteractorFactory();
interactorFactory.SetConnectionFactory(connectionFactory);
interactorFactory.SetRepositoryFactory(repositoryFactory);
interactorFactory.SetLogger(Log.Logger);

var listener = new RabbitItunesCrawlerListenerAdapter();
listener.SetInteractorFactory(interactorFactory);
listener.SetConnectionFactory(connectionFactory);
listener.SetLogger(Log.Logger);
listener.Listen();


Console.CancelKeyPress += OnExit;
closing.WaitOne();
listener.Dispose();

void OnExit(object? sender, ConsoleCancelEventArgs args)
{
    Log.Logger.Information("Service iTunes Crawler is exiting");
    closing.Set();
}