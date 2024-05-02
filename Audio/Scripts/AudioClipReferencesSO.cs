using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Audio/AudioClipReferences")]
public class AudioClipReferencesSO : ScriptableObject
{
    [Header("Player")]
    [SerializeField] private AudioClip _playerRunClip;
    [SerializeField] private AudioClip _playerTakeItemClip;
    [Header("UI")]
    [SerializeField] private AudioClip _clickClip;
    [SerializeField] private AudioClip _openClip;
    [SerializeField] private AudioClip _pufClip;
    [Header("Tables")]
    [SerializeField] private AudioClip _collectMoneyClip;
    [SerializeField] private AudioClip _addMoneyClip;
    [SerializeField] private AudioClip _trashClip;
    [Header("AI")]
    [SerializeField] private AudioClip _orderComplete;
    [SerializeField] private List<AudioClip> _wowClips;

    public AudioClip PlayerRunClip => _playerRunClip;
    public AudioClip PlayerTakeItemClip => _playerTakeItemClip;
    public AudioClip ClickClip => _clickClip;
    public AudioClip OpenClip => _openClip;
    public AudioClip PufClip => _pufClip;
    public AudioClip CollectMoneyClip => _collectMoneyClip;
    public AudioClip AddMoneyClip => _addMoneyClip;
    public AudioClip TrashClip => _trashClip;
    public AudioClip OrderComplete => _orderComplete;
    public List<AudioClip> WowClips => _wowClips;
}
