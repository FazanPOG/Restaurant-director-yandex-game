using Modules.Common.Scripts;
using Modules.Player.Scripts;
using Modules.YandexGames.Scripts;
using System.Collections;
using UnityEngine;

namespace Modules.UI.Scripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class OpenUI : MonoBehaviour
    {
        [SerializeField] private PlayerWallet _playerWallet;
        [SerializeField] private GameObject _iOpenableGameObject;
        [SerializeField, Min(1)] private int _cost;
        [SerializeField, Min(1)] private float _fillDuration;
        [SerializeField] private Sprite _itemSprite;

        private BoxCollider _collider;
        private IOpenable _iOpenable;
        private OpenVisual _visual;
        private Coroutine fillCoroutine;

        private bool _isActive;

        private void Awake()
        {
            if(_iOpenableGameObject.TryGetComponent<IOpenable>(out IOpenable iOpenable))
                _iOpenable = iOpenable;
            else
                throw new MissingComponentException("Missing IOpenable");

            _visual = GetComponentInChildren<OpenVisual>();
            _collider = GetComponent<BoxCollider>();

            if (_visual == null)
                throw new MissingComponentException("Missing child: OpenVisual");
        }

        public void Init() 
        {
            _visual.Init(_cost);

            _isActive = true;
            SwitchVisual();

            _iOpenable.Disable();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerWallet>(out PlayerWallet playerWallet))
            {
                fillCoroutine = StartCoroutine(FillImage(_fillDuration));
            }
        }

        private void SwitchVisual()
        {
            if (_playerWallet.CashAmount < _cost && _isActive)
                _visual.SetVideoSprite();
            else
                _visual.SetDefaultState();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerWallet>(out PlayerWallet playerWallet))
            {
                if (fillCoroutine != null)
                {
                    StopCoroutine(fillCoroutine);
                    _visual._filledImage.fillAmount = 0f;
                }
            }
        }

        private IEnumerator FillImage(float fillDuration)
        {
            float timer = 0f;

            while (timer < fillDuration)
            {
                timer += Time.deltaTime;

                float fillAmount = Mathf.Lerp(0f, 1f, timer / fillDuration);
                _visual._filledImage.fillAmount = fillAmount;

                yield return null;
            }

            _visual._filledImage.fillAmount = 1f;

            Buy();
        }

        private void Buy() 
        {
            if (_playerWallet.CashAmount >= _cost)
            {
                _playerWallet.Buy(_cost);
                Open();
            }
            else
            {
                ConfirmAdViewUI.Instance.Open(_itemSprite);
                ConfirmAdViewUI.Instance.WatchButton.onClick.AddListener(() => 
                {
                    ConfirmAdViewUI.Instance.Watch(Open);
                });
            }
        }

        internal void Open() 
        {
            ConfirmAdViewUI.Instance.gameObject.SetActive(false);

            _iOpenableGameObject.gameObject.SetActive(true);
            _iOpenable.Open();

            SaveSystem.Instance.SaveOpenObject(_iOpenableGameObject);

            _visual.PlayFX();
            Disable();
            StartCoroutine(WaitForFX(_visual.FX));
        }

        internal void Disable() 
        {
            _visual.Disable();
            _collider.enabled = false;
            _isActive = false;
        }

        internal void Enable() 
        {
            _visual.Enable();
            
            _collider.enabled = true;
            _isActive = true;

            SwitchVisual();
        }

        private IEnumerator WaitForFX(ParticleSystem fx) 
        {
            yield return new WaitWhile(() => fx.isPlaying);
            Hide();
        }

        private void Hide() => gameObject.SetActive(false);

        private void OnEnable()
        {
            _playerWallet.OnCashAmountChanged += SwitchVisual;

            _isActive = true;
            SwitchVisual();
        }

        private void OnDisable()
        {
            _playerWallet.OnCashAmountChanged -= SwitchVisual;
        }
    }
}
