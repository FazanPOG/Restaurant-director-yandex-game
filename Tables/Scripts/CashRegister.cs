using Modules.AI.Scripts;
using Modules.Common.Scripts.Interfaces;
using Modules.UI.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Tables.Scripts
{
    public class CashRegister : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform[] _clientQueuePoints;
        [SerializeField] private Transform _cashierPoint;
        [SerializeField] private CashRegisterSoundHandler _soundHandler;

        private CashRegisterInteractUI _cashRegisterInteractUI;
        private Queue<ClientRoot> _clientsQueue = new Queue<ClientRoot>();
        private Transform _freeClientPoint;
        private Cash _cash;
        private bool _canServiceFirstClient;
        private bool _hasCashier;

        public BaseInteractUI BaseInteractUI => _cashRegisterInteractUI;
        public Transform FreeClientPoint => _freeClientPoint;
        public bool CanServiceFirstClient => _canServiceFirstClient;
        public bool HasCashier => _hasCashier;

        private void Awake()
        {
            _cashRegisterInteractUI = GetComponentInChildren<CashRegisterInteractUI>();
            _cash = GetComponentInChildren<Cash>();

            if (_cashRegisterInteractUI == null)
                throw new MissingReferenceException("Missing child: CashRegisterInteractUI");
            if (_cash == null)
                throw new MissingReferenceException("Missing child: Cash");
        }

        private void OnEnable()
        {
            _soundHandler.Init(_cash);

            _freeClientPoint = _clientQueuePoints[0];

            _cashRegisterInteractUI.OnInteracted += InteractUI_OnInteracted;
            AIRoot.OnCashierGot += AIRoot_OnCashierGot;
        }

        private void AIRoot_OnCashierGot(CashRegister cashRegister, CashierRoot cashierRoot)
        {
            if (cashRegister == this) 
            {
                cashierRoot.Init();
                cashierRoot.transform.SetParent(_cashierPoint);
                cashierRoot.transform.localPosition = Vector3.zero;
                cashierRoot.transform.localRotation = Quaternion.identity;
                _hasCashier = true;
            }
        }

        private void InteractUI_OnInteracted(ICarry obj)
        {
            _canServiceFirstClient = false;

            if (_clientsQueue.Count == 0)
                return;

            int orderCost = _clientsQueue.Peek().OrderHandler.PayForOrder();
            _cash.AddCash(orderCost);
                
            GetFirstClientInQueue().Movement.OnTargetReached -= Movement_OnTargetReached;
            _clientsQueue.Dequeue();

            _freeClientPoint = _clientQueuePoints[_clientsQueue.Count];

            if(_clientsQueue.Count != 0)
                UpdateClientsPositionOnQueue();
        }

        public void AddClientToQueue(ClientRoot client) 
        {
            _clientsQueue.Enqueue(client);
            _freeClientPoint = _clientQueuePoints[_clientsQueue.Count];

            client.Movement.OnTargetReached += Movement_OnTargetReached;
        }

        private void Movement_OnTargetReached()
        {
            if(GetFirstClientInQueue().Movement.CurrentTargetType == ClientMovement.TargetType.CashRegister) 
                _canServiceFirstClient = true;
            else 
                throw new MissingReferenceException("Something got wrong");
        }

        private void UpdateClientsPositionOnQueue() 
        {
            int index = 0;
            foreach (ClientRoot client in _clientsQueue)
            {
                client.Movement.UpdateQueuePosition(_clientQueuePoints[index]);
                index++;
            }
        }

        public ClientRoot GetFirstClientInQueue() => _clientsQueue.Peek();

        private void OnDisable()
        {
            _clientsQueue.Clear();
            AIRoot.OnCashierGot -= AIRoot_OnCashierGot;
        }

        private void OnDestroy()
        {
            _cashRegisterInteractUI.OnInteracted -= InteractUI_OnInteracted;
        }
    }
}
