using Modules.Common.Scripts.Interfaces;
using Modules.UI.Scripts;
using System;
using System.Collections;
using UnityEngine;

namespace Modules.Player.Scripts
{
    public class Cutscene : MonoBehaviour, ICutscene
    {
        [SerializeField] private CutsceneDataSO _cutsceneDataSO;

        private Camera _camera;
        private Vector3 _cameraDefaultPosition;
        private CutsceneGuideUI _cutsceneGuideUI;
        private Action _endCutsceneCallback;

        public CutsceneDataSO CutsceneDataSO => _cutsceneDataSO;

        public void StartCutscene(Camera camera, Vector3 cameraDefaultPosition, CutsceneGuideUI cutsceneGuideUI, Action endCutsceneCallback)
        {
            _camera = camera;
            _cameraDefaultPosition = cameraDefaultPosition;
            _cutsceneGuideUI = cutsceneGuideUI;
            _endCutsceneCallback = endCutsceneCallback;

            StartCoroutine(MoveCameraSequence(_camera, _cameraDefaultPosition, _endCutsceneCallback));
        }

        public void StopCutscene()
        {
            StopAllCoroutines();
            _endCutsceneCallback?.Invoke();
        }

        private IEnumerator MoveCameraSequence(Camera camera, Vector3 cameraDefaultPosition, Action endCutsceneCallback)
        {
            for (int i = 0; i < _cutsceneDataSO.Targets.Count; i++)
            {
                _cutsceneGuideUI.SetText(_cutsceneDataSO.GuidTextsKeys[i]);
                yield return StartCoroutine(MoveCameraToTarget(camera, _cutsceneDataSO.Targets[i]));
            }

            endCutsceneCallback?.Invoke();
        }

        private IEnumerator MoveCameraToTarget(Camera camera, Vector3 target)
        {
            _cutsceneGuideUI.Enable();
            Vector3 startPosition = camera.transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < _cutsceneDataSO.MoveDuration)
            {
                camera.transform.position = Vector3.Lerp(startPosition, target, elapsedTime / _cutsceneDataSO.MoveDuration);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _cutsceneGuideUI.Disable();
            camera.transform.position = target;
        }
    }
}
