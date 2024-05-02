using System;

namespace Modules.Common.Scripts.Interfaces
{
    public interface IHasItems
    {
        public int ItemCount { get; }
        public int ItemCountMax { get; }

        public event Action OnItemCountChanged;
    }
}