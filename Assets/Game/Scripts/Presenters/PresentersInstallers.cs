using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    [CreateAssetMenu(
        fileName = "PresentersInstallers",
        menuName = "Zenject/New PresentersInstallers"
    )]
    public sealed class PresentersInstallers : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            
            Container
                .BindInterfacesAndSelfTo<PlanetPopupPresenter>()
                .FromNew()
                .AsSingle();
            
            var planets = Container.ResolveAll<Planet>().ToArray();
            Container
                .BindInterfacesAndSelfTo<PlanetPresenterCollection>()
                .FromNew()
                .AsSingle()
                .WithArguments(planets)
                .NonLazy();
            
            
            
            Container.BindInterfacesAndSelfTo<GameScreenPresenter>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesTo<MoneyPresenter>().AsSingle().NonLazy();
            
            // Container.BindInterfacesAndSelfTo<PlanetPopupPresenter>().AsCached().NonLazy();
            // Container.Bind<PlanetPresenter[]>().FromInstance(planetPresenters).AsCached().NonLazy();
        }
    }
}