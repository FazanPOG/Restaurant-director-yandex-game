using Modules.Player.Scripts;
using Modules.UI.Scripts;
using Modules.YandexGames.Scripts;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using YG;

public class AdManager : MonoBehaviour
{
    private const string TEXT_KEY = "AdAfter";

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private SettingsUI _settingsUI;
    [SerializeField] private AdBonusRewardUI _bonusRewardUI;
    [SerializeField] private MovementGuideUI _movementGuideUI;

    public static AdManager Instance { get; private set; }

    private float _timer = 0f;
    private float _timerMax = 65f;
    private string _timerTextLocalize;

    private Action _onRewardedAdCallback;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            throw new MissingReferenceException("More then one AdManager instance");

        YandexGame.RewardVideoEvent += Reward;
    }

    private void Start()
    {
        _timerTextLocalize = LocalizationController.Instance.GetByKey(TEXT_KEY) + ": ";
        _timerText.enabled = false;
    }

    private void Update()
    {
        if(_timer <= _timerMax) 
        {
            _timer += Time.unscaledDeltaTime;

            if(_timerMax - _timer <= 5f) 
            {
                DisableModules();
                _timerText.enabled = true;
                float remainingTime = Mathf.RoundToInt(_timerMax - _timer);
                _timerText.text = _timerTextLocalize + remainingTime.ToString();
            }
        }
        else 
        {
            EnableModules();
            _timerText.enabled = false;

            Debug.Log("-----Interstitial AD-----");
#if UNITY_WEBGL && !UNITY_EDITOR
            YandexGame.FullscreenShow();
#endif

            _timer = 0f;
        }
    }

    public void ShowRewardedAd(Action onRewardedCallback)
    {
        _onRewardedAdCallback = onRewardedCallback;

        Debug.Log("-----REWARD AD-----");
#if UNITY_WEBGL && !UNITY_EDITOR
        YandexGame.RewVideoShow(0);
#endif
    }

    private void Reward(int index) 
    {
        _onRewardedAdCallback?.Invoke();
    }

    public void DisableModules() 
    {
        Time.timeScale = 0f;
        _playerInputHandler.DisableJoystick();
        _settingsUI.DisableInteract();
        _bonusRewardUI.DisableInteract();
        _movementGuideUI.CanShow = false;
    }
    
    public void EnableModules() 
    {
        Time.timeScale = 1f;
        _playerInputHandler.EnableJoystick();
        _settingsUI.EnableInteract();
        _bonusRewardUI.EnableInteract();
        _movementGuideUI.CanShow = true;
    }
}
