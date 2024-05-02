using UnityEngine;
using UnityEngine.Pool;

namespace Modules.AI.Scripts
{
    public class CashierPool : MonoBehaviour
    {
        [SerializeField] private GameObject _cashierPrefab;

        private ObjectPool<CashierRoot> _pool;

        internal void Init() 
        {
            int prewarmObjectsCount = 2;
            int maxCashierCount = 10;
            _pool = new ObjectPool<CashierRoot>(OnCreateCashier, OnGetCashier, OnRelease, OnCashierDestroy, false, prewarmObjectsCount, maxCashierCount);
        }

        public CashierRoot Get()
        {
            var cashier = _pool.Get();
            return cashier;
        }

        private CashierRoot OnCreateCashier()
        {
            GameObject instance = Instantiate(_cashierPrefab);
            instance.transform.SetParent(this.gameObject.transform);
            CashierRoot clientRoot = instance.GetComponent<CashierRoot>();

            if (clientRoot == null)
                throw new MissingReferenceException("Wrong prefab");

            return clientRoot;
        }

        private void OnGetCashier(CashierRoot cashier)
        {
            cashier.gameObject.SetActive(true);
        }

        private void OnRelease(CashierRoot cashier)
        {
            cashier.gameObject.SetActive(false);
        }

        private void OnCashierDestroy(CashierRoot cashier)
        {
            Destroy(cashier);
        }
    }
}
