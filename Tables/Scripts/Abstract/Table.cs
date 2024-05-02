using UnityEngine;
using System.Collections.Generic;
using Modules.Common.Scripts.Interfaces;
using System;
using Modules.Items.Scripts;
using Modules.UI.Scripts;

namespace Modules.Tables.Scripts
{
    public abstract class Table : MonoBehaviour, IHasItems, IInteractable
    {
        [SerializeField] private Item _itemPrefab;

        private Stack<Item> _itemStack;
        private ItemSpawner _itemSpawner;
        private ItemInteractUI _itemInteractUI;

        public int ItemCount { get; protected set; }
        public int ItemCountMax { get; protected set; }
        public BaseInteractUI BaseInteractUI => _itemInteractUI;

        public event Action OnItemCountChanged;

        private void Awake()
        {
            _itemSpawner = GetComponentInChildren<ItemSpawner>();
            _itemInteractUI = GetComponentInChildren<ItemInteractUI>();

            ItemCountMax = _itemSpawner.ItemPointsLength;

            if (_itemInteractUI == null)
                throw new MissingReferenceException("Missing child: ItemInteractUI");
            if (_itemSpawner == null)
                throw new MissingReferenceException("Missing child: ItemSpawner");
            if (ItemCountMax == 0)
                throw new MissingReferenceException("Missing item spawn point (ItemSpawner)");
        }

        private void Start()
        {
            _itemStack = new Stack<Item>();
        }

        private void OnEnable()
        {
            _itemSpawner.OnItemSpawned += AddItem;
            _itemInteractUI.OnInteracted += RemoveItem;
        }

        protected virtual void AddItem(Item item)
        {
            _itemStack.Push(item);
            ItemCount++;

            OnItemCountChanged?.Invoke();
        }

        protected virtual void RemoveItem(ICarry iCarry)
        {
            if (iCarry.IsCanTakeItem() && _itemStack.Count != 0)
            {
                iCarry.TryTakeItem(_itemStack.Pop());
                ItemCount--;

                OnItemCountChanged?.Invoke();
            }
        }

        public Item GetItem()
        {
            return _itemPrefab;
        }

        private void OnDisable()
        {
            _itemSpawner.OnItemSpawned -= AddItem;
            _itemInteractUI.OnInteracted -= RemoveItem;
        }

    }
}
