namespace PodcastManager.ItunesCrawler.CrossCutting.Http;

public class HttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(string name)
    {
        return new();
    }
}