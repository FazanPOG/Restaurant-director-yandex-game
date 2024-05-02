using Modules.Tables.Scripts;
using UnityEngine;

namespace Modules.UI.Scripts
{
    public sealed class CashRegisterInteractUI : BaseInteractUI
    {
        [SerializeField] private CashRegister _cashRegister;

        private void Update()
        {
            if (_cashRegister.CanServiceFirstClient) 
            {
                if (_cashRegister.HasCashier || ICarry != null)
                {
                    _interactVisual.EnterInteraction();
                }
            }
            else 
            {
                _interactVisual.ExitInteraction();
            }
        }

    }
}
