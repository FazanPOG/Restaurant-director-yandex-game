using Modules.Common.Scripts.Interfaces;
using Modules.UI.Scripts;
using UnityEngine;

namespace Modules.Tables.Scripts
{
    public class Trash : MonoBehaviour, IInteractable
    {
        [SerializeField] private AudioSource _audioSource;

        private InteractUI _interactUI;

        public BaseInteractUI BaseInteractUI => _interactUI;

        private void Awake()
        {
            _interactUI = GetComponentInChildren<InteractUI>();

            if (_interactUI == null)
                throw new MissingReferenceException("Missing child: InteractUI");
        }

        private void OnEnable()
        {
            _interactUI.OnInteracted += InteractUI_OnInteracted;
        }

        private void InteractUI_OnInteracted(ICarry iCarry)
        {
            if (iCarry.ItemCarryList.Count > 0)
                AudioPlayer.Instance.PlayTrashSound(_audioSource);

            iCarry.ClearHand();
        }

        private void OnBecameVisible() => _audioSource.mute = false;

        private void OnBecameInvisible() => _audioSource.mute = true;

        private void OnDisable()
        {
            _interactUI.OnInteracted += InteractUI_OnInteracted;
        }
    }
}
