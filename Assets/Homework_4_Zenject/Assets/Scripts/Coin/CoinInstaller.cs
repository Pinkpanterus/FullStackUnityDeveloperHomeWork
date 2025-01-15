using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

public class CoinInstaller : Installer<GameObject, CoinInstaller>
{
    [Inject]
    private GameObject _coinPrefab;
    
    [Inject]
    private WorldBounds _parent;
        
    public override void InstallBindings()
    {
        Container
            .BindMemoryPool<Coin, CoinPool>()
            .WithInitialSize(10)
            .ExpandByOneAtATime()
            .FromComponentInNewPrefab(_coinPrefab)
            .UnderTransform(_parent.transform)
            .AsSingle();
            
        Container
            .Bind<ICoinSpawner>()
            .To<CoinPool>()
            .FromResolve();
        
        Container.BindInterfacesAndSelfTo<CoinController>().FromNew().AsSingle();
    }
}
