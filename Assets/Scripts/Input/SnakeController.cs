using System;
using Modules;
using UnityEngine.InputSystem;
using Zenject;

public sealed class SnakeController: IInitializable, IDisposable
{
    private ISnake _snake;
    private SnakeControlsActions _snakeControlsActions;
    
    public SnakeController(ISnake snake)
    {
        _snake = snake;
    }

    public void Initialize()
    {
        _snakeControlsActions = new SnakeControlsActions();
        _snakeControlsActions.Movement.Enable();
        
        _snakeControlsActions.Movement.Up.performed += MoveUp;
        _snakeControlsActions.Movement.Down.performed += MoveDown;
        _snakeControlsActions.Movement.Left.performed += MoveLeft;
        _snakeControlsActions.Movement.Right.performed += MoveRight;
    }

    public void Dispose()
    {
        _snakeControlsActions.Movement.Up.performed -= MoveUp;
        _snakeControlsActions.Movement.Down.performed -= MoveDown;
        _snakeControlsActions.Movement.Left.performed -= MoveLeft;
        _snakeControlsActions.Movement.Right.performed -= MoveRight;
        
        _snakeControlsActions.Movement.Disable();
    }

    private void MoveRight(InputAction.CallbackContext obj) => _snake.Turn(SnakeDirection.RIGHT);
    private void MoveLeft(InputAction.CallbackContext obj) => _snake.Turn(SnakeDirection.LEFT);
    private void MoveDown(InputAction.CallbackContext obj) => _snake.Turn(SnakeDirection.DOWN);
    private void MoveUp(InputAction.CallbackContext obj) => _snake.Turn(SnakeDirection.UP);
}
