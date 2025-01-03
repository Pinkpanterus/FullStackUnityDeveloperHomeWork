using Modules;
using Zenject;

public sealed class InputInstaller : Installer<InputInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SnakeController>().FromMethod(ctx =>
        {
            ISnake snake = Container.Resolve<ISnake>();
            SnakeController snakeController = new SnakeController(snake);
            return snakeController;
        }).AsSingle();
    }
}
