using System;

namespace Game.Presenters
{
    public interface IMoneyPresenter
    {
        event Action<int, int> OnMoneyAdded;
        event Action<int, int> OnMoneyRemoved;
        string GetCurrentMoney();
    }
}