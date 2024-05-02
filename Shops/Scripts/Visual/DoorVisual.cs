using UnityEngine;

namespace Modules.Shops.Scripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider))]
    public class DoorVisual : MonoBehaviour
    {
        private const string IsOpen = nameof(IsOpen);

        private Animator _animator;
        private int _objectsInsideTrigger = 0;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            _objectsInsideTrigger++;
            _animator.SetBool(IsOpen, true);
        }

        private void OnTriggerExit(Collider other)
        {
            _objectsInsideTrigger--;

            if (_objectsInsideTrigger == 0)
                _animator.SetBool(IsOpen, false);
        }
    }
}
