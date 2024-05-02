using Modules.Shops.Scripts;
using Modules.Tables.Scripts;
using Modules.UI.Scripts;
using Modules.YandexGames.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Modules.Tables.Scripts.Market;

namespace Modules.AI.Scripts
{
    public class AIRoot : MonoBehaviour
    {
        private const int NUMBER_BOTS_PER_TABLE_MAX = 2;
        private const int NUMBER_BOTS_PER_START_GAME = 3;

        [Header("Client")]
        [SerializeField] private ClientPool _clientPool;
        [SerializeField] private ClientData _clientData;
        [Space(10)]
        [Header("Cashier")]
        [SerializeField] private CashierPool _cashierPool;
        [Space(10)]
        [Header("Waiter")]
        [SerializeField] private WaiterPool _waiterPool;
        [SerializeField] private WaiterData _waiterData;

        private List<ClientSeatPlace> _allClientSeatPlaces;
        private Shop[] _shops;
        private float _clientSpawntimer;
        private int _maxClientCount;
        private int _waiterLevelCount;
        private int _clientLevelCount;

        public static event Action<CashRegister, CashierRoot> OnCashierGot;

        internal event Action<List<ClientSeatPlace>> OnAllClientSeatPlacesUpdated;
        internal event Action<WaiterRoot> OnWaiterGot;

        public void Init(Shop[] shops) 
        {
            _shops = shops;

            _allClientSeatPlaces = GetAllClientSeatPlaces();
            _clientSpawntimer = 0f;

            foreach (Shop shop in _shops) 
            {
                shop.OnShopExpanded += UpdateAllClientSeatPlaces;

                foreach (MarketProduct product in shop.ProductsList) 
                {
                    product.OnProductBought += Product_OnProductBought;
                }
            }

            _clientPool.Init(_clientData);
            _cashierPool.Init();
            _waiterPool.Init(_waiterData);

            CountMaxClients();

            foreach (Shop shop in _shops)
            {
                foreach (MarketProduct product in shop.ProductsList)
                {
                    if (product.PurchaseCount > 0) 
                    {
                        for (int i = 0; i < product.PurchaseCount; i++) 
                        {
                            Product_OnProductBought(product.Shop, product.ProductType);
                        }
                    }
                        
                }
            }
        }

        private void Product_OnProductBought(Shop shop, ProductType type)
        {
            switch (type) 
            {
                case ProductType.Cashier:
                    CashierRoot cashier = _cashierPool.Get();

                    OnCashierGot?.Invoke(shop.GetCashRegister(), cashier);
                    break;

                case ProductType.Waiter:
                    WaiterRoot waiter = _waiterPool.Get();
                    waiter.Init(_waiterData, shop);
                    waiter.LevelUpWaiter(_waiterLevelCount);

                    OnWaiterGot?.Invoke(waiter);
                    break;

                case ProductType.WaiterLevelUp:
                    _waiterLevelCount++;

                    _waiterPool.UpdateWaiterLevel(_waiterLevelCount);
                    break;
                
                case ProductType.ClientLevelUp:
                    _clientLevelCount++;

                    _clientPool.UpdateClientLevel(_clientLevelCount);
                    break;
            }
        }

        private void UpdateAllClientSeatPlaces()
        {
            _allClientSeatPlaces = GetAllClientSeatPlaces();
            
            CountMaxClients();

            OnAllClientSeatPlacesUpdated?.Invoke(_allClientSeatPlaces);
        }

        private void Update()
        {
            if (_clientPool.CountActive < _maxClientCount || _clientPool.CountAll < NUMBER_BOTS_PER_START_GAME)
                SpawnClient();
        }

        private void SpawnClient() 
        {
            if (_clientPool.CountAll == 0 && SaveSystem.Instance.HasSaves() == false)
            {
                ClientRoot client = _clientPool.Get();
                client.Init(this, _clientData, _clientPool.Release, _allClientSeatPlaces.ToArray());
                return;
            }

            if (_clientPool.CountAll == _clientPool.MaxCountClients && _clientPool.CountInactive == 0)
                return;

            if (_clientSpawntimer < _clientPool.SpawnDelay)
            {
                _clientSpawntimer += Time.deltaTime;
            }
            else
            {
                _clientSpawntimer = 0f;
                ClientRoot client = _clientPool.Get();
                client.Init(this, _clientData, _clientPool.Release, _allClientSeatPlaces.ToArray());
            }
        }

        private List<ClientSeatPlace> GetAllClientSeatPlaces() 
        {
            List<ClientSeatPlace> placesList = new List<ClientSeatPlace>();

            foreach (Shop shop in _shops) 
            {
                Stage[] stages = shop.OpenStages.ToArray();

                foreach (Stage stage in stages) 
                {
                    foreach (ClientSeatPlace openPlace in stage.OpenClientSeatPlaces)
                    {
                        placesList.Add(openPlace);
                    }
                }
            }

            return placesList;
        }

        private void CountMaxClients() 
        {
            _maxClientCount = NUMBER_BOTS_PER_TABLE_MAX * _allClientSeatPlaces.Count;
        }
    }
}
