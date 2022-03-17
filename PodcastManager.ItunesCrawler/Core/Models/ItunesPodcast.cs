namespace PodcastManager.ItunesCrawler.Models;

public record ItunesPodcast(
    int CollectionId,
    string CollectionName,
    string FeedUrl,
    int[] GenreIds,
    string[] Genres,
    string ArtworkUrl600,
    string PrimaryGenreName,
    string? CollectionExplicitness = null,
    string? ContentAdvisoryRating = null,
    int? ArtistId = null
);