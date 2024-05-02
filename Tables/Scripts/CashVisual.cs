using UnityEngine;

namespace Modules.Tables.Scripts
{
    public class CashVisual : MonoBehaviour
    {
        [SerializeField] private GameObject _cashPrefab;
        [SerializeField] private GameObject _cashStackPrefab;
        [SerializeField] private GameObject _cashGiantStackPrefab;
        [SerializeField] private GameObject _cashBundlePrefab;
        [SerializeField] private GameObject _cashGiantBundlePrefab;
        [SerializeField, Min(1)] private int _cashValue;
        [SerializeField, Min(2)] private int _cashStackValue;
        [SerializeField, Min(3)] private int _cashGiantStackValue;
        [SerializeField, Min(4)] private int _cashBundleValue;
        [SerializeField, Min(5)] private int _cashGiantBundleValue;

        private Cash _cash;
        private GameObject _cashVisual;

        private void Awake()
        {
            _cash = GetComponentInParent<Cash>();

            if (_cash == null)
                throw new MissingComponentException("Missing parent: Cash");
        }

        private void OnEnable()
        {
            _cash.OnCashAmountChanged += Cash_OnCashAmountChanged;
        }

        private void Cash_OnCashAmountChanged(int cashAmount)
        {
            Destroy(_cashVisual);

            if (cashAmount >= 1 && cashAmount < _cashStackValue) 
            {
                _cashVisual = Instantiate(_cashPrefab, transform.position, Quaternion.identity);
            }
            else if (cashAmount >= _cashStackValue && cashAmount < _cashGiantStackValue) 
            {
                _cashVisual = Instantiate(_cashStackPrefab, transform.position, Quaternion.identity);
            }
            else if (cashAmount >= _cashGiantStackValue && cashAmount < _cashBundleValue)
            {
                _cashVisual = Instantiate(_cashGiantStackPrefab, transform.position, Quaternion.identity);
            }
            else if (cashAmount >= _cashBundleValue && cashAmount < _cashGiantBundleValue)
            {
                _cashVisual = Instantiate(_cashBundlePrefab, transform.position, Quaternion.identity);
            }
            else if (cashAmount >= _cashGiantBundleValue) 
            {
                _cashVisual = Instantiate(_cashGiantBundlePrefab, transform.position, Quaternion.identity);
            }
        }

        private void OnDisable()
        {
            _cash.OnCashAmountChanged -= Cash_OnCashAmountChanged;
        }
    }
}
