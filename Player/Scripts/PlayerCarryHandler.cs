using System;
using System.Collections.Generic;
using UnityEngine;
using Modules.Common.Scripts.Interfaces;
using Modules.Items.Scripts;

namespace Modules.Player.Scripts
{
    public class PlayerCarryHandler : MonoBehaviour, ICarry
    {
        [SerializeField] private Transform _itemCarryPoint;

        private List<Item> _itemCarryList;
        private int _maxCarryItem;

        public int MaxCarryItem => _maxCarryItem;

        public List<Item> ItemCarryList => _itemCarryList;

        public event Action OnTookItem;
        public event Action OnPutItem;

        internal void Init()
        {
            _maxCarryItem = 10;
            _itemCarryList = new List<Item>();
        }

        public void TryTakeItem(Item item)
        {
            if (IsCanTakeItem())
            {
                _itemCarryList.Add(item);

                item.transform.SetParent(_itemCarryPoint);
                UpdateItemPosition();

                OnTookItem?.Invoke();
            }
        }

        public void PutItem(Item item)
        {
            _itemCarryList.Remove(item);
            item.DestroySelf();
            UpdateItemPosition();

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

        private void UpdateItemPosition() 
        {
            float nextPos = 0f;

            for (int i = 0; i < _itemCarryList.Count; i++) 
            {
                _itemCarryList[i].transform.localPosition = Vector3.zero;
                _itemCarryList[i].transform.localRotation = Quaternion.identity;

                if (i > 0)
                    _itemCarryList[i].transform.localPosition = new Vector3(0f, nextPos, 0f);

                nextPos += _itemCarryList[i].TopPoint.localPosition.y * _itemCarryList[i].transform.localScale.y;
            }
        }

        public bool IsCanTakeItem() => _itemCarryList.Count<_maxCarryItem;
    }
}
