using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PodcastManager.Doubles;
using PodcastManager.ItunesCrawler.Domain.Interactors;
using PodcastManager.ItunesCrawler.Doubles.Adapters.Itunes;
using PodcastManager.ItunesCrawler.Doubles.Repositories;
using PodcastManager.ItunesCrawler.Messages;
using PodcastManager.ItunesCrawler.Models;

namespace PodcastManager.ItunesCrawler.Application.Services;

public class PageServiceTests
{
    private ItunesSpy itunesSpy = null!;
    private PodcastRepositorySpy repositorySpy = null!;
    private PageService service = null!;

    private void CreateService()
    {
        itunesSpy = new ItunesSpy();
        repositorySpy = new PodcastRepositorySpy();

        service = new PageService();
        service.SetItunes(itunesSpy);
        service.SetRepository(repositorySpy);
        service.SetLogger(new LoggerDummy());
    }

    [SetUp]
    public void SetUp()
    {
        CreateService();
    }

    [Test]
    public void Constructor_ShouldInheritsFromPageInteractor()
    {
        service.Should().BeAssignableTo<IPageInteractor>();
    }

    [Test]
    public async Task Execute_ShouldCallPodcastsFromPageOncePodcastsOnceAndUpsertOnce()
    {
        var page = new Page(new Letter(new AppleGenre(1, "Genre 1"), 'A'), 2);
        await service.Execute(page);

        itunesSpy.PodcastsFromPageSpy.ShouldBeCalledOnce();
        itunesSpy.PodcastsFromPageSpy.LastParameter.Should().BeEquivalentTo(page);

        itunesSpy.GetPodcastsSpy.ShouldBeCalledOnce();
        itunesSpy.GetPodcastsSpy.LastParameter.Should().BeEquivalentTo(itunesSpy.PodcastCodes);

        repositorySpy.UpsertSpy.ShouldBeCalledOnce();
        repositorySpy.UpsertSpy.LastParameter.Should()
            .BeEquivalentTo(itunesSpy.Podcasts.Select(Podcast.FromApple));
    }
}