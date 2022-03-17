namespace PodcastManager.ItunesCrawler.Messages;

public record AppleGenre(int Id, string Name)
{
    public override string ToString()
    {
        return $"Genre: {Id} - {Name}";
    }
}