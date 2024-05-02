using System.Collections.Generic;
using UnityEngine;

namespace Modules.Items.Scripts
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemSO _itemSO;
        [SerializeField] private Transform _topPoint;

        public ItemSO ItemSO => _itemSO;
        public Transform TopPoint => _topPoint;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Item otherItem = (Item)obj;

            bool IdEquals = EqualityComparer<string>.Default.Equals(ItemSO.ID, otherItem.ItemSO.ID);

            return IdEquals;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + EqualityComparer<ItemSO>.Default.GetHashCode(ItemSO);
            hash = hash * 23 + gameObject.name.GetHashCode();
            return hash;
        }

        public void DestroySelf() 
        {
            Destroy(gameObject);
        }
    }
}
