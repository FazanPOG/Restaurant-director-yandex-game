using Modules.Common.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI.Scripts
{
    public class OpenVisual : MonoBehaviour
    {
        [SerializeField] internal Image _filledImage;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _videoImage;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private ParticleSystem _smokeFX;
        [SerializeField] private AudioSource _openAudioSource;
        [SerializeField] private AudioSource _fxAudioSource;

        public ParticleSystem FX => _smokeFX;

        internal void Init(int cost)
        {
            _filledImage.fillAmount = 0f;

            var result = NumberFormatter.FormatNumber(cost);

            if (result.Item2 == string.Empty)
                _costText.text = cost.ToString();
            else 
                _costText.text = $"{result.Item1} {result.Item2}";

            _videoImage.enabled = false;
        }

        internal void SetVideoSprite() 
        {
            _videoImage.enabled = true;
            _costText.enabled = false;
        }
        
        internal void SetDefaultState() 
        {
            _videoImage.enabled = false;
            _costText.enabled = true;
        }

        internal void PlayFX() 
        {
            _smokeFX.Play();
            AudioPlayer.Instance.PlayPufSound(_fxAudioSource);
            AudioPlayer.Instance.PlayOpenObjectSound(_openAudioSource);
        }

        internal void Disable() 
        {
            _filledImage.gameObject.SetActive(false);
            _costText.gameObject.SetActive(false);
            _backgroundImage.gameObject.SetActive(false);
            _videoImage.gameObject.SetActive(false);
        }

        internal void Enable() 
        {
            _filledImage.gameObject.SetActive(true);
            _costText.gameObject.SetActive(true);
            _backgroundImage.gameObject.SetActive(true);
            _videoImage.gameObject.SetActive(true);
        }

    }
}
