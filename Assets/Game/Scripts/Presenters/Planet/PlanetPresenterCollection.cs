using System.Collections.Generic;
using Modules.Planets;

namespace Game.Presenters
{
    public class PlanetPresenterCollection : IPlanetPresenterCollection
    {
        private Dictionary<string, PlanetPresenter> _presentersDictionary = new Dictionary<string, PlanetPresenter>();
        private PlanetPopupPresenter _planetPopupPresenter;
        private GameScreenPresenter _gameScreenPresenter;
        
        public PlanetPresenterCollection(Planet[] planets, PlanetPopupPresenter planetPopupPresenter, GameScreenPresenter gameScreenPresenter)
        {
            _planetPopupPresenter = planetPopupPresenter;
            _gameScreenPresenter = gameScreenPresenter;
            
            foreach (Planet planet in planets)
            {
                string planetName = planet.Name.Replace(" #", "");
                PlanetPresenter planetPresenter = new PlanetPresenter(planet, gameScreenPresenter, planetPopupPresenter);
                _presentersDictionary.Add(planetName, planetPresenter);
            }
        }
        
        public IPlanetPresenter GetPlanetPresenter(string planetName) => _presentersDictionary[planetName];
    }
}