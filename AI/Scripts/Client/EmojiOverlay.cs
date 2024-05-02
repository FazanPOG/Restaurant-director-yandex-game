using UnityEngine;

namespace Modules.AI.Scripts
{
    public class EmojiOverlay : MonoBehaviour
    {
        [SerializeField] private EmojiOverlayVisual _visual;

        private ClientOrderHandler _clientOrderHandler;

        internal void Init(ClientOrderHandler clientOrderHandler) 
        {
            _clientOrderHandler = clientOrderHandler;

            _clientOrderHandler.OnPaidForOrder += ClientOrderHandler_OnPaidForOrder;
        }

        private void ClientOrderHandler_OnPaidForOrder() => _visual.UpdateVisual();

        private void OnDisable()
        {
            _clientOrderHandler.OnPaidForOrder -= ClientOrderHandler_OnPaidForOrder;
        }
    }
}
