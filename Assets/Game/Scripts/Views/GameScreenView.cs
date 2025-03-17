using Game.Presenters;
using Game.Views;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Views
{
    public class GameScreenView: MonoBehaviour
    {
        private IGameScreenPresenter _presenter;
        
        [SerializeField] 
        private PlanetPopup _planetPopup;

        [Inject]
        public void Construct(IGameScreenPresenter presenter)
        {
            _presenter = presenter;
        }
        
        private void Start()
        {
            _presenter.OnPopupVisible += ShowPopUp;
        }

        private void OnDisable()
        {
            _presenter.OnPopupVisible -= ShowPopUp;
        }

        private void ShowPopUp(bool isPopUpShown)
        {
            if (isPopUpShown)
                _planetPopup.Show();
            else
                _planetPopup.Hide();
        }
    }
}