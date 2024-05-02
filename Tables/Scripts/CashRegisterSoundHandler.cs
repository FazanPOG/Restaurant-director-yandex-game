using UnityEngine;

namespace Modules.Tables.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class CashRegisterSoundHandler : MonoBehaviour
    {
        private AudioSource _audioSource;
        private Cash _cash;

        private void Awake() => _audioSource = GetComponent<AudioSource>();

        public void Init(Cash cash) 
        {
            _cash = cash;
            _cash.OnCashAmountChanged += Cash_OnCashAmountChanged;
        }

        private void Cash_OnCashAmountChanged(int cashAmount)
        {
            if (cashAmount == 0)
                AudioPlayer.Instance.PlayCollectMoneySound(_audioSource);
            else
                AudioPlayer.Instance.PlayAddMoneySound(_audioSource);
        }

        private void OnDisable()
        {
            _cash.OnCashAmountChanged -= Cash_OnCashAmountChanged;
        }

        private void OnBecameVisible() => _audioSource.mute = false; 

        private void OnBecameInvisible() => _audioSource.mute = true;
    }
}
