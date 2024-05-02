using Modules.Shops.Scripts;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Player.Scripts
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private FloatingJoystick _joystick;

        private const string Horizontal = nameof(Horizontal);
        private const string Vertical = nameof(Vertical);

        private Shop[] _shops;
        private PlayerInputAction _playerInputActions;
        private InputDevices _inputDevice;
        private Vector2 _movementInput;
        private float _joystickInputScale = 1f / 0.75f;
        public Vector2 MovementInput => _movementInput;

        internal void Init(Shop[] shops, bool isDesktop)
        {
            _movementInput = new Vector2(0, 0);

            _shops = shops;

            _playerInputActions = new PlayerInputAction();
            _inputDevice = InputDevices.Joystick;

            if (isDesktop == false)
                _joystick.gameObject.transform.localScale = new Vector3(_joystick.gameObject.transform.localScale.x * 2, _joystick.gameObject.transform.localScale.y * 2, _joystick.gameObject.transform.localScale.z * 2);

            _playerInputActions.Player.Enable();
            _playerInputActions.Player.Move.performed += Move_performed;
            _playerInputActions.Player.TouchScreen.performed += TouchScreen_performed;

            foreach (var shop in _shops) 
            {
                shop.OnMarketOpened += DisableJoystick;
                shop.OnMarketClosed += EnableJoystick;
            }
                
        }

        private void TouchScreen_performed(InputAction.CallbackContext context)
        {
            _inputDevice = InputDevices.Joystick;
        }

        private void Move_performed(InputAction.CallbackContext context)
        {
            _inputDevice = InputDevices.Keyboard;
        }

        public void DisableJoystick() => _joystick.gameObject.SetActive(false);
        public void EnableJoystick() => _joystick.gameObject.SetActive(true);

        private void Update()
        {
            switch (_inputDevice)
            {
                case InputDevices.Keyboard:
                    _movementInput = new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
                    break;

                case InputDevices.Joystick:
                    _movementInput = new Vector2(Mathf.Clamp(_joystick.Horizontal * _joystickInputScale, -1f, 1f),
                    Mathf.Clamp(_joystick.Vertical * _joystickInputScale, -1f, 1f));
                    break;
            }

        }

        public float GetMaxInputDirection() 
        {
            float maxInputDir = MathF.Max(Mathf.Abs(_movementInput.x), Mathf.Abs(_movementInput.y));
            return maxInputDir;
        }

        private void OnDisable()
        {
            _playerInputActions.Player.Move.performed -= Move_performed;
            _playerInputActions.Player.TouchScreen.performed -= TouchScreen_performed;
            _playerInputActions.Player.Disable();

            foreach (var shop in _shops)
            {
                shop.OnMarketOpened -= DisableJoystick;
                shop.OnMarketClosed -= EnableJoystick;
            }
        }

        private enum InputDevices
        {
            Keyboard,
            Joystick,
        }
    }
}
