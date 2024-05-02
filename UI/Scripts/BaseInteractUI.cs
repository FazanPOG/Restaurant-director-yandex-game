using System;
using UnityEngine;
using Modules.Common.Scripts.Interfaces;

namespace Modules.UI.Scripts
{
    public abstract class BaseInteractUI : MonoBehaviour
    {
        [SerializeField] protected InteractVisual _interactVisual;
        [SerializeField] protected PlayerPlace _playerPlace;

        public ICarry ICarry { get; protected set; }
        public GameObject GameObjectInPlayerPlace { get; private set; }
        public Transform AIPoint => _playerPlace.transform;

        public virtual event Action<ICarry> OnInteracted;

        private void OnEnable()
        {
            _interactVisual.OnImageFilled += InteractVisual_OnImageFilled;
            _playerPlace.OnEnterInPlace += PlayerPlace_OnEnterInPlace;
            _playerPlace.OnExitInPlace += PlayerPlace_OnExitInPlace;
        }

        protected virtual void PlayerPlace_OnEnterInPlace(GameObject obj)
        {
            GameObjectInPlayerPlace = obj;

            if (obj.TryGetComponent<ICarry>(out ICarry iCarry))
            {
                ICarry = iCarry;
                _interactVisual.EnterInteraction();
            }
        }

        protected virtual void PlayerPlace_OnExitInPlace(GameObject obj)
        {
            GameObjectInPlayerPlace = null;

            if (obj.TryGetComponent<ICarry>(out ICarry iCarry))
            {
                ICarry = null;
                _interactVisual.ExitInteraction();
            }
        }

        protected virtual void InteractVisual_OnImageFilled()
        {
            OnInteracted?.Invoke(ICarry);
        }

        private void OnDisable()
        {
            _interactVisual.OnImageFilled -= InteractVisual_OnImageFilled;
            _playerPlace.OnEnterInPlace -= PlayerPlace_OnEnterInPlace;
            _playerPlace.OnExitInPlace -= PlayerPlace_OnExitInPlace;
        }
    }
}
