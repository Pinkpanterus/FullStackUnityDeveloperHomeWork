using SnakeGame;
using Zenject;

public class UIInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IGameUI>().To<GameUI>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<EndGamePresenter>().AsSingle();
        Container.BindInterfacesAndSelfTo<ScorePresenter>().AsSingle();
        Container.BindInterfacesAndSelfTo<DifficultyPresenter>().AsSingle();
    }
}
