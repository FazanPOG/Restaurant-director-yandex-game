using Modules.Items.Scripts;
using Modules.Tables.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.AI.Scripts
{
    public class ClientOrderHandler : MonoBehaviour
    {
        [SerializeField, Range(1, 8)] private int _orderCountMax;
        [SerializeField] private GameObject _orderPrefab;
        [SerializeField] private Transform _orderParent;
        [SerializeField] private SelectingOrdersUI _selectingOrdersUI;
        
        private ClientRoot _clientRoot;
        private ClientMovement _move;
        private List<Order> _orderList;
        private bool _choseOrder;
        private int _orderCost;

        public string ID => _clientRoot.ID;
        internal bool ChoseOrder => _choseOrder;

        public IReadOnlyList<Order> OrderList
        {
            get { return _orderList.AsReadOnly(); }
        }

        public event Action OnChoseOrder;
        public event Action OnOrderRemoved;
        public event Action OnOrdersOver;
        public event Action OnPaidForOrder;

        internal void Init(ClientRoot clientRoot) 
        {
            _clientRoot = clientRoot;
            _move = _clientRoot.Movement;

            _orderList = new List<Order>();
            _choseOrder = false;
            _orderCost = 0;

            _selectingOrdersUI.Init(_move);

            _move.OnClientSeatPlaceSelected += Move_OnClientSeatPlaceSelected;
            _selectingOrdersUI.OnOrderChosen += ChooseOrders_OnOrderChosen;
        }

        private void Move_OnClientSeatPlaceSelected(ClientSeatPlace seatPlace)
        {
            _orderList.Clear();

            Item[] shopItems = seatPlace.Stage.Shop.ShopItems.ToArray();
            CreateRandomCountOfOrders(shopItems);
            HideOrderds();
        }

        private void ChooseOrders_OnOrderChosen()
        {
            ShowOrderds();
            _choseOrder = true;
            OnChoseOrder?.Invoke();
        }

        internal void ClientLevelUp(int level) 
        {
            int defaultValue = 1;
            _orderCountMax = level + defaultValue;

            if (_orderCountMax > 8)
                _orderCountMax = 8;
        }

        public void TakeOrder(Order order) 
        {
            if (_orderList.Contains(order) == false)
                throw new NullReferenceException("Order does not exist");

            _orderList.Remove(order);
            order.DestroySelf();

            OnOrderRemoved?.Invoke();

            if (_orderList.Count == 0)
            {
                _choseOrder = false;

                OnOrdersOver?.Invoke();
            }
        }

        private void CreateRandomCountOfOrders(Item[] items) 
        {
            int orderCount = UnityEngine.Random.Range(1, _orderCountMax);

            for (int i = 0; i < orderCount; i++)
            {
                Order order = InstantiateOrder();
                Item randomItem = items[UnityEngine.Random.Range(0, items.Length)];

                order.Init(randomItem);
                _orderCost += randomItem.ItemSO.Price;
                _orderList.Add(order);
            }
        }

        public int PayForOrder()
        {
            OnPaidForOrder?.Invoke();
            return _orderCost;
        }

        private Order InstantiateOrder() 
        {
            GameObject instance = Instantiate(_orderPrefab);
            instance.transform.SetParent(_orderParent);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;

            Order order = instance.GetComponent<Order>();

            if (order == null)
                throw new MissingComponentException("Missing order script in prefab");

            return order;
        }

        private void HideOrderds() 
        {
            foreach (Order order in _orderList) 
            {
                order.gameObject.SetActive(false);
            }
        }

        private void ShowOrderds() 
        {
            foreach (Order order in _orderList)
            {
                order.gameObject.SetActive(true);
            }
        }

        internal void Disable()
        {
            _orderList.Clear();
            _choseOrder = false;

            _move.OnClientSeatPlaceSelected -= Move_OnClientSeatPlaceSelected;
            _selectingOrdersUI.OnOrderChosen -= ChooseOrders_OnOrderChosen;
            _selectingOrdersUI.Disable();
        }
    }
}
