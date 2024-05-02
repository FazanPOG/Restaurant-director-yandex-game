using Modules.Common.Scripts;
using Modules.YandexGames.Enums;
using System;
using System.Collections;
using UnityEngine;
using YG;

namespace Modules.YandexGames.Scripts
{
    public class LocalizationController : MonoBehaviour
    {
        private LanguageType _currentLanguage;
        private ILocalizationAsset _currentAsset;

        public static LocalizationController Instance { get; private set; }

        public event Action LanguageUpdated;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            Debug.Log(YandexGame.EnvironmentData.browserLang);
            SetLanguage(LanguageType.Russian);

#if UNITY_WEBGL && !UNITY_EDITOR
            //var lang = YandexGamesSdk.Environment.i18n.lang;
            var lang = YandexGame.EnvironmentData.browserLang;

            switch (lang)
            {
                case ("en"):
                    SetLanguage(LanguageType.English);
                    break;
            }
#endif
        }
        public void SetLanguage(LanguageType type)
        {
            if (_currentLanguage == type)
                return;

            _currentLanguage = type;

            switch (_currentLanguage)
            {
                case LanguageType.Russian:
                    _currentAsset = new RussianLocalizationAsset();
                    break;
                case LanguageType.English:
                    _currentAsset = new EnglishLocalizationAsset();
                    break;
            }

            LanguageUpdated?.Invoke();
        }

        public string GetByKey(string key)
        {
            return _currentAsset.GetByKey(key);
        }
    }
}
