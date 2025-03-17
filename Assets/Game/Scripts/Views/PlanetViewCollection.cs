using System.Collections.Generic;
using Game.Presenters;
using Modules.UI;
using UnityEngine;

namespace Game.Views
{
    public class PlanetViewCollection
    {
        private Dictionary<string, PlanetView> _planetViews = new Dictionary<string, PlanetView>();

        public PlanetViewCollection(IPlanetPresenterCollection presenterCollection, PlanetView[] planetViews, ParticleAnimator particleAnimator, Vector3 moneyIconPosition)
        {
            foreach (PlanetView planetView in planetViews)
            {
                string planetName = planetView.Name;
                IPlanetPresenter planetPresenter = presenterCollection.GetPlanetPresenter(planetName);
                planetView.Construct(planetPresenter, particleAnimator, moneyIconPosition);
                _planetViews.Add(planetName, planetView);
            }
        }

        public PlanetView GetPlanetView(string planetName) => _planetViews[planetName];
    }
}