using Modules.Shops.Scripts;
using Modules.Tables.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI.Scripts 
{
    public class MarketUI : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SettingsUI _settingsUI;
        [SerializeField] private MovementGuideUI _movementGuideUI;

        private Shop _shop;
        private MarketProduct[] _products;

        internal MarketProduct[] ProductsUI => _products;

        private void Awake()
        {
            _products = GetComponentsInChildren<MarketProduct>();

            if (_products.Length == 0)
                throw new MissingComponentException("Missing children: MarketProductUI");
        }

        public void Init(Market market, Shop shop, bool isDesktop) 
        {
            if (isDesktop == false)
                transform.localScale *= 1.25f;

            _shop = shop;

            _closeButton.onClick.AddListener(() =>
            {
                AudioPlayer.Instance.PlayClickSound(_audioSource);
                _shop.MarketClosed();
                Disable();
            });

            foreach (MarketProduct marketProduct in _products) 
                marketProduct.Init(market, _shop, _audioSource);

            Disable();
        }

        internal void Enable() 
        {
            gameObject.SetActive(true);
            _settingsUI.DisableInteract();
            _movementGuideUI.CanShow = false;
        }

        internal void Disable()
        {
            _settingsUI.EnableInteract();
            _movementGuideUI.CanShow = true;
            gameObject.SetActive(false);
        }
    }
}
