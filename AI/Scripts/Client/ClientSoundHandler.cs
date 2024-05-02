using Modules.AI.Scripts;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
public class ClientSoundHandler : MonoBehaviour
{
    private AudioSource _audioSource;
    private ClientOrderHandler _clientOrderHandler;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;
        Mute();
    }

    internal void Init(ClientOrderHandler clientOrderHandler) 
    {
        _clientOrderHandler = clientOrderHandler;
        UnMute();

        _clientOrderHandler.OnOrderRemoved += ClientOrderHandler_OnOrderRemoved;
        _clientOrderHandler.OnPaidForOrder += ClientOrderHandler_OnPaidForOrder;
    }
    
    private void ClientOrderHandler_OnOrderRemoved() => AudioPlayer.Instance.PlayOrderCompleteSound(_audioSource);
    private void ClientOrderHandler_OnPaidForOrder() => AudioPlayer.Instance.PlayRandomWowSound(_audioSource);

    private void OnBecameVisible() => UnMute();
    private void OnBecameInvisible() => Mute();

    private void Mute() => _audioSource.mute = true;
    private void UnMute() => _audioSource.mute = false;

    private void OnDisable()
    {
        _clientOrderHandler.OnOrderRemoved -= ClientOrderHandler_OnOrderRemoved;
        _clientOrderHandler.OnPaidForOrder -= ClientOrderHandler_OnPaidForOrder;
    }
}
