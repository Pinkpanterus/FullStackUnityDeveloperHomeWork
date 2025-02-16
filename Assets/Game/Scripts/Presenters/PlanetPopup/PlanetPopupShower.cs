using Modules.Planets;
using UnityEngine;

namespace Game.Presenters
{
    public class PlanetPopupShower
    {
        private PlanetPopupPresenter _planetPopupPresenter;
        private PlanetPopup _planetPopup;

        public PlanetPopupShower(PlanetPopupPresenter planetPopupPresenter, PlanetPopup planetPopup)
        {
            _planetPopupPresenter = planetPopupPresenter;
            _planetPopup = planetPopup;
        }

        public void Show(IPlanet planet)
        {
            Debug.Log($"PlanetPopupShower called for {planet.Name}");
            _planetPopupPresenter.ChangePlanet(planet);
            _planetPopup.Show();
        }
    }
}