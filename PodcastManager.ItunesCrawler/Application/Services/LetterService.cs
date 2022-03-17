using PodcastManager.ItunesCrawler.Adapters;
using PodcastManager.ItunesCrawler.Domain.Interactors;
using PodcastManager.ItunesCrawler.Messages;
using Serilog;

namespace PodcastManager.ItunesCrawler.Application.Services;

public class LetterService : ILetterInteractor
{
    private IItunesAdapter itunes = null!;
    private IItunesCrawlerEnqueuerAdapter itunesCrawlerEnqueuer = null!;
    private ILogger logger = null!;

    public async Task Execute(Letter letter)
    {
        var totalPages = await itunes.GetTotalPages(letter);
        if (totalPages == 0)
            totalPages = 1;

        var pages = new List<Page>(totalPages);
        for (var i = 1; i < totalPages + 1; i++)
            pages.Add(new Page(letter, i));
        itunesCrawlerEnqueuer.EnqueuePage(pages);
        logger.Information("Enqueueing {TotalPages} pages for {Genre} letter {Letter}",
            totalPages, letter.Genre.Name, letter.Char);
    }

    public void SetEnqueuer(IItunesCrawlerEnqueuerAdapter itunesCrawlerEnqueuer)
    {
        this.itunesCrawlerEnqueuer = itunesCrawlerEnqueuer;
    }

    public void SetItunes(IItunesAdapter itunes)
    {
        this.itunes = itunes;
    }

    public void SetLogger(ILogger logger)
    {
        this.logger = logger;
    }
}