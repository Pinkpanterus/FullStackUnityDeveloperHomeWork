using System.Collections.Generic;
using Game.Presenters;
using UnityEngine;

namespace Game.Scripts.Views
{
    public class PlanetCollectionView
    {
        [SerializeField]
        private PlanetView[] _planetViews;
        
        private IPlanetCollectionPresenter _planetCollectionPresenter;
    }

    public interface IPlanetCollectionPresenter
    {
        IReadOnlyCollection<IPlanetPresenter> PlanetPresenters { get; }
    }
}