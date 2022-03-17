using PodcastManager.ItunesCrawler.Domain.Interactors;

namespace PodcastManager.ItunesCrawler.Domain.Factories;

public interface IInteractorFactory
{
    IGenreInteractor CreateGenre();
    ILetterInteractor CreateLetter();
    IPageInteractor CreatePage();
}