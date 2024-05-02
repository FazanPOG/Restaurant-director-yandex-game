using Modules.Common.Scripts.Interfaces;
using Modules.Player.Scripts;
using System;
using UnityEngine;

namespace Modules.UI.Scripts
{
    public sealed class ItemInteractUI : BaseInteractUI
    {
        private IHasItems _iHasItems;

        public override event Action<ICarry> OnInteracted;

        private void Awake()
        {
            _iHasItems = GetComponentInParent<IHasItems>();

            if (_iHasItems == null)
                throw new MissingReferenceException("Missing child: IHasItems");
        }

        private void OnEnable()
        {
            _iHasItems.OnItemCountChanged += IhasItems_OnItemCountChanged;

            _interactVisual.OnImageFilled += InteractVisual_OnImageFilled;
            _interactVisual.OnImageFilled -= base.InteractVisual_OnImageFilled;

            _playerPlace.OnEnterInPlace += PlayerPlace_OnEnterInPlace;
            _playerPlace.OnEnterInPlace -= base.PlayerPlace_OnEnterInPlace;

            _playerPlace.OnExitInPlace += base.PlayerPlace_OnExitInPlace;
        }

        private void IhasItems_OnItemCountChanged()
        {
            if (_iHasItems.ItemCount > 0 && ICarry != null && ICarry.IsCanTakeItem())
            {
                _interactVisual.Show();
            }
            else
            {
                _interactVisual.Hide();
            }
        }

        protected override void PlayerPlace_OnEnterInPlace(GameObject obj)
        {
            if(obj.TryGetComponent<ICarry>(out ICarry iCarry)) 
            {
                ICarry = iCarry;

                if (_iHasItems.ItemCount > 0 && ICarry != null && ICarry.IsCanTakeItem())
                {
                    _interactVisual.EnterInteraction();
                }
            }
        }

        protected override void InteractVisual_OnImageFilled()
        {
            if (ICarry.IsCanTakeItem())
            {
                OnInteracted?.Invoke(ICarry);
            }
        }

        private void OnDisable()
        {
            _iHasItems.OnItemCountChanged -= IhasItems_OnItemCountChanged;
            _interactVisual.OnImageFilled -= InteractVisual_OnImageFilled;
            _playerPlace.OnEnterInPlace -= PlayerPlace_OnEnterInPlace;
            _playerPlace.OnExitInPlace -= base.PlayerPlace_OnExitInPlace;
        }
    }
}
