using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Presenters
{
    public class PlanetPopup : MonoBehaviour
    {
        private IPlanetPopupPresenter _planetPopupPresenter;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _populationText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _incomeText;
        [SerializeField] private TextMeshProUGUI _upgradePriceText;
        [SerializeField] private Image _planetImage;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _upgradeButton;

        [Inject]
        public void Construct(IPlanetPopupPresenter planetPopupPresenter)
        {
            _planetPopupPresenter = planetPopupPresenter;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _closeButton.onClick.RemoveListener(_planetPopupPresenter.Hide);
            _planetPopupPresenter.OnPopulationChanged -= UpdatePopulation;
            _planetPopupPresenter.OnLevelChanged -= UpdateLevel;
            _planetPopupPresenter.OnIncomeChanged -= UpdateIncome;
            
            _upgradeButton.onClick.RemoveListener(UpgradePlanet);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _closeButton.onClick.AddListener(_planetPopupPresenter.Hide);

            _titleText.text = _planetPopupPresenter.PlanetName;
            _planetImage.sprite = _planetPopupPresenter.Icon;
            _levelText.text = _planetPopupPresenter.Level;
            _upgradePriceText.text = _planetPopupPresenter.Price;
            UpdatePopulation(_planetPopupPresenter.Population);
            UpdateIncome(_planetPopupPresenter.MinuteIncome);
            UpdateLevel(_planetPopupPresenter.Level);
            UpdateUpgradeButton();
            _planetPopupPresenter.OnPopulationChanged += UpdatePopulation;
            _planetPopupPresenter.OnLevelChanged += UpdateLevel;
            _planetPopupPresenter.OnIncomeChanged += UpdateIncome;
            
            _upgradeButton.onClick.AddListener(UpgradePlanet);
        }

        private void UpgradePlanet()
        {
            _planetPopupPresenter.UpgradePlanet();
        }

        public void UpdateLevel(string level)
        {
            _levelText.text = level;
            UpdateUpgradeButton();
        }

        private void UpdateUpgradeButton()
        {
            if (_planetPopupPresenter.CanUpgrade)
                _upgradeButton.interactable = true;
            else
                _upgradeButton.interactable = false;
            
            _upgradePriceText.text = _planetPopupPresenter.Price;
        }

        private void UpdateIncome(string income)
        {
            _incomeText.text = income;
        }

        private void UpdatePopulation(string population)
        {
            _populationText.text = population;
        }
    }
}