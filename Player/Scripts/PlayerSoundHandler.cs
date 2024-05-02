using System.Collections;
using UnityEngine;

namespace Modules.Player.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(AudioSource))]
    public class PlayerSoundHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource _footStepsAudioSource;
        [SerializeField] private AudioSource _otherAudioSource;

        private PlayerMovement _playerMovement;
        private PlayerCarryHandler _playerCarryHandler;
        
        private bool _isFootStepsPlaying = false;

        private void Awake() => Mute(); 

        internal void Init(PlayerMovement playerMovement, PlayerCarryHandler playerCarryHandler) 
        {
            _playerMovement = playerMovement;
            _playerCarryHandler = playerCarryHandler;

            UnMute();

            _playerCarryHandler.OnTookItem += PlayerCarryHandler_OnTookItem;
        }

        private void PlayerCarryHandler_OnTookItem()
        {
            AudioPlayer.Instance.PlayTakeItemSound(_otherAudioSource);
        }

        private void Update()
        {
            if (_footStepsAudioSource.mute == false)
                HandleRunSound();
        }

        private void HandleRunSound() 
        {
            if (_playerMovement.IsMoving && _isFootStepsPlaying == false) 
            {
                _isFootStepsPlaying = true;
                AudioPlayer.Instance.PlayFootStepSound(_footStepsAudioSource);
                StartCoroutine(WaitForFootStepSound());
            }
            else if(_playerMovement.IsMoving == false)
            {
                _footStepsAudioSource.Stop();
                _isFootStepsPlaying = false;
            }
        }

        private IEnumerator WaitForFootStepSound()
        {
            while (_footStepsAudioSource.isPlaying)
            {
                yield return null;
            }

            _isFootStepsPlaying = false;
        }

        private void Mute() 
        { 
            _footStepsAudioSource.mute = true; 
            _otherAudioSource.mute = true; 
        }

        private void UnMute()
        {
            _footStepsAudioSource.mute = false;
            _otherAudioSource.mute = false;
        }

        private void OnDisable()
        {
            _playerCarryHandler.OnTookItem -= PlayerCarryHandler_OnTookItem;
        }
    }
}
