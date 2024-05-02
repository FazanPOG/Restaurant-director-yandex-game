using Modules.Common.Scripts;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Modules.AI.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class WaiterMovement : MonoBehaviour, IBotMovement
    {
        private NavMeshAgent _agent;
        private bool _onMove;

        public NavMeshAgent Agent => _agent;
        public bool OnMove => _onMove;

        public event Action OnMoved;
        public event Action OnTargetReached;

        private void Awake() => _agent = GetComponent<NavMeshAgent>();

        internal void Init() 
        {
            _agent.avoidancePriority = UnityEngine.Random.Range(1, 50);
            _onMove = false;
        }

        private void Update()
        {
            if (_onMove) 
            {
                if (IsDestinationReached()) 
                {
                    _onMove = false;
                    OnTargetReached?.Invoke();
                }
            }
            
        }

        public void MoveToTarget(Vector3 target)
        {
            _agent.destination = target;
            _onMove = true;

            OnMoved?.Invoke();
        }

        private bool IsDestinationReached()
        {
            if (!_agent.pathPending && _agent.remainingDistance <= 0.3f)
                return true;

            return false;
        }
    }
}
