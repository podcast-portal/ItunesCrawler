using MongoDB.Driver;
using PodcastManager.ItunesCrawler.CrossCutting.Mongo.Data;
using PodcastManager.ItunesCrawler.Domain.Repositories;
using PodcastManager.ItunesCrawler.Models;

namespace PodcastManager.ItunesCrawler.CrossCutting.Mongo;

public class MongoPodcastRepository : IPodcastRepository
{
    private IMongoDatabase database = null!;

    public async Task<(int total, int newPodcasts, int updated)> Upsert(Podcast[] podcasts)
    {
        var collection = GetCollection<PodcastData>("podcasts");

        var (requests, newPodcasts) = await PrepareRequests();

        if (!requests.Any()) return (0, 0, 0);

        var response = await collection.BulkWriteAsync(requests);

        return (podcasts.Length, newPodcasts, (int) response.ModifiedCount);

        UpdateOneModel<PodcastData> CreateUpdateModel(PodcastData podcast)
        {
            var filter = new FilterDefinitionBuilder<PodcastData>().Eq(x => x.Code, podcast.Code);
            var update = CreateUpdate(podcast);
            return new UpdateOneModel<PodcastData>(filter, update);
        }

        UpdateDefinition<PodcastData> CreateUpdate(PodcastData podcast) =>
            Builders<PodcastData>.Update
                .Set(x => x.Imported.Itunes!.Genres, podcast.Imported.Itunes!.Genres)
                .SetOrUnset(x => x.Imported.Itunes!.ArtistId!, podcast.Imported.Itunes!.ArtistId)
                .Set(x => x.Imported.Itunes!.ArtworkUrl600, podcast.Imported.Itunes!.ArtworkUrl600)
                .SetOrUnset(x => x.Imported.Itunes!.CollectionExplicitness!, podcast.Imported.Itunes!.CollectionExplicitness)
                .Set(x => x.Imported.Itunes!.CollectionId, podcast.Imported.Itunes!.CollectionId)
                .Set(x => x.Imported.Itunes!.CollectionName, podcast.Imported.Itunes!.CollectionName)
                .Set(x => x.Imported.Itunes!.FeedUrl, podcast.Imported.Itunes!.FeedUrl)
                .Set(x => x.Imported.Itunes!.GenreIds, podcast.Imported.Itunes!.GenreIds)
                .SetOrUnset(x => x.Imported.Itunes!.ContentAdvisoryRating!, podcast.Imported.Itunes!.ContentAdvisoryRating)
                .SetOrUnset(x => x.Imported.Itunes!.PrimaryGenreName, podcast.Imported.Itunes!.PrimaryGenreName);

        async Task<(WriteModel<PodcastData>[] requests, int newPodcasts)> PrepareRequests()
        {
            var codes = podcasts
                .Select(PodcastData.FromPodcast)
                .Select(x => x.Code).ToArray();
            
            var existingPodcasts = await GetExistingPodcasts(codes);
            var existingCodes = existingPodcasts
                .Select(x => x.Code)
                .ToList();

            var result = podcasts
                .Select(PodcastData.FromPodcast)
                .Where(x => !existingCodes.Contains(x.Code))
                .Select(x => (WriteModel<PodcastData>) new InsertOneModel<PodcastData>(x))
                .Concat(existingPodcasts.Select(CreateUpdateModel))
                .ToArray();

            return (result, result.Length - existingCodes.Count);
        }
        
        async Task<List<PodcastData>> GetExistingPodcasts(int[] codes)
        {
            var find = collection
                .Find(x => codes.Contains(x.Code));
            var list = await find
                .ToListAsync();
            return list;
        }
    }
    
    public void SetDatabase(IMongoDatabase database) =>
        this.database = database;

    private IMongoCollection<T> GetCollection<T>(string name) =>
        database.GetCollection<T>(name);
}