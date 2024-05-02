using UnityEngine;
using Modules.Player.Scripts;
using Modules.UI.Scripts;
using Modules.Shops.Scripts;
using Modules.AI.Scripts;
using System.Linq;
using Modules.YandexGames.Scripts;

namespace Modules.Root.Scripts
{
    public class GameRoot : MonoBehaviour
    {
        [Header("PlayerRoot")]
        [Space(5)]
        [SerializeField] private PlayerRoot _playerRoot;

        [Header("UI")]
        [Space(5)]
        [SerializeField] private UIRoot _uiRoot;

        [Header("Shops")]
        [Space(5)]
        [SerializeField] private Shop[] _shops;

        [Header("Environment")]
        [Space(5)]
        [SerializeField] private Shops.Scripts.Environment _environment;

        [Header("AI")]
        [Space(5)]
        [SerializeField] private AIRoot _aiRoot;

        [Header("Audio")]
        [Space(5)]
        [SerializeField] private AudioPlayer _audioPlayer;

        private void Start()
        {
            if(IsAllShopsAddedFromShops() == false)
                throw new MissingReferenceException("Not all shops have been added");
            if(_playerRoot == null || _uiRoot == null || _aiRoot == null || _audioPlayer == null)
                throw new MissingReferenceException("Not all SerializeField have been added");

            InitAllModules();
        }

        private void InitAllModules() 
        {
            bool isDesktop = true;

#if UNITY_WEBGL && !UNITY_EDITOR
            DeviceHandler deviceHandler = new DeviceHandler();
            isDesktop = deviceHandler.IsDesktop();
#endif
            _audioPlayer.Init();
            InitShops(isDesktop);
            _playerRoot.Init(_shops, isDesktop);
            _aiRoot.Init(_shops);
            _uiRoot.Init(_playerRoot, isDesktop);
            _environment.Init();
        }

        private void InitShops(bool isDesktop) 
        {
            foreach (Shop shop in _shops) 
                shop.Init(isDesktop);
        }

        private bool IsAllShopsAddedFromShops() 
        {
            Shop[] allShops = FindObjectsOfType<Shop>();
            bool hasDuplicates = allShops.GroupBy(x => x).Any(g => g.Count() > 1);

            if(hasDuplicates == true)
                return false;

            return allShops.Length == _shops.Length;
        }
    }
}