using Modules.YandexGames.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Modules.UI.Scripts
{
    public class MovementGuideVisual : MonoBehaviour
    {
        private const string PRESSWASD_TEXT_KEY = "PressWASD";
        private const string OR_TEXT_KEY = "Or";

        [SerializeField] private Image _endlessImage;
        [SerializeField] private Image _tapImage;
        [SerializeField] private TextMeshProUGUI _guideText;

        internal void Init(bool isDesktop) 
        {
            if (isDesktop == false)
                _guideText.text = string.Empty;
            else
                _guideText.text = LocalizationController.Instance.GetByKey(PRESSWASD_TEXT_KEY) + " " + LocalizationController.Instance.GetByKey(OR_TEXT_KEY);
        }

        internal void Disable() 
        {
            _endlessImage.enabled = false;
            _tapImage.enabled = false;
            _guideText.enabled = false;
        }
        
        internal void Enable() 
        {
            _endlessImage.enabled = true;
            _tapImage.enabled = true;
            _guideText.enabled = true;
        }
    }
}
