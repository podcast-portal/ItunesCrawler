using PodcastManager.ItunesCrawler.Messages;

namespace PodcastManager.ItunesCrawler.Domain.Interactors;

public interface ILetterInteractor
{
    Task Execute(Letter letter);
}