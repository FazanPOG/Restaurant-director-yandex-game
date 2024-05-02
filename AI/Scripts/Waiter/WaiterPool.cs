using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Modules.AI.Scripts
{
    public class WaiterPool : MonoBehaviour
    {
        private GameObject _waiterPrefab;
        private WaiterData _waiterData;
        private ObjectPool<WaiterRoot> _pool;
        private List<WaiterRoot> _allWaiters;
        private int _waiterLevel;

        public IReadOnlyList<WaiterRoot> AllWaiters => _allWaiters.AsReadOnly();

        internal void Init(WaiterData waiterData)
        {
            _waiterData = waiterData;
            _waiterPrefab = _waiterData.Prefab;
            _allWaiters = new List<WaiterRoot>();

            int prewarmObjectsCount = 2;
            int maxWaiterCount = 30;
            _pool = new ObjectPool<WaiterRoot>(OnCreateWaiter, OnGetWaiter, OnRelease, OnWaiterDestroy, false, prewarmObjectsCount, maxWaiterCount);

        }

        public WaiterRoot Get()
        {
            var waiterRoot = _pool.Get();
            waiterRoot.LevelUpWaiter(_waiterLevel);

            return waiterRoot;
        }

        public void UpdateWaiterLevel(int level)
        {
            _waiterLevel = level;

            foreach (var waiter in _allWaiters)
                waiter.LevelUpWaiter(_waiterLevel);
        }

        private WaiterRoot OnCreateWaiter()
        {
            GameObject instance = Instantiate(_waiterPrefab);
            instance.transform.SetParent(this.gameObject.transform);
            instance.transform.position = _waiterData.WaitingClientPoints[UnityEngine.Random.Range(0, _waiterData.WaitingClientPoints.Length)].transform.position;
            WaiterRoot waiterRoot = instance.GetComponent<WaiterRoot>();

            if (waiterRoot == null)
                throw new MissingReferenceException("Wrong prefab");

            _allWaiters.Add(waiterRoot);

            return waiterRoot;
        }

        private void OnGetWaiter(WaiterRoot waiterRoot)
        {
            waiterRoot.gameObject.SetActive(true);
        }

        private void OnRelease(WaiterRoot waiterRoot)
        {
            waiterRoot.gameObject.SetActive(false);
        }

        private void OnWaiterDestroy(WaiterRoot waiterRoot)
        {
            Destroy(waiterRoot);
        }
    }
}
