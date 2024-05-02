using Modules.Common.Scripts;
using Modules.Player.Scripts;
using Modules.Shops.Scripts;
using Modules.Tables.Scripts;
using Modules.YandexGames.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI.Scripts
{
    public class MarketProduct : MonoBehaviour
    {
        [SerializeField] private MarketProductSO _marketProductSO;
        [SerializeField] private MarketProductUIVisual _visual;

        private Market _market;
        private Shop _shop;
        private Button _buyButton;
        private PlayerWallet _wallet;
        private AudioSource _audioSource;
        private int _currentPrice;
        private int _purchaseCount;
        private int _maxProductCount;

        public int PurchaseCount => _purchaseCount;
        public Market.ProductType ProductType => _marketProductSO.ProductType;
        public Shop Shop => _shop;

        public event Action<Shop, Market.ProductType> OnProductBought;

        internal void Init(Market market, Shop shop, AudioSource audioSource)
        {
            _market = market;
            _shop = shop;
            _audioSource = audioSource;

            _maxProductCount = _marketProductSO.MaxProductCount;
            _currentPrice = _marketProductSO.StartPrice;

            _visual.Init(_marketProductSO, _currentPrice);
            _buyButton = _visual.BuyButton;

            _buyButton.onClick.AddListener(TryBuy);
            _buyButton.onClick.AddListener(_audioSource.Play);

            _purchaseCount = SaveSystem.Instance.LoadMarketProduct(_shop, _marketProductSO.ProductType);
            for (int i = 0; i < _purchaseCount; i++) 
            {
                UpdateProduct(false);
            }
        }

        private void TryBuy()
        {
            if (CanBuy() && _wallet.CanBuy(_currentPrice))
            {
                _wallet.Buy(_currentPrice);
                UpdateProduct(true);
                SaveSystem.Instance.SaveMarketProduct(_shop, _marketProductSO.ProductType, _purchaseCount);

                OnProductBought?.Invoke(_shop, _marketProductSO.ProductType);
            }
            else 
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                AdManager.Instance.ShowRewardedAd(AdRewardCallback);
#else
                Debug.Log("-----REWARD AD-----");
                UpdateProduct(true);
                SaveSystem.Instance.SaveMarketProduct(_shop, _marketProductSO.ProductType, _purchaseCount);

                OnProductBought?.Invoke(_shop, _marketProductSO.ProductType);
#endif
            }
        }

        private void AdRewardCallback() 
        {
            UpdateProduct(true);
            SaveSystem.Instance.SaveMarketProduct(_shop, _marketProductSO.ProductType, _purchaseCount);

            OnProductBought?.Invoke(_shop, _marketProductSO.ProductType);
        }

        private void UpdateProduct(bool incresePurchaseCount)
        {
            if(incresePurchaseCount)
                _purchaseCount++;

            if (CanBuy()) 
            {
                _currentPrice += CalculateNextPrice();
                _visual.UpdatePriceText(_currentPrice);
                UpdateVideoImage();
            }
            else 
            {
                _visual.SoldOut();

                if (_market.PlayerWallet != null) 
                    _market.PlayerWallet.OnCashAmountChanged -= UpdateVideoImage;
                
                _buyButton.interactable = false;
            }
            
        }

        private bool CanBuy() => _purchaseCount < _maxProductCount;

        private int CalculateNextPrice()
        {
            float koefficient = 1.5f;
            int tempPrice = (int)Mathf.Floor(_currentPrice * koefficient);

            while (tempPrice % 10 != 0)
                tempPrice++;

            return tempPrice;
        }

        private void UpdateVideoImage()
        {
            if (_wallet != null)
            {
                if (_wallet.CanBuy(_currentPrice) == false)
                    _visual.VideoImageState(true, _currentPrice);
                else
                    _visual.VideoImageState(false, _currentPrice);
            }
        }

        private void OnEnable()
        {
            if(_market != null) 
            {
                if (_market.PlayerWallet != null)
                {
                    if (_wallet == null && CanBuy())
                    {
                        _wallet = _market.PlayerWallet;
                        _wallet.OnCashAmountChanged += UpdateVideoImage;
                    }
                }
            }

            UpdateVideoImage();
        }

        private void OnDisable()
        {
            if (_wallet != null)
                _wallet.OnCashAmountChanged -= UpdateVideoImage;

            _wallet = null;
        }
    }
}
