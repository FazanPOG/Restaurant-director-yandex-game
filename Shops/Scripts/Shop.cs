using Modules.Items.Scripts;
using Modules.Tables.Scripts;
using Modules.UI.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.Shops.Scripts
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private Market[] _markets;

        private Stage[] _stages;
        private List<Stage> _openStages;
        private List<Item> _shopItems;
        private List<MarketProduct> _products;

        public IReadOnlyList<Stage> OpenStages 
        {
            get { return _openStages.AsReadOnly(); }
        }
        public IReadOnlyList<Item> ShopItems 
        {
            get { return _shopItems.AsReadOnly(); }
        }
        public IReadOnlyList<MarketProduct> ProductsList
        {
            get { return _products.AsReadOnly(); }
        }

        public event Action OnShopExpanded;
        public event Action OnMarketOpened;
        public event Action OnMarketClosed;

        private void Awake()
        {
            _stages = GetComponentsInChildren<Stage>(true);

            _openStages = new List<Stage>();
            _shopItems = new List<Item>();
            _products = new List<MarketProduct>();

            if (_markets.Length == 0)
                throw new MissingComponentException("Missing shop market");

            foreach (Stage stage in _stages) 
                stage.OnOpenClientSeatPlacesCountChanged += Stage_OnOpenClientSeatPlacesCountChanged;
        }

        public void Init(bool isDesktop) 
        {
            InitStages();
            InitMarkets(isDesktop);

            if (_stages.Length == 0)
                throw new MissingReferenceException("Missing child game object: stage");
        }

        private void InitStages() 
        {
            foreach (Stage stage in _stages)
            {
                stage.Init(this);

                if (stage.IsOpen)
                {
                    _openStages.Add(stage);
                    UpdateShopItems();
                }
                else
                {
                    stage.OnOpened += AddOpenedStage;
                }
            }

        }
        
        private void InitMarkets(bool isDesktop) 
        {
            foreach (Market market in _markets) 
            {
                market.Init(this, isDesktop);

                foreach (MarketProduct product in market.Products) 
                {
                    _products.Add(product);
                }
            }
        }

        private void Stage_OnOpenClientSeatPlacesCountChanged()
        {
            OnShopExpanded?.Invoke();
        }

        internal void MarketOpened() => OnMarketOpened?.Invoke();
        internal void MarketClosed() => OnMarketClosed?.Invoke();

        internal void AddOpenedStage(Stage stage) 
        {
            _openStages.Add(stage);
            UpdateShopItems();

            Stage nextStageToOpen = _stages.FirstOrDefault(item => item.IsOpen == false);
            if(nextStageToOpen != null)
                nextStageToOpen.OpenUI.Enable();

            OnShopExpanded?.Invoke();
        }

        internal bool IsStageNextToOpen(Stage stageToCheck) 
        {
            int counter = 0;
            foreach (Stage stage in _stages) 
            {
                if(stage == stageToCheck) 
                    return counter == 0;

                if (stage.IsOpen == false)
                    counter++;
            }

            return false;
        }

        private void UpdateShopItems() 
        {
            foreach (Stage stage in _openStages) 
            {
                foreach (Item item in stage.StageItems) 
                {
                    if (_shopItems.Contains(item) == false)
                        _shopItems.Add(item);
                }
            }
        }

        public CashRegister GetCashRegister() 
        {
            if(_stages[0].CashRegister == null)
                throw new MissingComponentException("Missing CashRegister");

            return _stages[0].CashRegister;
        }
    }
}
