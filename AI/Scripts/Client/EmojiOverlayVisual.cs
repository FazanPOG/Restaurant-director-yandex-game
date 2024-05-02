using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.AI.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class EmojiOverlayVisual : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _emojis;
        [SerializeField] private Image _emojiImage;

        private void Start() => Hide();

        internal void UpdateVisual() 
        {
            Show();
            Sprite randomEmoji = _emojis[Random.Range(0, _emojis.Count)];
            _emojiImage.sprite = randomEmoji;
        }

        internal void OnAnimationEnded() => Hide();

        private void Hide() => gameObject.SetActive(false);
        private void Show() => gameObject.SetActive(true);
    }
}
