using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PodcastManager.ItunesCrawler.Adapters;
using PodcastManager.ItunesCrawler.CrossCutting.Http.Tests.Doubles.HttpClientFactory;
using PodcastManager.ItunesCrawler.Doubles.HttpClientFactory;
using PodcastManager.ItunesCrawler.Messages;

namespace PodcastManager.ItunesCrawler.CrossCutting.Http.Tests;

public class ItunesAdapterTests
{
    private ItunesAdapter adapter = null!;

    private void CreateAdapter(IHttpClientFactory? httpClientFactory = null)
    {
        adapter = new ItunesAdapter();
        adapter.SetFactory(httpClientFactory ?? new HttpClientFactoryDummy());
    }

    public class WithoutData : ItunesAdapterTests
    {
        [SetUp]
        public void SetUp()
        {
            CreateAdapter();
        }

        [Test]
        public void Constructor_ShouldInheritsFromItunesAdapter()
        {
            adapter.Should().BeAssignableTo<IItunesAdapter>();
        }

        [Test]
        public async Task GetGenres_WithoutDataShouldBeEmpty()
        {
            var result = await adapter.GetGenres();
            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetTotalPages_WithoutDataShouldBeEmpty()
        {
            var result = await adapter.GetTotalPages(new Letter(new AppleGenre(1, "Genre 1"), 'A'));
            result.Should().Be(0);
        }

        [Test]
        public async Task PodcastsFromPage_WithoutDataShouldBeEmpty()
        {
            var result = await adapter.PodcastsFromPage(new Page(new Letter(new AppleGenre(1, "Genre 1"), 'A'), 1));
            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetPodcasts_WithoutDataShouldBeEmpty()
        {
            var result = await adapter.GetPodcasts(new[] {1, 2, 3, 4});
            result.Should().BeEmpty();
        }
    }

    public class WithData : ItunesAdapterTests
    {
        [SetUp]
        public void SetUp()
        {
            CreateAdapter(new HttpClientFactoryStub());
        }

        [Test]
        public void Constructor_ShouldInheritsFromItunesAdapter()
        {
            adapter.Should().BeAssignableTo<IItunesAdapter>();
        }

        [Test]
        public async Task GetGenres_ShouldReturn4Genres()
        {
            var result = await adapter.GetGenres();
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(4);
            result[0].Should().BeEquivalentTo(new AppleGenre(1, "Genre 1"));
            result[1].Should().BeEquivalentTo(new AppleGenre(2, "Genre 2"));
            result[2].Should().BeEquivalentTo(new AppleGenre(3, "Genre 3"));
            result[3].Should().BeEquivalentTo(new AppleGenre(4, "Genre 4"));
        }

        [Test]
        [TestCase(1, "Genre 1", 'A', 4)]
        [TestCase(2, "Genre 2", 'E', 37)]
        public async Task GetTotalPages_ShouldReturnExpectedValue(
            int genreId, string genre, char c, short expected)
        {
            var letter = new Letter(new AppleGenre(genreId, genre), c);
            var result = await adapter.GetTotalPages(letter);
            result.Should().Be(expected);
        }

        [Test]
        public async Task PodcastsFromPage_ShouldReturn20Podcasts()
        {
            var page = new Page(new Letter(new AppleGenre(1, "Genre 1"), 'A'), 1);
            var result = await adapter.PodcastsFromPage(page);
            result.Should().HaveCount(20);
            result.Should().BeEquivalentTo(new[]
            {
                1572253369,
                1183036922,
                1573139700,
                1569069227,
                1516734036,
                1506820910,
                1386268221,
                1583548784,
                1519507475,
                382483375,
                1569041481,
                1570687060,
                1562202386,
                1526576128,
                1555057950,
                1561888717,
                1557799390,
                597373344,
                1567325061,
                1547768827
            });
        }

        [Test]
        public async Task GetPodcasts_ShouldReturns13Podcasts()
        {
            var codes = new[]
            {
                1146613549, 1157592262, 942491627, 1444893065, 1485274451,
                1473538883, 1366367676, 1049710625, 1382253130, 1530449093,
                381816509, 1592153224, 1580793616
            };
            var result = await adapter.GetPodcasts(codes);
            result.Should().HaveCount(13);
        }
    }
}