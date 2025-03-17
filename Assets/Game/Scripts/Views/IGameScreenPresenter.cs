using System;

namespace Game.Views
{
    public interface IGameScreenPresenter
    {
        event Action<bool> OnPopupVisible;
        bool IsPopupVisible { get; set; }
    }
}