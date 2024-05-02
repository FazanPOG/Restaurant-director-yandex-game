using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.AI.Scripts
{
    public class SelectingOrdersUI : MonoBehaviour
    {
        [SerializeField] private float _timerMax;
        [SerializeField] private Image _fillImage;

        private ClientMovement _movement;

        internal event Action OnOrderChosen;

        internal void Init(ClientMovement move) 
        {
            _movement = move;

            _fillImage.fillAmount = 0;
            _fillImage.gameObject.SetActive(false);

            _movement.OnTargetReached += Move_OnTargetReached;
        }

        private void Move_OnTargetReached()
        {
            ClientMovement.TargetType targetType = _movement.CurrentTargetType;

            if (targetType == ClientMovement.TargetType.Chair)
            {
                _fillImage.gameObject.SetActive(true);
                StartCoroutine(FillImage(_timerMax));
            }
        }

        IEnumerator FillImage(float timerMax) 
        {
            float elapsedTime = 0f;
            float fillPercentage = 0f;

            while (elapsedTime < timerMax)
            {
                elapsedTime += Time.deltaTime;
                fillPercentage = Mathf.Clamp01(elapsedTime / timerMax);

                _fillImage.fillAmount = fillPercentage;

                yield return null;
            }

            _fillImage.gameObject.SetActive(false);
            OnOrderChosen?.Invoke();
        }

        internal void Disable()
        {
            _movement.OnTargetReached -= Move_OnTargetReached;
        }
    }
}
