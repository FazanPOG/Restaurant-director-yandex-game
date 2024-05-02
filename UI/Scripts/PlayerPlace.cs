using UnityEngine;
using System;
using Modules.Player.Scripts;
using Modules.AI.Scripts;

namespace Modules.UI.Scripts
{
    public class PlayerPlace : MonoBehaviour
    {
        [SerializeField] private PlayerPlaceVisual _visual;

        private Collider _collider;
        private GameObject _currentActiveObject;

        public event Action<GameObject> OnEnterInPlace;
        public event Action<GameObject> OnExitInPlace;

        private void Awake() => _collider = GetComponent<Collider>();

        private void OnTriggerEnter(Collider other)
        {
            if(_currentActiveObject == null) 
            {
                EnterTrigger(other);
                _currentActiveObject = other.gameObject;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_currentActiveObject == null)
            {
                EnterTrigger(other);
                _currentActiveObject = other.gameObject;
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (_currentActiveObject == other.gameObject) 
            {
                ExitTrigger(other);
                _currentActiveObject = null;
            }
        }

        private void EnterTrigger(Collider other) 
        {
            if (other.TryGetComponent<PlayerRoot>(out PlayerRoot playerRoot))
            {
                _visual.PlayVisual();
                OnEnterInPlace?.Invoke(playerRoot.gameObject);
            }

            if (other.TryGetComponent<WaiterRoot>(out WaiterRoot waiterRoot))
            {
                _visual.PlayVisual();
                OnEnterInPlace?.Invoke(waiterRoot.gameObject);
            }
        }

        private void ExitTrigger(Collider other) 
        {
            if (other.TryGetComponent<PlayerRoot>(out PlayerRoot playerRoot))
            {
                _visual.StopVisual();
                OnExitInPlace?.Invoke(playerRoot.gameObject);
            }

            if (other.TryGetComponent<WaiterRoot>(out WaiterRoot waiterRoot))
            {
                _visual.StopVisual();
                OnExitInPlace?.Invoke(waiterRoot.gameObject);
            }
        }
    }
}