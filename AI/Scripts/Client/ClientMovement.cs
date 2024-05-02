using Modules.Common.Scripts;
using Modules.Tables.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Modules.AI.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ClientMovement : MonoBehaviour, IBotMovement
    {
        private ClientRoot _clientRoot;
        private NavMeshAgent _agent;

        private Transform[] _wanderPointsTransform;
        private Transform[] _exitPointsTransform;

        private ClientSeatPlace[] _allClientSeatPlaces;
        private ClientSeatPlace _selectedClientSeatPlace;
        private Transform _chair;

        private CashRegister _cashRegister;
        private Transform _queuePointTransform;

        private TargetType _targetType;

        private float _defaultSpeed;
        private bool _linkTraversalStarted = false;
        private bool _onMove;

        private Action<ClientRoot> ExitCallback;

        public bool OnMove => _onMove;
        internal TargetType CurrentTargetType => _targetType;
        internal Transform Chair => _chair;
        internal Transform QueuePointTransform => _queuePointTransform;

        public event Action OnMoved;
        public event Action OnTargetReached;
        internal event Action<ClientSeatPlace> OnClientSeatPlaceSelected;
        internal event Action OnLinkTraversalStarted;
        internal event Action OnLinkTraversalComplete;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();

            _defaultSpeed = _agent.speed;
            _targetType = TargetType.None;

            _agent.avoidancePriority = UnityEngine.Random.Range(1, 80);
            _agent.autoTraverseOffMeshLink = true;
        }

        private void OnEnable() 
        {
            _onMove = true;
            _targetType = TargetType.None;
        } 

        internal void Init(ClientRoot clientRoot, ClientData clientData, ClientSeatPlace[] allClientSeatPlaces, Action<ClientRoot> exitCallback) 
        {
            _clientRoot = clientRoot;
            _allClientSeatPlaces = allClientSeatPlaces;
            ExitCallback = exitCallback;

            _exitPointsTransform = clientData.ExitPoints;
            _wanderPointsTransform = clientData.WanderPoints;

            _clientRoot.OrderHandler.OnOrdersOver += OrderHandler_OnOrdersOver;
            _clientRoot.OrderHandler.OnPaidForOrder += OrderHandler_OnPaidForOrder;
        }

        private void Update()
        {
            if (_onMove) 
            {
                HandleNavMeshLinkTraversal();

                switch (_targetType) 
                {
                    case TargetType.None:
                        _chair = FindFreeChair();
                        if(_chair != null) 
                        {
                            TakeChair(_chair);
                            SetDestination(_chair.position, TargetType.Chair);
                        }
                        break;

                    case TargetType.Chair:
                        if (IsDestinationReached())
                        {
                            _onMove = false;
                            OnTargetReached?.Invoke();
                        }
                        break;

                    case TargetType.CashRegister:
                        if (IsDestinationReached())
                        {
                            _onMove = false;
                            OnTargetReached?.Invoke();
                        }
                        break;

                    case TargetType.Exit:
                        if (IsDestinationReached())
                        {
                            ExitCallback?.Invoke(_clientRoot);
                            _onMove = false;
                            OnTargetReached?.Invoke();
                        }
                        break;
                }
            }
            
        }

        public void MoveToTarget(Vector3 target)
        {
            _agent.destination = target;
            _onMove = true;
            OnMoved?.Invoke();
        }

        private void SetDestination(Vector3 target, TargetType nextTargetType) 
        {
            _targetType = nextTargetType;
            MoveToTarget(target);
        }

        private void HandleNavMeshLinkTraversal()
        {
            if (_agent.isOnOffMeshLink)
            {
                if (_linkTraversalStarted == false)
                {
                    _linkTraversalStarted = true;

                    float agentNavMeshLinkSpeed = 1.5f;
                    _agent.speed = agentNavMeshLinkSpeed;

                    OnLinkTraversalStarted?.Invoke();
                }
            }
            else
            {
                if (_linkTraversalStarted == true)
                {
                    _linkTraversalStarted = false;
                    _agent.speed = _defaultSpeed;

                    OnLinkTraversalComplete?.Invoke();
                }
            }
        }

        private bool IsDestinationReached()
        {
            if (!_agent.pathPending && _agent.remainingDistance <= 0.5f)
                return true;

            return false;
        }

        internal void UpdateAllClientSeatPlaces(List<ClientSeatPlace> allClientSeatPlaces)
        {
            _allClientSeatPlaces = allClientSeatPlaces.ToArray();
        }

        private Transform FindFreeChair() 
        {
            int randomIndex = UnityEngine.Random.Range(0, _allClientSeatPlaces.Length);
            ClientSeatPlace randomPlace = _allClientSeatPlaces[randomIndex];
            if (randomPlace.HasFreeChair())
            {
                _selectedClientSeatPlace = randomPlace;
                Transform chair = _selectedClientSeatPlace.GetNearestFreeChair();

                return chair;
            }

            return null;
        }

        private void TakeChair(Transform chair) 
        {
            _selectedClientSeatPlace.TakeChair(chair, _clientRoot.OrderHandler);

            OnClientSeatPlaceSelected?.Invoke(_selectedClientSeatPlace);
        }

        private void OrderHandler_OnOrdersOver()
        {
            float standUpDelay = 2f;
            StartCoroutine(MoveToCashRegisterWithStandUpDelay(standUpDelay));
        }

        internal void UpdateQueuePosition(Transform transform)
        {
            _queuePointTransform = transform;
            SetDestination(_queuePointTransform.position, TargetType.CashRegister);
        }

        private void OrderHandler_OnPaidForOrder()
        {
            int randomIndex = UnityEngine.Random.Range(0, _exitPointsTransform.Length);
            Transform randomExit = _exitPointsTransform[randomIndex].transform;

            SetDestination(randomExit.position, TargetType.Exit);
        }

        private IEnumerator MoveToCashRegisterWithStandUpDelay(float delay) 
        {
            _selectedClientSeatPlace.FreeUpChair(_chair, _clientRoot.OrderHandler);
            _cashRegister = _selectedClientSeatPlace.Stage.CashRegister;
            _queuePointTransform = _cashRegister.FreeClientPoint;
            _cashRegister.AddClientToQueue(_clientRoot);
            yield return new WaitForSeconds(delay);
            SetDestination(_queuePointTransform.position, TargetType.CashRegister);
            yield break;
        }

        internal void Disable() 
        {
            _clientRoot.OrderHandler.OnOrdersOver -= OrderHandler_OnOrdersOver;
            _clientRoot.OrderHandler.OnPaidForOrder -= OrderHandler_OnPaidForOrder;
        }

        internal enum TargetType
        {
            None,
            Chair,
            CashRegister,
            Exit,
        }
    }
}
