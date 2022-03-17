namespace PodcastManager.ItunesCrawler.Messages;

public record Letter(AppleGenre Genre, char Char)
{
    public override string ToString()
    {
        return $"Letter with {Genre} '{Char}'";
    }
}