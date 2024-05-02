using Modules.YandexGames.Scripts;
using System;
using UnityEngine;

namespace Modules.Player.Scripts
{
    public class PlayerWallet : MonoBehaviour
    {
        [SerializeField] private int _cashAmount;

        public int CashAmount => _cashAmount;

        public event Action OnCashAmountChanged;

        internal void Init() 
        {
            _cashAmount = SaveSystem.Instance.LoadMoney();
            OnCashAmountChanged?.Invoke();
        }

        public void AddCash(int cashAmount) 
        {
            _cashAmount += cashAmount;
            SaveSystem.Instance.SaveMoney(_cashAmount);

            OnCashAmountChanged?.Invoke();
        }

        public void Buy(int cost) 
        {
            if(cost >= 0) 
            {
                _cashAmount -= cost;
                SaveSystem.Instance.SaveMoney(_cashAmount);

                OnCashAmountChanged?.Invoke();
            }
            else 
            {
                throw new ArgumentException("Cost must be >= 0");
            }

            if (_cashAmount < 0)
                throw new ArgumentException("Trying to buy when don't have enough money");
        }

        public bool CanBuy(int cost) => _cashAmount >= cost;
    }
}
