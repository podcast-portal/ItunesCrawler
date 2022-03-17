using PodcastManager.ItunesCrawler.Adapters;
using PodcastManager.ItunesCrawler.Domain.Interactors;
using PodcastManager.ItunesCrawler.Domain.Repositories;
using PodcastManager.ItunesCrawler.Messages;
using PodcastManager.ItunesCrawler.Models;
using Serilog;

namespace PodcastManager.ItunesCrawler.Application.Services;

public class PageService : IPageInteractor
{
    private IItunesAdapter itunes = null!;
    private ILogger logger = null!;
    private IPodcastRepository repository = null!;

    public async Task Execute(Page page)
    {
        try
        {
            var codes = await itunes.PodcastsFromPage(page);
            var applePodcasts = await itunes.GetPodcasts(codes);
            var podcasts = applePodcasts
                .Where(x => !string.IsNullOrEmpty(x.FeedUrl))
                .Select(Podcast.FromApple)
                .ToArray();
            var (total, newPodcasts, updated) = await repository.Upsert(podcasts);

            if (newPodcasts + updated == 0) return;

            logger.Information("{Genre} - {Char} - {Page} - Total podcasts: {Total} - " +
                               "new: {NewPodcasts} - updated: {UpdatedPodcasts}",
                page.Letter.Genre, page.Letter.Char, page.Number, total, newPodcasts, updated);
        }
        catch (Exception e)
        {
            logger.Error(e, "error in {Page}", page);
        }
    }

    public void SetItunes(IItunesAdapter itunes)
    {
        this.itunes = itunes;
    }

    public void SetRepository(IPodcastRepository repository)
    {
        this.repository = repository;
    }

    public void SetLogger(ILogger logger)
    {
        this.logger = logger;
    }
}