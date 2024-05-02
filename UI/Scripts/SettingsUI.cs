using Modules.Player.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI.Scripts
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private SettingsScreen _screen;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private MovementGuideUI _movementGuideUI;

        internal void Init(PlayerInputHandler playerInputhandler) 
        {
            _openButton.onClick.AddListener(() => 
            {
                ShowSettingsScreen();
                AudioPlayer.Instance.PlayClickSound(_audioSource);
                playerInputhandler.DisableJoystick();
                _movementGuideUI.CanShow = false;
            });

            _closeButton.onClick.AddListener(() =>
            {
                AudioPlayer.Instance.PlayClickSound(_audioSource);
                playerInputhandler.EnableJoystick();
                HideSettingsScreen();
                _movementGuideUI.CanShow = true;
            });

            _screen.Init(_audioSource);

            HideSettingsScreen();
        }

        public void EnableInteract() => _openButton.interactable = true;
        public void DisableInteract() => _openButton.interactable = false;

        private void ShowSettingsScreen() => _screen.gameObject.SetActive(true);
        private void HideSettingsScreen() => _screen.gameObject.SetActive(false);

        private void OnDestroy()
        {
            _openButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}
