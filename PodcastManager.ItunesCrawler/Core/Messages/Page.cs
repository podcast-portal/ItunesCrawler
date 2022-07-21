namespace PodcastManager.ItunesCrawler.Messages;

// { "letter": { "genre": { "id": 1303, "name": "Comedy" }, "char": "G" }, "number": 17 }
public record Page(Letter Letter, int Number)
{
    public override string ToString()
    {
        return $"Page of {Letter} - page {Number}";
    }
}