using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClipReferencesSO _audioClipReferencesSO;
    [SerializeField] private AudioSource _backgroundSource;

    private AudioListener _audioListener;
    private bool _isSoundMuted = false;

    public static AudioPlayer Instance { get; private set; }

    private void Awake() 
    {
        #region Singlton
        if(Instance == null)
            Instance = this;
        else
            throw new MissingComponentException($"More then one {name} instance");
        #endregion

        _audioListener = GetComponent<AudioListener>();

        if (_audioClipReferencesSO == null)
            throw new MissingComponentException("Missing AudioClipReferencesSO object");

        Mute();
    }

    public void Init() 
    {
        UnMute();
    }

    public void SwitchSoundActiveState() => _isSoundMuted = !_isSoundMuted;
    public void SwitchMusicActiveState() => _backgroundSource.mute = !_backgroundSource.mute;

    public void PlayFootStepSound(AudioSource audioSource, float volume = 0.1f) 
    {
        AudioClip clip = _audioClipReferencesSO.PlayerRunClip;
        PlaySound(audioSource, clip);
    }

    public void PlayCollectMoneySound(AudioSource audioSource, float volume = 0.5f) => PlayOneShotSound(audioSource, _audioClipReferencesSO.CollectMoneyClip, volume);

    public void PlayTakeItemSound(AudioSource audioSource, float volume = 1f) => PlayOneShotSound(audioSource, _audioClipReferencesSO.PlayerTakeItemClip, volume);

    public void PlayAddMoneySound(AudioSource audioSource, float volume = 0.5f) => PlayOneShotSound(audioSource, _audioClipReferencesSO.AddMoneyClip, volume);
    public void PlayTrashSound(AudioSource audioSource, float volume = 0.5f) => PlayOneShotSound(audioSource, _audioClipReferencesSO.TrashClip, volume);

    public void PlayOrderCompleteSound(AudioSource audioSource, float volume = 0.5f) => PlayOneShotSound(audioSource, _audioClipReferencesSO.OrderComplete, volume); 

    public void PlayClickSound(AudioSource audioSource, float volume = 0.25f) => PlaySound(audioSource, _audioClipReferencesSO.ClickClip, volume);
    public void PlayOpenObjectSound(AudioSource audioSource, float volume = 0.25f) => PlaySound(audioSource, _audioClipReferencesSO.OpenClip, volume);
    public void PlayPufSound(AudioSource audioSource, float volume = 0.25f) => PlaySound(audioSource, _audioClipReferencesSO.PufClip, volume);

    public void PlayRandomWowSound(AudioSource audioSource, float volume = 0.25f) => PlayRandomSound(audioSource, _audioClipReferencesSO.WowClips, volume);

    private void PlaySound(AudioSource audioSource, AudioClip clip, float volume = 0.25f)
    {
        if(_isSoundMuted == false) 
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
        }
    }

    private void PlayOneShotSound(AudioSource audioSource, AudioClip clip, float volume = 0.5f) 
    {
        if (_isSoundMuted == false)
        {
            audioSource.volume = volume;
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayRandomSound(AudioSource audioSource, List<AudioClip> clips, float volume = 0.5f) 
    {
        if (_isSoundMuted == false)
        {
            if (clips.Count == 0)
                throw new MissingReferenceException("No clips in list");

            int randomIndex = Random.Range(0, clips.Count);
            audioSource.clip = clips[randomIndex];
            audioSource.volume = volume;
            audioSource.Play();
        }
    }

    public void Mute() => _audioListener.gameObject.SetActive(false);
    public void UnMute() => _audioListener.gameObject.SetActive(true);
}
