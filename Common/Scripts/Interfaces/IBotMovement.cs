using System;
using UnityEngine;

namespace Modules.Common.Scripts
{
    public interface IBotMovement
    {
        public bool OnMove { get; }
        public void MoveToTarget(Vector3 target);

        public event Action OnMoved;
        public event Action OnTargetReached;
    }
}
