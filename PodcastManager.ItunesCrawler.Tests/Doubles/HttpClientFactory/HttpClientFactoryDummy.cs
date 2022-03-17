using System.Net;
using System.Net.Http;
using RichardSzalay.MockHttp;

namespace PodcastManager.ItunesCrawler.Doubles.HttpClientFactory;

public class HttpClientFactoryDummy : IHttpClientFactory
{
    public virtual HttpClient CreateClient(string name)
    {
        var mock = new MockHttpMessageHandler();
        mock.When("*")
            .Respond(HttpStatusCode.OK, new StringContent(string.Empty));
        return mock.ToHttpClient();
    }
}