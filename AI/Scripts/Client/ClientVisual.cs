using UnityEngine;
using static Modules.AI.Scripts.ClientMovement;

namespace Modules.AI.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class ClientVisual : MonoBehaviour
    {
        private const string SitDown = nameof(SitDown);
        private const string StandUp = nameof(StandUp);
        private const string Waiting = nameof(Waiting);
        private const string Walking = nameof(Walking);
        private const string HappyWalk = nameof(HappyWalk);

        private Animator _animator;
        private ClientMovement _movement;
        private ClientOrderHandler _orderHandler;
        private TargetType _targetType;

        private void Awake() => _animator = GetComponent<Animator>();

        internal void Init(ClientRoot root) 
        {
            _movement = root.Movement;
            _orderHandler = root.OrderHandler;

            _movement.OnMoved += Move_OnMoved;
            _movement.OnTargetReached += Move_OnTargetReached;
            _movement.OnLinkTraversalStarted += Move_OnLinkTraversalStarted;
            _movement.OnLinkTraversalComplete += Move_OnLinkTraversalComplete;
            _orderHandler.OnOrdersOver += OrderHandler_OrdersOver;
            _orderHandler.OnPaidForOrder += OrderHandler_OnPaidForOrder;
        }

        private void Move_OnTargetReached()
        {
            _targetType = _movement.CurrentTargetType;

            switch (_targetType)
            {
                case TargetType.Chair:
                    _animator.SetTrigger(SitDown);
                    break;

                case TargetType.CashRegister:
                    _animator.SetTrigger(Waiting);
                    break;
            }
        }

        private void Update()
        {
            if (_targetType == TargetType.CashRegister)
                _movement.gameObject.transform.rotation = Quaternion.Lerp(_movement.gameObject.transform.rotation, _movement.QueuePointTransform.transform.rotation, 0.01f);

            if (_targetType == TargetType.Chair && _movement.OnMove == false) 
                _movement.gameObject.transform.rotation = Quaternion.Slerp(_movement.gameObject.transform.rotation, _movement.Chair.transform.rotation, 0.01f);
        }

        private void Move_OnMoved() 
        {
            if(_targetType != TargetType.Exit)
                _animator.SetTrigger(Walking);
        }

        private void Move_OnLinkTraversalStarted() => _animator.SetTrigger(Waiting);

        private void Move_OnLinkTraversalComplete() => _animator.SetTrigger(Walking);

        private void OrderHandler_OrdersOver() => _animator.SetTrigger(StandUp);

        private void OrderHandler_OnPaidForOrder() 
        {
            _targetType = TargetType.Exit;
            _animator.SetTrigger(HappyWalk);
        }

        internal void Disable() 
        {
            _targetType = TargetType.None;

            _movement.OnMoved -= Move_OnMoved;
            _movement.OnTargetReached -= Move_OnTargetReached;
            _movement.OnLinkTraversalStarted -= Move_OnLinkTraversalStarted;
            _movement.OnLinkTraversalComplete -= Move_OnLinkTraversalComplete;
            _orderHandler.OnOrdersOver -= OrderHandler_OrdersOver;
            _orderHandler.OnPaidForOrder -= OrderHandler_OnPaidForOrder;
        }
    }
}
