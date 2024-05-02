using Modules.Common.Scripts.Interfaces;
using Modules.Items.Scripts;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Modules.AI.Scripts
{
    public class WaiterCarryHandler : MonoBehaviour, ICarry
    {
        [SerializeField] private Transform _itemCarryPoint;
        [SerializeField, Min(1)] private int _maxCarryItem;

        private List<Item> _itemCarryList;

        public int MaxCarryItem => _maxCarryItem;
        public List<Item> ItemCarryList => _itemCarryList;

        public event Action OnTookItem;
        public event Action OnPutItem;

        internal void Init()
        {
            _itemCarryList = new List<Item>();
        }

        internal void LevelUpWaiter(int currentLevel) 
        {
            int defaultValue = 1;
            _maxCarryItem = currentLevel + defaultValue; 
        }

        public void TryTakeItem(Item item)
        {
            if (IsCanTakeItem())
            {
                _itemCarryList.Add(item);
                item.transform.SetParent(_itemCarryPoint);

                OnTookItem?.Invoke();
            }
        }

        public void PutItem(Item item)
        {
            _itemCarryList.Remove(item);
            item.DestroySelf();

            OnPutItem?.Invoke();
        }

        public void ClearHand()
        {
            foreach (Item item in _itemCarryList)
            {
                item.DestroySelf();
            }

            _itemCarryList?.Clear();

            OnPutItem?.Invoke();
        }

        public bool IsCanTakeItem() => _itemCarryList.Count < _maxCarryItem;
    }
}
