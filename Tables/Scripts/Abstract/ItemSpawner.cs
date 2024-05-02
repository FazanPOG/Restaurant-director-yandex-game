using Modules.Items.Scripts;
using System;
using UnityEngine;

namespace Modules.Tables.Scripts
{
    public abstract class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _itemSpawnPoints;

        private Table _table;
        private Item _item;

        public event Action<Item> OnItemSpawned;

        public int ItemPointsLength => _itemSpawnPoints.Length;

        private void Awake()
        {
            _table = GetComponentInParent<Table>();
        }

        private void Start()
        {
            _item = _table.GetItem();
        }

        protected virtual void TrySpawnItem()
        {
            if (_table.ItemCount < _table.ItemCountMax)
            {
                Item itemInstance = Instantiate(_item);
                SetItemParent(itemInstance);

                OnItemSpawned.Invoke(itemInstance);
            }
        }

        protected virtual void SetItemParent(Item itemInstance)
        {
            int index = _table.ItemCount;

            itemInstance.transform.SetParent(_itemSpawnPoints[index]);
            itemInstance.transform.localRotation = Quaternion.identity;
            itemInstance.transform.localPosition = Vector3.zero;
        }
    }
}
