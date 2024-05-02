using Modules.Player.Scripts;
using UnityEngine;

namespace Modules.UI.Scripts
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private MoneyCounterUI _moneyCounter;
        [SerializeField] private SettingsUI _settingsUI;
        [SerializeField] private MovementGuideUI _movementGuideUI;
        [SerializeField] private AdBonusRewardUI _bonusRewardUI;

        public void Init(PlayerRoot player, bool isDesktop)
        {
            CatchExceptions();

            if (player.TryGetComponent<PlayerWallet>(out PlayerWallet wallet))
                _moneyCounter.Init(wallet);

            if(player.TryGetComponent<PlayerInputHandler>(out PlayerInputHandler handler))
                _settingsUI.Init(handler);

            _movementGuideUI.Init(isDesktop);

            if (wallet != null && player.TryGetComponent<PlayerMovement>(out PlayerMovement movement))
                _bonusRewardUI.Init(wallet, movement, isDesktop);
        }

        private void CatchExceptions() 
        {
            if(_moneyCounter == null)
                throw new MissingReferenceException("Missing MoneyCounterUI");
            if (_settingsUI == null)
                throw new MissingReferenceException("Missing SettingsUI");
            if (_movementGuideUI == null)
                throw new MissingReferenceException("Missing MovementGuideUI");
        }
    }
}
