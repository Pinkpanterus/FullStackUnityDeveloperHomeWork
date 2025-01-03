using SnakeGame;
using Zenject;

public class GameUI_Installer : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IGameUI>().To<GameUI>().FromInstance(FindObjectOfType<GameUI>()).AsSingle();
    }
}
