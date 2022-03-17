using MongoDB.Bson.Serialization.Attributes;
using PodcastManager.ItunesCrawler.Models;
using Podcast = PodcastManager.ItunesCrawler.Models.Podcast;

namespace PodcastManager.ItunesCrawler.CrossCutting.Mongo.Data;

[BsonIgnoreExtraElements]
public record PodcastData(
    
    ImportedPodcast Imported,
    int Code,
    string Title,
    string Feed,
    string Image,
    bool IsPublished = false)
{
    public static PodcastData FromPodcast(Podcast podcast) =>
        new(new ImportedPodcast(podcast.Imported), podcast.Code, podcast.Title,
            podcast.Feed, podcast.Image, podcast.IsPublished);
}

