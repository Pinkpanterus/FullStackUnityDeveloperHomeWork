using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MoneyWidget
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _moneyText;
        
        [SerializeField]
        private Transform _moneyIconTransform;

        public Vector3 GetMoneyIconPosition()
        {
            return _moneyIconTransform.position;
        }

        public void SetMoneyText(string text)
        {
            _moneyText.text = text;
        }    
    }
}
