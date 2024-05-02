using Modules.Player.Scripts;
using System;
using UnityEngine;

namespace Modules.Tables.Scripts
{
    public class Cash : MonoBehaviour
    {
        private int _cashAmount = 0;

        public int CashAmount => _cashAmount;

        public event Action<int> OnCashAmountChanged;

        internal void AddCash(int cashAmount) 
        {
            if(cashAmount < 0)
                throw new MissingComponentException("Cash amount must be >= 0");

            _cashAmount += cashAmount;

            OnCashAmountChanged?.Invoke(_cashAmount);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerWallet>(out PlayerWallet playerWallet) && _cashAmount > 0)
            {
                playerWallet.AddCash(_cashAmount);
                _cashAmount = 0;

                OnCashAmountChanged?.Invoke(_cashAmount);
            }
        }
    }
}
