using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

public class SnakeInstaller : MonoInstaller
{
    [Header("Snake")]
    [SerializeField]
    private Snake _snakePrefab;
    
    [Inject]
    private WorldBounds _worldBounds;
        
    public override void InstallBindings()
    {
        Container
            .Bind<ISnake>()
            .FromComponentInNewPrefab(_snakePrefab)
            .UnderTransform(_worldBounds.transform)
            .AsCached();
        
        Container.BindInterfacesAndSelfTo<SnakeSelfCollidedController>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<SnakeOutOfBoundariesController>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<SnakeGetCoinController>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<SnakeIncreaseSpeedController>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<SnakeExpandController>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<SnakeStopController>().FromNew().AsSingle();
    }
}

