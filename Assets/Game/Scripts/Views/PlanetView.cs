using System.Linq;
using DG.Tweening;
using Game.MoneyWidget;
using Game.Presenters;
using Modules.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class PlanetView : MonoBehaviour
{
    private IPlanetPresenter _presenter;
    private PlanetPopupShower _popupShower;
    private ParticleAnimator _particleAnimator;
    private Vector3 _moneyIconPosition;
    private Transform _iconTransform;

    private Image _planetImage;
    private SmartButton _button;
    private GameObject _coin;
    private GameObject _lock;
    private TextMeshProUGUI _priceText;
    private TextMeshProUGUI _timerText;
    private Image _progressbar;

    [Inject]
    public void Construct(PlanetPresenter[] presenters, PlanetPopupShower popupShower, ParticleAnimator particleAnimator, Vector3 moneyIconPosition)
    {
        _iconTransform = transform.Find("Icon");
        _button = _iconTransform.gameObject.GetComponent<SmartButton>();
        _planetImage = _iconTransform.gameObject.GetComponent<Image>();
        _coin = transform.Find("Coin").gameObject;
        _lock = transform.Find("Lock").gameObject;
        _priceText = transform.Find("Price").gameObject.GetComponentInChildren<TextMeshProUGUI>();

        GameObject income = transform.Find("Income").gameObject;
        _timerText = income.GetComponentInChildren<TextMeshProUGUI>();
        _progressbar = income.GetComponentInChildren<Image>();

        Sprite planetIcon = _planetImage.sprite;
        
        _presenter = presenters.First(p => p.GetIcon(true) == planetIcon);
        _popupShower = popupShower;
        _particleAnimator = particleAnimator;
        _moneyIconPosition = moneyIconPosition;
    }

    private void Start()
    {
        _button.OnClick += OnButtonClicked;
        _button.OnHold += OnButtonHold;

        _presenter.OnPlanetUnlocked += PlanetUnlock;
        _presenter.OnPlanetIncomeReady += IncomeReady;
        _presenter.OnPlanetIncomeTimeChanged += PlanetIncomeTimeChange;
        _presenter.OnCoinPressed += AnimateCoinTransfer;

        UpdatePlanetIcon();
        UpdateCoinIcon();
        UpdateIncomeIndicator();
        UpdatePrice();
    }

    private void UpdatePrice()
    {
        string price = _presenter.GetPrice();
        _priceText.text = price;
    }


    private void UpdateIncomeIndicator()
    {
        var incomeProgress = _presenter.IncomeProgress();
        bool needToShow = incomeProgress > 0 & incomeProgress < 1;

        if (_timerText.gameObject.activeSelf != needToShow)
            _timerText.gameObject.SetActive(needToShow);

        if (_progressbar.gameObject.activeSelf != needToShow)
            _progressbar.gameObject.SetActive(needToShow);

        if (_timerText.gameObject.activeSelf)
            _timerText.text = _presenter.IncomeProgressAsString();

        if (_timerText.gameObject.activeSelf)
            _progressbar.fillAmount = incomeProgress;
    }


    private void OnDisable()
    {
        _button.OnClick -= OnButtonClicked;
        _button.OnHold -= OnButtonHold;

        _presenter.OnPlanetUnlocked -= PlanetUnlock;
        _presenter.OnPlanetIncomeReady -= IncomeReady;
        _presenter.OnPlanetIncomeTimeChanged -= PlanetIncomeTimeChange;
        _presenter.OnCoinPressed -= AnimateCoinTransfer;
    }

    private void AnimateCoinTransfer()
    {
        _coin.SetActive(false);
        
        Vector3 from = _coin.gameObject.transform.position;
        _particleAnimator.Emit(from,_moneyIconPosition,1, ()=> _presenter.PlanetIncomeGather());
    }
    

    private void UpdateCoinIcon()
    {
        _coin.SetActive(_presenter.IsIncomeReady());
    }


    private void UpdatePlanetIcon()
    {
        Sprite icon = _presenter.GetCurrentIcon();
        _planetImage.sprite = icon;
    }

    private void OnButtonClicked()
    {
        _presenter.PlanetClick();
    }

    private void OnButtonHold()
    {
        _popupShower.Show(_presenter.Planet);
    }

    private void PlanetUnlock()
    {
        _lock.SetActive(false);
        _planetImage.sprite = _presenter.GetIcon(true);

        _priceText.transform.parent.gameObject.SetActive(false);
    }

    private void IncomeReady(bool ready)
    {
        _coin.SetActive(ready);
        UpdateIncomeIndicator();
    }
 
    private void PlanetIncomeTimeChange(float time)
    {
        UpdateIncomeIndicator();
    }
}