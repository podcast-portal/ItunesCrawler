namespace PodcastManager.ItunesCrawler.Messages;

public record Page(Letter Letter, int Number)
{
    public override string ToString()
    {
        return $"Page of {Letter} - page {Number}";
    }
}