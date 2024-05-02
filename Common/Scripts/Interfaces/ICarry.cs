using Modules.Items.Scripts;
using System;
using System.Collections.Generic;

namespace Modules.Common.Scripts.Interfaces
{
    public interface ICarry
    {
        public int MaxCarryItem { get; }
        public List<Item> ItemCarryList { get; }

        public event Action OnTookItem;
        public event Action OnPutItem;

        public void TryTakeItem(Item item);
        public void PutItem(Item item);
        public void ClearHand();
        public bool IsCanTakeItem();
    }
}
