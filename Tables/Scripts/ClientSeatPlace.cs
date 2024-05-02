using Modules.AI.Scripts;
using Modules.Common.Scripts;
using Modules.Common.Scripts.Interfaces;
using Modules.Shops.Scripts;
using Modules.UI.Scripts;
using Modules.YandexGames.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.Tables.Scripts
{
    public class ClientSeatPlace : MonoBehaviour, IInteractable, IOpenable
    {
        [SerializeField] private Transform[] _clientChairsArray;
        [SerializeField, TextArea] private string DEBUG_STRING = "Waiting Init...";

        private ClientSeatPlaceVisual _visual;
        private InteractUI _interactUI;
        private Dictionary<Transform, bool> _clientChairsDictionary;
        private List<ClientOrderHandler> _clientsList;
        private Stage _stage;
        private OpenUI _openUI;
        private bool _isOpen;
        private bool _isBusyByWaiter;

        public IReadOnlyList<ClientOrderHandler> Clients => _clientsList.AsReadOnly();
        public Stage Stage => _stage;
        public bool IsOpen => _isOpen;
        public bool IsBusyByWaiter => _isBusyByWaiter;
        public OpenUI OpenUI => _openUI;

        public BaseInteractUI BaseInteractUI => _interactUI;

        public event Action<ClientSeatPlace> OnOpened;

        private void Update()
        {
            if (_clientChairsDictionary != null) 
            {
                DEBUG_STRING = $"ClientListCount - {_clientsList.Count} \n FreeChairsCount - {FreeChairCounter()}";
            }
        }

        private void Awake()
        {
            _visual = GetComponentInChildren<ClientSeatPlaceVisual>();
            _interactUI = GetComponentInChildren<InteractUI>();
            _openUI = GetComponentInChildren<OpenUI>();

            if (_visual == null)
                throw new MissingReferenceException("Missing child game objects: ClientSeatPlaceVisual");

            _interactUI.OnInteracted += InteractUI_OnInteracted;
        }

        public void Init(Stage stage)
        {
            _stage = stage;

            _clientChairsDictionary = new Dictionary<Transform, bool>();
            _clientsList = new List<ClientOrderHandler>();

            if (_clientChairsArray.Length == 0)
                throw new NullReferenceException("Missing seat place");

            foreach (Transform chair in _clientChairsArray)
            {
                _clientChairsDictionary.Add(chair, true);
            }

            _isOpen = SaveSystem.Instance.LoadOpenObject(gameObject);

            if (_openUI != null)
            {
                if (_isOpen == false)
                    _openUI.Init();
                else
                    _openUI.Disable();
            }
            else 
            {
                _isOpen = true;
            }
        }

        public void Open()
        {
            _visual.Enable();
            _interactUI.gameObject.SetActive(true);
            _isOpen = true;

            OnOpened?.Invoke(this);
        }

        public void Disable()
        {
            _visual.Disable();
            _interactUI.gameObject.SetActive(false);
        }

        private void InteractUI_OnInteracted(ICarry iCarry)
        {
            if (_interactUI.GameObjectInPlayerPlace.TryGetComponent<WaiterClientHandler>(out WaiterClientHandler waiterClientHandler))
            {
                IReadOnlyList<Order> orderList = waiterClientHandler.CurrentClient.OrderList;

                foreach (Order order in orderList)
                {
                    if (iCarry.ItemCarryList.Contains(order.Item))
                    {
                        iCarry.PutItem(iCarry.ItemCarryList[iCarry.ItemCarryList.IndexOf(order.Item)]);
                        waiterClientHandler.CurrentClient.TakeOrder(order);
                        return;
                    }
                }
            }
            else 
            {
                foreach(var client in _clientsList)
                {
                    foreach (Order order in client.OrderList)
                    {
                        if (iCarry.ItemCarryList.Contains(order.Item))
                        {
                            iCarry.PutItem(iCarry.ItemCarryList[iCarry.ItemCarryList.IndexOf(order.Item)]);
                            client.TakeOrder(order);
                            return;
                        }
                    }
                }
            }
        }

        public void TakeChair(Transform chairTransform, ClientOrderHandler client)
        {
            CatchKeyException(chairTransform);
            _clientChairsDictionary[chairTransform] = false;
            _clientsList.Add(client);
        }

        public void FreeUpChair(Transform chairTransform, ClientOrderHandler client)
        {
            CatchKeyException(chairTransform);
            _clientChairsDictionary[chairTransform] = true;
            _clientsList.Remove(client);
        }

        public void TakeSeatPlace(WaiterClientSeatPlaceHandler waiter) => _isBusyByWaiter = true;
        public void FreeUpSeatPlace(WaiterClientSeatPlaceHandler waiter) => _isBusyByWaiter = false;

        private void CatchKeyException(Transform chairTransform)
        {
            if (!_clientChairsDictionary.ContainsKey(chairTransform))
            {
                throw new NullReferenceException("seat place does not exist or it has been moving");
            }
        }

        public Transform GetNearestFreeChair()
        {
            foreach (var item in _clientChairsDictionary)
            {
                if (item.Value == true)
                {
                    return item.Key;
                }
            }

            throw new MissingReferenceException("Trying to take a non-existent chair");
        }

        private int FreeChairCounter() 
        {
            int counter = 0;
            foreach (var item in _clientChairsDictionary) 
            {
                if (item.Value == true)
                {
                    counter++;
                }
            }
            return counter;
        }

        public bool HasFreeChair() 
        {
            return _clientChairsDictionary.ContainsValue(true);
        }

        public bool HasClient() => _clientsList.Count > 0;

        private void OnDestroy()
        {
            _interactUI.OnInteracted -= InteractUI_OnInteracted;
        }
    }
}
