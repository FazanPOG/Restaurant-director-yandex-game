using Modules.Player.Scripts;
using Modules.YandexGames.Scripts;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI.Scripts
{
    public class ConfirmAdViewUI : MonoBehaviour
    {
        private const string CONFIRM_TEXT_KEY = "WatchTheVideo";

        [SerializeField] private TextMeshProUGUI _confirmText;
        [SerializeField] private TextMeshProUGUI _bonusText;
        [SerializeField] private Image _itemImage;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _watchButton;
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private MovementGuideUI _movementGuideUI;

        public static ConfirmAdViewUI Instance { get; private set; }
        public Button WatchButton => _watchButton;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                throw new Exception("Instance already excist");
        }

        private void Start()
        {
            _confirmText.text = LocalizationController.Instance.GetByKey(CONFIRM_TEXT_KEY) + "?";

            _closeButton.onClick.AddListener(() => 
            {
                gameObject.SetActive(false);
            });

            gameObject.SetActive(false);
        }

        public void Open(Sprite sprite, string bonusText = null) 
        {
            gameObject.SetActive(true);
            _itemImage.sprite = sprite;
            _itemImage.rectTransform.anchoredPosition = Vector2.zero;

            if (bonusText != null) 
            {
                _itemImage.rectTransform.anchoredPosition = new Vector2(-75, _itemImage.rectTransform.anchoredPosition.y);
                _bonusText.gameObject.SetActive(true);
                _bonusText.text = bonusText;
            }
        }
        
        public void Watch(Action rewardedCallback) 
        {
            AdManager.Instance.ShowRewardedAd(rewardedCallback);
        }

        private void OnDisable()
        {
            _watchButton.onClick.RemoveAllListeners();
            _playerInputHandler.EnableJoystick();
            _movementGuideUI.CanShow = true;
        }

        private void OnEnable()
        {
            _bonusText.gameObject.SetActive(false);
            _movementGuideUI.CanShow = false;
            _playerInputHandler.DisableJoystick();
        }
    }
}