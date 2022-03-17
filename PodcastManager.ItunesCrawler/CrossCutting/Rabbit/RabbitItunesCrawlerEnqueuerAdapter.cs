using System.Text;
using Newtonsoft.Json;
using PodcastManager.ItunesCrawler.Adapters;
using PodcastManager.ItunesCrawler.Messages;
using RabbitMQ.Client;

namespace PodcastManager.ItunesCrawler.CrossCutting.Rabbit;

public class RabbitItunesCrawlerEnqueuerAdapter : IItunesCrawlerEnqueuerAdapter
{
    private IConnection connection = null!;

    public void SetConnection(IConnection connection) =>
        this.connection = connection;

    public void Dispose()
    {
        connection.Dispose();
        GC.SuppressFinalize(this);
    }

    private void BatchPublish<T>(string queue, IEnumerable<T> messages)
    {
        var (channel, properties) = CreateChannel(queue);
        using (channel)
        {
            var bodies = messages
                .Select(x => new ReadOnlyMemory<byte>(PrepareBody(x)));
            var batch = channel.CreateBasicPublishBatch();
            foreach (var body in bodies)
                batch.Add(string.Empty, queue, true, properties, body);
            batch.Publish();
        }

    }

    private void Publish<T>(string queue, T message)
    {
        var (channel, properties) = CreateChannel(queue);
        using (channel)
        {
            var body = PrepareBody(message);
            channel.
                BasicPublish(string.Empty, queue, properties, body);
        }
    }
    private (IModel, IBasicProperties) CreateChannel(string queue)
    {
        var model = connection.CreateModel();
        model.QueueDeclare(queue, true, false, false);
            
        var basicProperties = model.CreateBasicProperties();
        basicProperties.Persistent = true;
            
        return (model, basicProperties);
    }

    private static byte[] PrepareBody<T>(T message)
    {
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        return body;
    }
    public void EnqueueLetter(IEnumerable<Letter> letters) =>
        BatchPublish(RabbitConfiguration.ImportLetterQueue, letters);

    public void EnqueuePage(IEnumerable<Page> pages) =>
        BatchPublish(RabbitConfiguration.ImportPageQueue, pages);

    public void EnqueueStart() =>
        Publish(RabbitConfiguration.ImportAllQueue, new ImportAll());
}