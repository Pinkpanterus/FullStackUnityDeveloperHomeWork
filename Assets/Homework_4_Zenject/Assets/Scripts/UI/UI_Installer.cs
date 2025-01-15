using SnakeGame;
using Zenject;

public class UI_Installer : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IGameUI>().To<GameUI>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<UI_EndGameController>().AsSingle();
        Container.BindInterfacesAndSelfTo<UI_ScorePresenter>().AsSingle();
        Container.BindInterfacesAndSelfTo<UI_DifficultyPresenter>().AsSingle();
    }
}
