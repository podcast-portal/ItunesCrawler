using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PodcastManager.Doubles;
using PodcastManager.ItunesCrawler.Domain.Interactors;
using PodcastManager.ItunesCrawler.Doubles.Adapters.Enqueuer;
using PodcastManager.ItunesCrawler.Doubles.Adapters.Itunes;
using PodcastManager.ItunesCrawler.Messages;

namespace PodcastManager.ItunesCrawler.Application.Services;

public class LetterServiceTests
{
    private ItunesCrawlerEnqueuerSpy itunesCrawlerEnqueuerSpy = null!;
    private ItunesSpy itunesSpy = null!;
    private LetterService service = null!;

    private void CreateService()
    {
        itunesCrawlerEnqueuerSpy = new ItunesCrawlerEnqueuerSpy();
        itunesSpy = new ItunesSpy();

        service = new LetterService();
        service.SetEnqueuer(itunesCrawlerEnqueuerSpy);
        service.SetItunes(itunesSpy);
        service.SetLogger(new LoggerDummy());
    }

    [SetUp]
    public void SetUp()
    {
        CreateService();
    }

    [Test]
    public void Constructor_ShouldInheritsFromLetterInteractor()
    {
        service.Should().BeAssignableTo<ILetterInteractor>();
    }

    [Test]
    public async Task Execute_ShouldCallGetPagesOnceAndEnqueuePageFourTimes()
    {
        var letter = new Letter(new AppleGenre(1, "Genre 1"), 'A');

        await service.Execute(letter);

        itunesSpy.GetTotalPagesSpy.ShouldBeCalledOnce();
        itunesSpy.GetTotalPagesSpy.LastParameter.Should().Be(letter);
        itunesCrawlerEnqueuerSpy.EnqueuePageSpy.ShouldBeCalled(1);
        itunesCrawlerEnqueuerSpy.EnqueuePageSpy.LastParameter
            .Should().BeEquivalentTo(new[]
            {
                new Page(letter, 1),
                new Page(letter, 2),
                new Page(letter, 3),
                new Page(letter, 4)
            });
    }
}