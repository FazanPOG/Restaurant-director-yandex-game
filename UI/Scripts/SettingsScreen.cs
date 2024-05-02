using Modules.YandexGames.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI.Scripts
{
    public class SettingsScreen : MonoBehaviour
    {
        private const string SETTINGS_TEXT_KEY = "Settings";

        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _musicButton;
        [SerializeField] private Image _soundOffImage;
        [SerializeField] private Image _musicOffImage;
        [SerializeField] private TextMeshProUGUI _settingsText;

        internal void Init(AudioSource audioSource) 
        {
            _soundOffImage.gameObject.SetActive(false);
            _musicOffImage.gameObject.SetActive(false);
            _settingsText.text = LocalizationController.Instance.GetByKey(SETTINGS_TEXT_KEY);

            _soundButton.onClick.AddListener(() => 
            {
                AudioPlayer.Instance.PlayClickSound(audioSource);
            });
            _soundButton.onClick.AddListener(AudioPlayer.Instance.SwitchSoundActiveState);
            _soundButton.onClick.AddListener(SwitchSoundButtonSprite);

            _musicButton.onClick.AddListener(() =>
            {
                AudioPlayer.Instance.PlayClickSound(audioSource);
            });
            _musicButton.onClick.AddListener(AudioPlayer.Instance.SwitchMusicActiveState);
            _musicButton.onClick.AddListener(SwitchMusicButtonSprite);
        }

        private void SwitchSoundButtonSprite() 
        {
            _soundOffImage.gameObject.SetActive(!_soundOffImage.IsActive());
        }
        
        private void SwitchMusicButtonSprite() 
        {
            _musicOffImage.gameObject.SetActive(!_musicOffImage.IsActive());
        }
    }
}
