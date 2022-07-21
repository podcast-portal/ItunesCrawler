namespace PodcastManager.ItunesCrawler.Messages;

// { "id": 1303, "name": "Comedy" }
public record AppleGenre(int Id, string Name)
{
    public override string ToString()
    {
        return $"Genre: {Id} - {Name}";
    }
}