using Modules.YandexGames.Scripts;
using UnityEngine;
using YG;

namespace Modules.Player.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;
        [SerializeField, Range(1, 10)] private float _cameraSpeed;
        [SerializeField] private CutsceneController _cutsceneController;

        private Camera _camera;
        private PlayerCarryHandler _playerCarryHandler;
        private PlayerWallet _playerWallet;
        private bool _isAnyCutsceneGoing = false;

        private void Awake() 
        { 
            _camera = GetComponent<Camera>();
        }

        internal void Init(PlayerCarryHandler playerCarryHandler, PlayerWallet playerWallet, bool isDesktop)
        {
            _playerCarryHandler = playerCarryHandler;
            _playerWallet = playerWallet;

            if (isDesktop == false)
                _camera.fieldOfView = _camera.fieldOfView / 2;

            MovementHandler();

            _cutsceneController.Init(_camera, SetDefaultState);

            if (SaveSystem.Instance.HasSaves() == false) 
            {
                StopCameraMoving();
                _cutsceneController.StartOpenCutscene();

                _playerCarryHandler.OnPutItem += StartPayForOrderCutscene;
                _playerWallet.OnCashAmountChanged += StartBuyNextStageCutscene;
            }
        }

        private void StartBuyNextStageCutscene()
        {
            if(_playerWallet.CashAmount % 100 != 0)
            {
                StopCameraMoving();
                _cutsceneController.StartBuyNextStageCutscene(BuyNextStageCutsceneEndCallback);
            }
        }

        private void StartPayForOrderCutscene()
        {
            StopCameraMoving();
            _cutsceneController.StartPayForOrderCutscene(StartPayForOrderCutsceneEndCallback);
        }

        private void Update()
        {
            //if (Input.anyKey && _isAnyCutsceneGoing)
            //    _cutsceneController.StopOpenCutscene();

            if(_isAnyCutsceneGoing == false)
                MovementHandler();
        }

        private void BuyNextStageCutsceneEndCallback() 
        {
            SetDefaultState();
            _playerWallet.OnCashAmountChanged -= StartBuyNextStageCutscene;
        }
        
        private void StartPayForOrderCutsceneEndCallback() 
        {
            SetDefaultState();
            _playerCarryHandler.OnPutItem -= StartPayForOrderCutscene;
        }

        private void SetDefaultState() => _isAnyCutsceneGoing = false;
        private void StopCameraMoving() => _isAnyCutsceneGoing = true;

        private void MovementHandler()
        {
            transform.position = Vector3.Lerp(transform.position, _target.position + _offset, _cameraSpeed * Time.deltaTime);
        }
    }
}
