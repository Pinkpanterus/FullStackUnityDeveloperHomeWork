using Game.MoneyWidget;
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
            var planets = Container.ResolveAll<Planet>();
            foreach (var planet in planets)
            {
                Container
                    .Bind<PlanetPresenter>()
                    .AsCached()
                    .WithArguments(planet) 
                    .NonLazy();
            }
            
            PlanetPresenter[] planetPresenters = Container.ResolveAll<PlanetPresenter>().ToArray();
            Container.Bind<PlanetPresenter[]>().FromInstance(planetPresenters).AsCached().NonLazy();
            
            Container.BindInterfacesTo<MoneyPresenter>().AsSingle().NonLazy();
        }
    }
}