using PodcastManager.ItunesCrawler.Adapters;
using PodcastManager.ItunesCrawler.Domain.Interactors;
using PodcastManager.ItunesCrawler.Messages;
using Serilog;

namespace PodcastManager.ItunesCrawler.Application.Services;

public class GenreService : IGenreInteractor
{
    private const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ#";
    private IItunesAdapter itunes = null!;
    private IItunesCrawlerEnqueuerAdapter itunesCrawlerEnqueuer = null!;
    private ILogger logger = null!;

    public async Task Execute()
    {
        var genres = await itunes.GetGenres();

        var letters = genres
            .SelectMany(_ => Letters, (genre, letter) => new Letter(genre, letter))
            .ToList();

        itunesCrawlerEnqueuer.EnqueueLetter(letters);
        logger.Information("Enqueueing {TotalLetters:N0} letters for {TotalGenres} genres",
            letters.Count, genres.Length);
    }

    public void SetItunes(IItunesAdapter itunes)
    {
        this.itunes = itunes;
    }

    public void SetLogger(ILogger logger)
    {
        this.logger = logger;
    }

    public void SetEnqueuer(IItunesCrawlerEnqueuerAdapter itunesCrawlerEnqueuer)
    {
        this.itunesCrawlerEnqueuer = itunesCrawlerEnqueuer;
    }
}