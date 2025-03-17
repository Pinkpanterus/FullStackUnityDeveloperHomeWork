using System;
using Game.Views;

namespace Game.Presenters
{
    public class GameScreenPresenter : IGameScreenPresenter
    {
        public event Action<bool> OnPopupVisible;
        public bool IsPopupVisible
        {
            get { return _isPopupVisible;}
            set
            {
                if (_isPopupVisible != value)
                {
                    _isPopupVisible = value; 
                    OnPopupVisible?.Invoke(value);
                }
            }}

        private bool _isPopupVisible;
    }
}