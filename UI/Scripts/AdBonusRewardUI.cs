using Modules.Player.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI.Scripts
{
    public class AdBonusRewardUI : MonoBehaviour
    {
        private const int MONEY_BONUS_COEFFICIENT = 2;
        private const int DEFAULT_MONEY_BONUS = 500;
        private const float SPEED_BONUS_COEFFICIENT = 1.5f;

        [SerializeField] private Button _moneyBonusButton;
        [SerializeField] private Button _moveSpeedBonusButton;
        [SerializeField] private TextMeshProUGUI _bonusText;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Sprite _moneyBonusSprite;
        [SerializeField] private Sprite _moveSpeedBonusSprite;

        private PlayerWallet _playerWallet;
        private PlayerMovement _playerMovement;
        private int _moneyBonus;

        internal void Init(PlayerWallet wallet, PlayerMovement movement, bool isDesktop) 
        {
            _playerWallet = wallet;
            _playerMovement = movement;

            if (isDesktop == false) 
            {
                transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y * 1.5f, transform.localScale.z * 1.5f);
                _rectTransform.anchoredPosition += new Vector2(-100f, 0f);
            }

            _moneyBonusButton.onClick.AddListener(WatchMoneyBonusReward);
            _moveSpeedBonusButton.onClick.AddListener(WatchMoveSpeedBonusReward);

            if(_bonusText != null) 
            {
                _playerWallet.OnCashAmountChanged += PlayerWallet_OnCashAmountChanged;

                PlayerWallet_OnCashAmountChanged();
            }
        }

        private void PlayerWallet_OnCashAmountChanged()
        {
            if (_playerWallet.CashAmount <= 500)
                _bonusText.text = DEFAULT_MONEY_BONUS.ToString();
            else
                _bonusText.text = "2x";
        }

        private void WatchMoneyBonusReward() 
        {
            _moneyBonus = _playerWallet.CashAmount * MONEY_BONUS_COEFFICIENT;

            if (_playerWallet.CashAmount <= 500) 
                _moneyBonus = DEFAULT_MONEY_BONUS;

            ConfirmAdViewUI.Instance.Open(_moneyBonusSprite, _moneyBonus.ToString());
            ConfirmAdViewUI.Instance.WatchButton.onClick.AddListener(() =>
            {
                ConfirmAdViewUI.Instance.Watch(TakeMoneyBonus);
            });
        }
        
        private void WatchMoveSpeedBonusReward() 
        {
            ConfirmAdViewUI.Instance.Open(_moveSpeedBonusSprite, "x2");
            ConfirmAdViewUI.Instance.WatchButton.onClick.AddListener(() => 
            {
                ConfirmAdViewUI.Instance.Watch(TakeSpeedBonus);
            });
        }

        public void DisableInteract() 
        {
            _moneyBonusButton.interactable = false;
            _moveSpeedBonusButton.interactable = false;
        }
        
        public void EnableInteract() 
        {
            _moneyBonusButton.interactable = true;
            _moveSpeedBonusButton.interactable = true;
        }

        private void TakeMoneyBonus() 
        { 
            _playerWallet.AddCash(_moneyBonus);
            ConfirmAdViewUI.Instance.gameObject.SetActive(false);
        }
        private void TakeSpeedBonus() 
        { 
            _playerMovement.SpeedBonus(SPEED_BONUS_COEFFICIENT);
            ConfirmAdViewUI.Instance.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if (_bonusText != null)
            {
                _playerWallet.OnCashAmountChanged -= PlayerWallet_OnCashAmountChanged;
            }
        }
    }
}
