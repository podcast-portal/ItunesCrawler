using PodcastManager.ItunesCrawler.Messages;

namespace PodcastManager.ItunesCrawler.Domain.Interactors;

public interface IPageInteractor
{
    Task Execute(Page page);
}