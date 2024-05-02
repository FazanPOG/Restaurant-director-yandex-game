using UnityEngine;

namespace Modules.UI.Scripts
{
    public class MovementGuideUI : MonoBehaviour
    {
        [SerializeField] private MovementGuideVisual _visual;
        [SerializeField, Range(0, 60)] private float _timerMax;

        private float _timer;
        private bool _isInit;

        public bool CanShow;

        internal void Init(bool isDesktop)
        {
            _visual.Init(isDesktop);

            _timer = 0f;
            _isInit = true;

            Hide();
        }

        private void Update()
        {
            if (_isInit) 
            {
                if (Input.anyKey && CanShow) 
                {
                    _timer = 0f;
                    Hide();
                }

                _timer += Time.deltaTime;

                if(_timer >= _timerMax && CanShow) 
                    Show();
            }
        }

        private void Show() => _visual.Enable();
        private void Hide() => _visual.Disable();
    }
}
