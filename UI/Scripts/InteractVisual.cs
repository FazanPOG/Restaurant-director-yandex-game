using System;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI.Scripts
{
    public class InteractVisual : MonoBehaviour
    {
        [SerializeField] private Image fillingImage;

        private float interactTimer;
        private float interactTimerMax = 1f;

        public event Action OnImageFilled;

        private void Awake()
        {
            fillingImage.fillAmount = 0f;
            interactTimer = 0f;
        }

        private void Start()
        {
            Hide();
        }

        public void EnterInteraction()
        {
            Show();
        }

        public void ExitInteraction()
        {
            fillingImage.fillAmount = 0f;
            interactTimer = 0f;
            Hide();
        }

        private void Update()
        {
            if (interactTimer < interactTimerMax)
            {
                interactTimer += Time.deltaTime;

                fillingImage.fillAmount = interactTimer;
            }
            else
            {
                interactTimer = 0f;

                fillingImage.fillAmount = interactTimer;

                OnImageFilled?.Invoke();
            }
        }

        internal void Show()
        {
            gameObject.SetActive(true);
        }

        internal void Hide()
        {
            gameObject.SetActive(false);
        }

    }
}
