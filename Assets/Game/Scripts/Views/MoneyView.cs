using DG.Tweening;
using Game.Presenters;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyText;

        private IMoneyPresenter _moneyPresenter;
        private float _animationTimeStep = 0.005f;

        [Inject]
        public void Construct(IMoneyPresenter moneyPresenter)
        {
            _moneyPresenter = moneyPresenter;
            _moneyPresenter.OnMoneyAdded += OnMoneyAdded;
            _moneyPresenter.OnMoneyRemoved += OnMoneyRemoved;

            SetMoneyText(_moneyPresenter.GetCurrentMoney());
        }

        private void OnDestroy()
        {
            _moneyPresenter.OnMoneyAdded -= OnMoneyAdded;
            _moneyPresenter.OnMoneyRemoved -= OnMoneyRemoved;
        }

        private void SetMoneyText(string money)
        {
            _moneyText.text = money;
        }

        private void OnMoneyRemoved(int newvalue, int range)
        {
            int oldValue = newvalue + range; // Предполагаем, что newvalue - это значение после удаления, а range - количество удаленных денег
            int repeatCount = 0;

            int loops = range > 500 ? 500 : range; // Ограничиваем количество анимаций до 500

            DOTween.Sequence()
                .AppendCallback(() => SetMoneyText((oldValue - repeatCount).ToString())) // Первое действие выполняется сразу
                .OnStepComplete(() =>
                {
                    repeatCount++;
                    SetMoneyText((oldValue - repeatCount).ToString()); // Обновляем текст после каждого шага
                })
                .AppendInterval(_animationTimeStep)
                .SetLoops(loops, LoopType.Restart) // Повторяем loops раз
                .OnComplete(() =>
                {
                    if (range > 500)
                    {
                        SetMoneyText(newvalue.ToString()); // Устанавливаем финальное значение, если range > 500
                    }
                });
        }


        private void OnMoneyAdded(int newvalue, int range)
        {
            int oldValue = newvalue - range; // Предполагаем, что newvalue - это значение после добавления, а range - количество добавленных денег
            int repeatCount = 0;

            int loops = range > 500 ? 500 : range; // Ограничиваем количество анимаций до 500

            DOTween.Sequence()
                .AppendCallback(() => SetMoneyText((oldValue + repeatCount).ToString())) // Первое действие выполняется сразу
                .OnStepComplete(() =>
                {
                    repeatCount++;
                    SetMoneyText((oldValue + repeatCount).ToString()); // Обновляем текст после каждого шага
                })
                .AppendInterval(_animationTimeStep)
                .SetLoops(loops, LoopType.Restart) // Повторяем loops раз
                .OnComplete(() =>
                {
                    if (range > 500)
                    {
                        SetMoneyText(newvalue.ToString()); // Устанавливаем финальное значение, если range > 500
                    }
                });
        }
    }
}