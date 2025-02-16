using System;
using DG.Tweening;
using Modules.Money;
using UnityEngine;
using Zenject;

namespace Game.MoneyWidget
{
    public class MoneyPresenter : IInitializable, IDisposable
    {
        private IMoneyStorage _moneyStorage;
        private MoneyView _moneyView;
        private float _cycleDelay = 0.01f; // лучше вынести в конфиг

        [Inject]
        public MoneyPresenter(MoneyView moneyView, IMoneyStorage moneyStorage)
        {
            _moneyView = moneyView;
            _moneyStorage = moneyStorage;
        }

        public void Initialize()
        {
            _moneyStorage.OnMoneyChanged += ChangeMoneyCount;
            _moneyView.SetMoneyText(_moneyStorage.Money.ToString()); 
        }

        public void Dispose()
        {
            _moneyStorage.OnMoneyChanged -= ChangeMoneyCount;
        }

        private void SpendMoney(int newvalue, int range)
        {
            // Debug.Log($"newvalue: {newvalue}, range: {range}");
            _moneyView.SetMoneyText(newvalue.ToString());
        }

        private void AddMoney(int newvalue, int range)
        {
            int oldValue = newvalue - range;
            int valueToShow = oldValue;

            DOTween.Sequence()
                .AppendCallback(() =>
                {
                    valueToShow += 1;
                    _moneyView.SetMoneyText(valueToShow.ToString());
                }) // Выполняем действие сразу
                .SetDelay(_cycleDelay) // Задержка между повторениями
                .SetLoops(range, LoopType.Restart); // Повторяем (repeatCount - 1) раз
            // .OnComplete(() => Debug.Log("All actions completed!"));
        }

        private void ChangeMoneyCount(int newvalue, int prevvalue)
        {
            if (newvalue > prevvalue)
                AddMoney(newvalue, newvalue - prevvalue);
            
            if (newvalue < prevvalue)
                SpendMoney(newvalue, prevvalue - newvalue);
        }
    }
}