using Modules.Common.Scripts;
using Modules.UI.Scripts;
using Modules.YandexGames.Scripts;
using UnityEngine;

namespace Modules.Shops.Scripts
{
    public class Bench : MonoBehaviour, IOpenable
    {
        [SerializeField] private GameObject[] _visualGameObjects;

        private OpenUI _openUI;
        private bool _isOpen;

        public OpenUI OpenUI => _openUI;
        public bool IsOpen => _isOpen;

        private void Awake()
        {
            _openUI = GetComponentInChildren<OpenUI>();
        }

        public void Init()
        {
            _isOpen = SaveSystem.Instance.LoadOpenObject(gameObject);

            if (_isOpen == false)
                _openUI.Init();
            else
                _openUI.Disable();
        }

        public void Disable() => HideVisual();

        public void Open() => ShowVisual();

        private void HideVisual()
        {
            foreach (var gameObject in _visualGameObjects)
                gameObject.SetActive(false);
        }

        private void ShowVisual()
        {
            foreach (var gameObject in _visualGameObjects)
                gameObject.SetActive(true);
        }
    }
}
