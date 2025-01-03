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
            .FromMethod(ctx =>
            {
                Transform parent = _worldBounds.transform;
                DiContainer diContainer = ctx.Container;
                ISnake snake = diContainer.InstantiatePrefabForComponent<ISnake>(_snakePrefab, parent);
                return snake;
            })
            .AsSingle();
    }
}

