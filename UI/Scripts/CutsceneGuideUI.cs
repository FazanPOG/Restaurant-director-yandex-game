using Modules.YandexGames.Scripts;
using TMPro;
using UnityEngine;

namespace Modules.UI.Scripts
{
    public class CutsceneGuideUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _guideText;
        [SerializeField] private MovementGuideUI _movementGuideUI;

        public void Enable() 
        { 
            gameObject.SetActive(true);
            _movementGuideUI.CanShow = false;
        }

        public void Disable() 
        {
            _movementGuideUI.CanShow = true;
            gameObject.SetActive(false); 
        }

        public void SetText(string key) => _guideText.text = LocalizationController.Instance.GetByKey(key);
    }
}
