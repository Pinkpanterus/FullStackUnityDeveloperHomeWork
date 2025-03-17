using Game.Presenters;
using Modules.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlanetView : MonoBehaviour
{
    public string Name => gameObject.name;
    private IPlanetPresenter _presenter;
    private ParticleAnimator _particleAnimator;
    private Vector3 _moneyIconPosition;
    
    [SerializeField] private Image _planetImage;
    [SerializeField] private SmartButton _button;
    [SerializeField] private GameObject _coin;
    [SerializeField] private GameObject _lock;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Image _progressbar;

    public void Construct(IPlanetPresenter presenter, ParticleAnimator particleAnimator, Vector3 moneyIconPosition)
    {
        _presenter = presenter;
        _particleAnimator = particleAnimator;
        _moneyIconPosition = moneyIconPosition;
        
        UpdatePlanetIcon();
        UpdateCoinIcon();
        UpdateIncomeIndicator();
        UpdatePrice();
        
        _button.OnClick += OnButtonClicked;
        _button.OnHold += OnButtonHold;

        _presenter.OnPlanetUnlocked += PlanetUnlock;
        _presenter.OnPlanetIncomeReady += IncomeReady;
        _presenter.OnPlanetIncomeTimeChanged += PlanetIncomeTimeChange;
        _presenter.OnCoinPressed += AnimateCoinTransfer;
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

        // if (_timerText.gameObject.activeSelf != needToShow)
        //     _timerText.gameObject.SetActive(needToShow);
        //
        if (_progressbar.transform.parent.gameObject.activeSelf != needToShow)
            _progressbar.transform.parent.gameObject.SetActive(needToShow);

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
        _presenter.PlanetHold();
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