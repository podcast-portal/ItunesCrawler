using System.Text;
using Newtonsoft.Json;
using PodcastManager.ItunesCrawler.Domain.Factories;
using PodcastManager.ItunesCrawler.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace PodcastManager.ItunesCrawler.CrossCutting.Rabbit;

public class RabbitItunesCrawlerListenerAdapter
{
    private IInteractorFactory interactorFactory = null!;
    private IConnection connection = null!;
    private IModel channel = null!;
    private ILogger logger = null!;

    public void SetInteractorFactory(IInteractorFactory interactorFactory)
    {
        this.interactorFactory = interactorFactory;
    }

    public void Listen()
    {
        ListenTo<ImportAll>(RabbitConfiguration.ImportAllQueue,
            _ => interactorFactory.CreateGenre().Execute(), 2, false);
        ListenTo<Letter>(RabbitConfiguration.ImportLetterQueue,
            interactorFactory.CreateLetter().Execute, 2, false);
        ListenTo<Page>(RabbitConfiguration.ImportPageQueue,
            interactorFactory.CreatePage().Execute, 2, false);
    }
    
    
    public void SetConnectionFactory(IConnectionFactory connectionFactory)
    {
        connection = connectionFactory.CreateConnection();
        channel = connection.CreateModel();
    }
    public void SetLogger(ILogger logger) => this.logger = logger;

    private void ListenTo<T>(string queue, Func<T, Task> action, ushort prefetch = 30, bool isGlobal = true)
    {
        logger.Information("listening to: {Queue}", queue);
        ConfigureChannel();
        channel.BasicConsume(queue, false, ConfigureConsumer());

        async Task TryProcessMessage(BasicDeliverEventArgs basicDeliverEventArgs)
        {
            try
            {
                await ProcessMessage(basicDeliverEventArgs);
                channel.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
            }
            catch (Exception e)
            {
                logger.Error(e, "Error: '{Error}' processing message {Queue}", e.Message, queue);
                channel.BasicNack(basicDeliverEventArgs.DeliveryTag, false, false);
            }
        }
        async Task ProcessMessage(BasicDeliverEventArgs args)
        {
            var json = Encoding.UTF8.GetString(args.Body.ToArray());
            if (string.IsNullOrEmpty(json)) json = "{}";
            
            var message = JsonConvert.DeserializeObject<T>(json);
            
            logger.Debug("Message Received: {Message}", message);
            await action(message!);
        }
        void ConfigureChannel()
        {
            channel.QueueDeclare(queue, true, false, false);
            channel.BasicQos(0, prefetch, isGlobal);
        }
        EventingBasicConsumer ConfigureConsumer()
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (_, args) => await TryProcessMessage(args);
            return consumer;
        }
    }

    public void Dispose()
    {
        connection.Dispose();
        channel.Dispose();
        GC.SuppressFinalize(this);
    }

}