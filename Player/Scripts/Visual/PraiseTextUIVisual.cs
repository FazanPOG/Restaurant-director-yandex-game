using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Player.Scripts
{
    [RequireComponent(typeof(Animation))]
    public class PraiseTextUIVisual : MonoBehaviour
    {
        [SerializeField] private Image[] _confetties;
        [SerializeField] private TextMeshProUGUI _praiseText;

        private Animation _animation;
        private List<string> _praises;

        private void Awake() => _animation = GetComponent<Animation>();

        internal void Init(List<string> praises) 
        {
            _praises = praises;

            Disable();
        }

        private void OnAnimationEnded() => Disable();

        internal void Enable() 
        {
            foreach (var item in _confetties)
                item.enabled = true;
            _praiseText.enabled = true;

            int randomIndex = Random.Range(0, _praises.Count);
            _praiseText.text = _praises[randomIndex];
            _animation.Play();
        }

        internal void Disable()
        {
            _animation.Stop();

            foreach (var item in _confetties)
                item.enabled = false;
            _praiseText.enabled = false;
        }
    }
}
