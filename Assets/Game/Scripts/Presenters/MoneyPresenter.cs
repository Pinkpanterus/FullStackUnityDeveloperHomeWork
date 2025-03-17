using System;
using Modules.Money;

namespace Game.Presenters
{
    public class MoneyPresenter: IDisposable, IMoneyPresenter
    {
        public event Action<int, int> OnMoneyAdded;
        public event Action<int, int> OnMoneyRemoved;
        private IMoneyStorage _moneyStorage;

        public MoneyPresenter(IMoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;
            _moneyStorage.OnMoneyEarned += OnMoneyEarned;
            _moneyStorage.OnMoneySpent += OnMoneySpent;
        }

        public void Dispose()
        {
            _moneyStorage.OnMoneyEarned -= OnMoneyEarned;
            _moneyStorage.OnMoneySpent -= OnMoneySpent;
        }

        private void OnMoneySpent(int newvalue, int range)
        {
            OnMoneyRemoved?.Invoke(newvalue, range);
        }

        private void OnMoneyEarned(int newvalue, int range)
        {
            OnMoneyAdded?.Invoke(newvalue, range);
        }

        public string GetCurrentMoney() => _moneyStorage.Money.ToString();
    }
}