using Modules.YandexGames.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Player.Scripts
{
    public class PraiseTextUI : MonoBehaviour
    {
        private const string PRAISE_TEXT_KEY_COOL = "Cool";
        private const string PRAISE_TEXT_KEY_WOW = "Wow";
        private const string PRAISE_TEXT_KEY_AMAZING = "Amazing";
        private const string PRAISE_TEXT_KEY_SUPER = "Super";
        private const string PRAISE_TEXT_KEY_LEGENDARY = "Legendary";

        [SerializeField] private PraiseTextUIVisual _visual;

        private List<string> _praises;
        private PlayerWallet _wallet;
        private int _cashAmount;

        internal void Init(PlayerWallet playerWallet) 
        {
            _wallet = playerWallet;
            _cashAmount = playerWallet.CashAmount;

            _praises = new List<string>()
            {
                LocalizationController.Instance.GetByKey(PRAISE_TEXT_KEY_COOL),
                LocalizationController.Instance.GetByKey(PRAISE_TEXT_KEY_WOW),
                LocalizationController.Instance.GetByKey(PRAISE_TEXT_KEY_AMAZING),
                LocalizationController.Instance.GetByKey(PRAISE_TEXT_KEY_SUPER),
                LocalizationController.Instance.GetByKey(PRAISE_TEXT_KEY_LEGENDARY),
            };

            _visual.Init(_praises);

            _wallet.OnCashAmountChanged += Wallet_OnCashAmountChanged;
        }

        private void Wallet_OnCashAmountChanged()
        {
            _visual.Disable();

            if (_cashAmount > _wallet.CashAmount)
                _visual.Enable();

            _cashAmount = _wallet.CashAmount;
        }
    }
}
