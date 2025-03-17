using System.Linq;
using Modules.UI;
using UnityEngine;
using Zenject;
using Game.Presenters;
using Game.Scripts.Views;

namespace Game.Views
{
    public sealed class ViewsInstaller : MonoInstaller
    {
        [SerializeField] private MoneyView _moneyView;
        [SerializeField] private Transform _moneyIconTransform;

        public override void InstallBindings()
        {
            PlanetView[] planetViews = FindObjectsOfType<PlanetView>().ToArray();
            IPlanetPresenterCollection planetPresenterCollection = Container.Resolve<IPlanetPresenterCollection>();
            Vector3 moneyIconPosition = _moneyIconTransform.position;
            ParticleAnimator particleAnimator = FindObjectOfType<ParticleAnimator>();
            
            Container
                .Bind<PlanetViewCollection>()
                .FromNew()
                .AsSingle()
                .WithArguments(planetPresenterCollection, planetViews, particleAnimator, moneyIconPosition)
                .NonLazy();

            Container.BindInstance(_moneyView).AsSingle().NonLazy();
            Container.Bind<GameScreenView>().FromComponentInHierarchy().AsSingle().NonLazy();
            // Container.Bind<Vector3>().FromMethod(() => _moneyView.GetMoneyIconPosition()).WhenInjectedInto<PlanetView>();
            // Container.Bind<Vector3>().FromMethod(_ => _moneyIconTransform.position).AsSingle().WhenInjectedInto<PlanetViewCollection>().NonLazy();
            // Container.Bind<ParticleAnimator>().FromComponentInHierarchy().AsSingle().NonLazy();
            // Container.Bind<MoneyView>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}