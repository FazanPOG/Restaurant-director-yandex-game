using Modules.Player.Scripts;
using Modules.Shops.Scripts;
using Modules.UI.Scripts;
using UnityEngine;

namespace Modules.Tables.Scripts
{
    public class Market : MonoBehaviour
    {
        [SerializeField] private MarketUI _marketUI;
        [SerializeField] private PlayerPlace _playerPlace;

        private Shop _shop;
        private PlayerWallet _playerWallet;

        public MarketProduct[] Products => _marketUI.ProductsUI;
        internal PlayerWallet PlayerWallet => _playerWallet;

        private void OnEnable()
        {
            _playerPlace.OnEnterInPlace += PlayerPlace_OnEnterInPlace;
        }

        public void Init(Shop shop, bool isDesktop) 
        {
            _shop = shop;

            _marketUI.Init(this, _shop, isDesktop);
        }

        private void PlayerPlace_OnEnterInPlace(GameObject obj)
        {
            if(obj.TryGetComponent<PlayerWallet>(out PlayerWallet wallet)) 
            {
                _playerWallet = wallet;
                _marketUI.Enable();
                _shop.MarketOpened();
            }
        }

        private void OnDisable()
        {
            _playerPlace.OnEnterInPlace -= PlayerPlace_OnEnterInPlace;
        }

        public enum ProductType
        {
            Cashier,
            Waiter,
            WaiterLevelUp,
            ClientLevelUp,
        }
    }
}
