using Modules.Common.Scripts;
using Modules.Player.Scripts;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Modules.UI.Scripts
{
    public class MoneyCounterUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyCounterText;

        private PlayerWallet _playerWallet;

        internal void Init(PlayerWallet playerWallet) 
        {
            _playerWallet = playerWallet;
            PlayerWallet_OnCashAmountChanged();

            _playerWallet.OnCashAmountChanged += PlayerWallet_OnCashAmountChanged;
        }

        private void PlayerWallet_OnCashAmountChanged() 
        {
            var result = NumberFormatter.FormatNumber(_playerWallet.CashAmount);

            if (result.Item2 == string.Empty)
                _moneyCounterText.text = _playerWallet.CashAmount.ToString();
            else
                _moneyCounterText.text = $"{result.Item1} {result.Item2}";
        }

        private void OnDisable()
        {
            _playerWallet.OnCashAmountChanged -= PlayerWallet_OnCashAmountChanged;
        }
    }
}