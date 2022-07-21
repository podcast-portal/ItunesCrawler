namespace PodcastManager.ItunesCrawler.Messages;

// { "genre": { "id": 1303, "name": "Comedy" }, "char": "G" }
public record Letter(AppleGenre Genre, char Char)
{
    public override string ToString()
    {
        return $"Letter with {Genre} '{Char}'";
    }
}