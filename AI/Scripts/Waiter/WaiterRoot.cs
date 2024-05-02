using Modules.Items.Scripts;
using Modules.Shops.Scripts;
using Modules.Tables.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.AI.Scripts
{
    [RequireComponent(typeof(WaiterMovement))]
    [RequireComponent(typeof(WaiterClientSeatPlaceHandler))]
    [RequireComponent(typeof(WaiterClientHandler))]
    [RequireComponent(typeof(WaiterCarryHandler))]
    public class WaiterRoot : MonoBehaviour
    {
        [SerializeField, TextArea(5, 20)] private string DEBUG_STRING;

        private Shop _shop;
        private WaiterData _data;
        private WaiterMovement _movement;
        private WaiterClientHandler _clientHandler;
        private WaiterClientSeatPlaceHandler _seatPlaceHandler;
        private WaiterCarryHandler _carryHandler;
        private WaiterVisual _visual;
        private List<Table> _tables;
        private ClientOrderHandler _currentClient;
        private int _levelUpsCount;

        internal Shop Shop => _shop;

        private void Awake()
        {
            _movement = GetComponent<WaiterMovement>();
            _clientHandler = GetComponent<WaiterClientHandler>();
            _seatPlaceHandler = GetComponent<WaiterClientSeatPlaceHandler>();
            _carryHandler = GetComponent<WaiterCarryHandler>();
            _visual = GetComponentInChildren<WaiterVisual>();

            if(_visual == null)
                throw new MissingComponentException("Missing child: WaiterVisual");
        }

        internal void Init(WaiterData waiterData, Shop shop) 
        {
            _data = waiterData;
            _shop = shop;
            _tables = _data.GetShopTables(_shop);

            _carryHandler.Init();
            _movement.Init();
            _seatPlaceHandler.Init(this, _data);
            _clientHandler.Init(_seatPlaceHandler, this, _carryHandler);
            _visual.Init(_movement, _carryHandler);

            _shop.OnShopExpanded += UpdateShopData;
            _clientHandler.OnClientFound += ClientHandler_OnClientFound;
            _clientHandler.OnClientSeatPlaceServed += SetDefaultState;
            _carryHandler.OnTookItem += ChooseNextTarget;
        }

        internal void LevelUpWaiter(int currentLevel) => _carryHandler.LevelUpWaiter(currentLevel);

        private void UpdateShopData()
        {
            _tables = _data.GetShopTables(_shop);
            _seatPlaceHandler.UpdateShopData();
        }

        private void Update()
        {
            UpdateDebugString();
        }

        private void UpdateDebugString() 
        {
            if(_clientHandler.CurrentClient != null) 
            {
                DEBUG_STRING = $"Current client ID {_clientHandler.CurrentClient.ID} \n" +
                    $"Current seat place {_seatPlaceHandler.CurrentSeatPlace} \n";
            }
                
        }

        private void ClientHandler_OnClientFound(ClientSeatPlace seatPlace) 
        {
            _currentClient = _clientHandler.CurrentClient;

            _currentClient.OnOrderRemoved += Client_OnOrderRemoved;

            _seatPlaceHandler.SetCurrentSeatPlace(seatPlace);
            ChooseNextTarget();
        }

        private void Client_OnOrderRemoved()
        {
            _clientHandler.HandleOrder();

            if (_clientHandler.CurrentOrders.Count != 0)
                ChooseNextTarget();
        }

        private void ChooseNextTarget()
        {
            
            int itemCarryCount = _carryHandler.ItemCarryList.Count;
            List<Order> orders = _clientHandler.CurrentOrders;

            if (itemCarryCount == orders.Count && itemCarryCount != 0)
                MoveToSeatPlace();
            else if (_carryHandler.IsCanTakeItem() && itemCarryCount < orders.Count)
                MoveToTableWithCertainItem(orders[itemCarryCount].Item);
            else
                MoveToSeatPlace();
        }

        private void MoveToTableWithCertainItem(Item item) 
        {
            Table table = FindTableWithCertainItem(item);
            _movement.MoveToTarget(table.BaseInteractUI.AIPoint.position);
        }

        private void MoveToSeatPlace() 
        {
            _movement.MoveToTarget(_seatPlaceHandler.CurrentSeatPlace.BaseInteractUI.AIPoint.position);
        }

        private void MoveToWaitingClientPoint() 
        {
            Transform randomPoint = _data.WaitingClientPoints[Random.Range(0, _data.WaitingClientPoints.Length)];
            _movement.MoveToTarget(randomPoint.position);
        }

        internal void UpdateClient() 
        {
            _currentClient.OnOrderRemoved -= Client_OnOrderRemoved;
            _currentClient = _clientHandler.CurrentClient;
            _currentClient.OnOrderRemoved += Client_OnOrderRemoved;
            
            ChooseNextTarget();
        }

        private void SetDefaultState() 
        {
            _seatPlaceHandler.SetDefaultState();
            _clientHandler.SetDefaultState();
            _currentClient.OnOrderRemoved -= Client_OnOrderRemoved;
            MoveToWaitingClientPoint();
        }

        private Table FindTableWithCertainItem(Item certainItem)
        {
            Table table = _tables.FirstOrDefault(table => table.GetItem() == certainItem);

            if (table == null)
                throw new MissingReferenceException("Table does not excist");

            return table;
        }

        private void OnDisable()
        {
            _shop.OnShopExpanded -= UpdateShopData;
            _clientHandler.OnClientFound -= ClientHandler_OnClientFound;
            _clientHandler.OnClientSeatPlaceServed -= SetDefaultState;
            _carryHandler.OnTookItem -= ChooseNextTarget;
        }
    }
}
