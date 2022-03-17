using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PodcastManager.Doubles;
using PodcastManager.ItunesCrawler.Domain.Interactors;
using PodcastManager.ItunesCrawler.Doubles.Adapters.Enqueuer;
using PodcastManager.ItunesCrawler.Doubles.Adapters.Itunes;
using PodcastManager.ItunesCrawler.Messages;

namespace PodcastManager.ItunesCrawler.Application.Services;

public class GenreServiceTests
{
    private ItunesCrawlerEnqueuerSpy itunesCrawlerEnqueuerSpy = null!;
    private ItunesSpy itunesSpy = null!;
    private GenreService service = null!;

    [SetUp]
    public void SetUp()
    {
        CreateService();
    }

    private void CreateService()
    {
        itunesSpy = new ItunesSpy();
        itunesCrawlerEnqueuerSpy = new ItunesCrawlerEnqueuerSpy();

        service = new GenreService();
        service.SetItunes(itunesSpy);
        service.SetEnqueuer(itunesCrawlerEnqueuerSpy);
        service.SetLogger(new LoggerDummy());
    }

    [Test]
    public void Constructor_InheritsFromGenreInteractor()
    {
        service.Should().BeAssignableTo<IGenreInteractor>();
    }

    [Test]
    public async Task Execute_ShouldCallGetGenreFromItunesOnce()
    {
        await service.Execute();
        itunesSpy.ListGenresSpy.ShouldBeCalledOnce();
        itunesCrawlerEnqueuerSpy.EnqueueLetterSpy.ShouldBeCalled(1);
        itunesCrawlerEnqueuerSpy.EnqueueLetterSpy.LastParameter.First()
            .Should().Be(new Letter(new AppleGenre(1, "Genre 1"), 'A'));
        itunesCrawlerEnqueuerSpy.EnqueueLetterSpy.LastParameter.Last()
            .Should().Be(new Letter(new AppleGenre(3, "Genre 3"), '#'));
    }
}