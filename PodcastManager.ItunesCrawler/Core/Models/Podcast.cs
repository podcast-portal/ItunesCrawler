namespace PodcastManager.ItunesCrawler.Models;

public record Podcast(ItunesPodcast Imported, int Code, string Title, string Feed, string Image,
    bool IsPublished = false)
{
    public static Podcast FromApple(ItunesPodcast imported)
    {
        return new Podcast(imported, imported.CollectionId, imported.CollectionName,
            imported.FeedUrl, imported.ArtworkUrl600);
    }
}