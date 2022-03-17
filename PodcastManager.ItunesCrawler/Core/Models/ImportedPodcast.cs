namespace PodcastManager.ItunesCrawler.Models;

public record ImportedPodcast(
    ItunesPodcast? Itunes = null,
    FeedPodcast? Feed = null
);