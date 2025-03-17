namespace Game.Presenters
{
    public interface IPlanetPresenterCollection
    {
        IPlanetPresenter GetPlanetPresenter(string planetName);
    }
}