using Modules.Items.Scripts;
using Modules.Root.Scripts;
using UnityEngine;

namespace Modules.Player.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class PlayerVisual : MonoBehaviour
    {
        private const string IsMoving = nameof(IsMoving);
        private const string Speed = nameof(Speed);
        private const string IsCarry = nameof(IsCarry);

        private PlayerMovement _playerMovement;
        private PlayerCarryHandler _playerCarryHandler;
        private PlayerInputHandler _playerInputHandler;
        private Animator _animator;

        internal void Init(PlayerMovement playerMovement, PlayerCarryHandler playerCarryHandler, PlayerInputHandler playerInputHandler)
        {
            _playerMovement = playerMovement;
            _playerCarryHandler = playerCarryHandler;
            _playerInputHandler = playerInputHandler;
            _animator = GetComponent<Animator>();

            _playerCarryHandler.OnTookItem += PlayerCarryHandler_OnTookItem;
            _playerCarryHandler.OnPutItem += PlayerCarryHandler_OnRemovedItem;
        }

        private void PlayerCarryHandler_OnTookItem()
        {
            _animator.SetBool(IsCarry, true);
        }

        private void PlayerCarryHandler_OnRemovedItem()
        {
            if (_playerCarryHandler.ItemCarryList.Count == 0) 
            {
                _animator.SetBool(IsCarry, false);
            }
        }

        private void Update()
        {
            float dir = _playerInputHandler.GetMaxInputDirection();
            _animator.SetFloat(Speed, dir);
            _animator.SetBool(IsMoving, _playerMovement.IsMoving);
        }

        private void OnDisable()
        {
            _playerCarryHandler.OnTookItem -= PlayerCarryHandler_OnTookItem;
            _playerCarryHandler.OnPutItem -= PlayerCarryHandler_OnRemovedItem;
        }
    }
}
