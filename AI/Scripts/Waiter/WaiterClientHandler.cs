using Modules.Items.Scripts;
using Modules.Tables.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.AI.Scripts
{
    public class WaiterClientHandler : MonoBehaviour
    {
        [SerializeField, TextArea(3, 10)] private string DEBUG_STRING;

        private WaiterClientSeatPlaceHandler _waiterClientSeatPlaceHandler;
        private WaiterRoot _waiterRoot;
        private WaiterCarryHandler _waiterCarryHandler;
        private ClientOrderHandler _currentClient;
        private List<Order> _currentOrders = new List<Order>();

        internal List<Order> CurrentOrders => _currentOrders;
        internal ClientOrderHandler CurrentClient => _currentClient;

        internal event Action OnClientSeatPlaceServed;
        internal event Action<ClientSeatPlace> OnClientFound;

        internal void Init(WaiterClientSeatPlaceHandler waiterClientSeatPlaceHandler, WaiterRoot waiterRoot, WaiterCarryHandler waiterCarryHandler)
        {
            _waiterClientSeatPlaceHandler = waiterClientSeatPlaceHandler;
            _waiterRoot = waiterRoot;
            _waiterCarryHandler = waiterCarryHandler;
        }

        private void Update() 
        {
            UpdateDebugString();

            if (_currentClient == null)
                FindingForReadyClient();
        }

        private void UpdateDebugString()
        {
            DEBUG_STRING = "";

            for (int i = 0; i < _currentOrders.Count; i++)
                DEBUG_STRING += $"Current orders {_currentOrders[i].Item.name} \n";
        }

        internal void HandleOrder() 
        {
            CheckIfPlayerServicedClient();
            UpdateProperties();
            HandleClientServiceStatus();
        }

        private void HandleClientServiceStatus() 
        {
            if (_currentOrders.Count == 0)
            {
                _waiterCarryHandler.ClearHand();

                if (_waiterClientSeatPlaceHandler.CurrentSeatPlace.Clients.FirstOrDefault(client => client.ChoseOrder && client != _currentClient) == null)
                {
                    OnClientSeatPlaceServed?.Invoke();
                }
                else
                {
                    _currentClient = _waiterClientSeatPlaceHandler.CurrentSeatPlace.Clients.First(client => client != _currentClient && client.ChoseOrder);
                    UpdateProperties();
                    _waiterRoot.UpdateClient();
                }
            }
        }
        
        private void CheckIfPlayerServicedClient() 
        {
            if (_currentClient == null)
                throw new MissingReferenceException("Current client NULL");

            if (_waiterCarryHandler.ItemCarryList.Count > _currentClient.OrderList.Count)
            {
                Debug.Log("Player remove order");
                Item itemFromOrderWhichPlayerRemoved = _currentOrders.Except(_currentClient.OrderList.ToList()).FirstOrDefault().Item;
                Item itemToRemove = _waiterCarryHandler.ItemCarryList.First(item => item.ItemSO.ID == itemFromOrderWhichPlayerRemoved.ItemSO.ID);
                _waiterCarryHandler.PutItem(itemToRemove);
            }
        }

        internal void UpdateProperties() 
        {
            UpdateCurrentOrders();
        }

        private void UpdateCurrentOrders() => _currentOrders = _currentClient.OrderList.ToList();

        internal void SetDefaultState() 
        {
            _currentOrders.Clear();
            _currentClient = null;
        }

        internal void FindingForReadyClient()
        {
            if(_currentClient != null)
                throw new MissingReferenceException("Current client must be null");

            ClientSeatPlace place = _waiterClientSeatPlaceHandler.GetNonServiceSeatPlace();

            if(place != null) 
            {
                ClientOrderHandler client = place.Clients.FirstOrDefault(client => client.ChoseOrder);

                if (client != null)
                {
                    _currentClient = client;

                    UpdateCurrentOrders();

                    OnClientFound?.Invoke(place);
                }
            }
        }
    }
}
