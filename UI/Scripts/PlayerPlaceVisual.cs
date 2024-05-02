using UnityEngine;

namespace Modules.UI.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class PlayerPlaceVisual : MonoBehaviour
    {
        private const string IsPlaceTriggered = nameof(IsPlaceTriggered);

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        internal void PlayVisual() => _animator.SetBool(IsPlaceTriggered, true); 

        internal void StopVisual() => _animator.SetBool(IsPlaceTriggered, false);
    }
}
