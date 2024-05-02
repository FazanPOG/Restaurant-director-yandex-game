using Modules.Shops.Scripts;
using UnityEngine;

namespace Modules.Player.Scripts
{
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerCarryHandler))]
    [RequireComponent(typeof(PlayerWallet))]
    public class PlayerRoot : MonoBehaviour
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private PlayerVisual _playerVisual;
        [SerializeField] private PlayerSoundHandler _soundHandler;
        [SerializeField] private PraiseTextUI _praiseTextUI;

        private PlayerInputHandler _playerInputHandler;
        private PlayerMovement _playerMovement;
        private PlayerCarryHandler _playerCarryHandler;
        private PlayerWallet _playerWallet;

        private void Awake()
        {
            _playerInputHandler = GetComponent<PlayerInputHandler>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerCarryHandler = GetComponent<PlayerCarryHandler>();
            _playerWallet = GetComponent<PlayerWallet>();
        }

        public void Init(Shop[] shops, bool isDesktop)
        {
            _playerInputHandler.Init(shops, isDesktop);
            _playerMovement.Init(_playerInputHandler);
            _playerCarryHandler.Init();
            _playerVisual.Init(_playerMovement, _playerCarryHandler, _playerInputHandler);
            _playerWallet.Init();
            _soundHandler.Init(_playerMovement, _playerCarryHandler);
            _praiseTextUI.Init(_playerWallet);
            _cameraController.Init(_playerCarryHandler, _playerWallet, isDesktop);
        }
    }
}
