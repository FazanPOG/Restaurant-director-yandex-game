using Modules.UI.Scripts;
using System;
using UnityEngine;

namespace Modules.Player.Scripts
{
    public class CutsceneController : MonoBehaviour
    {
        [SerializeField] private CutsceneGuideUI _guideUI;
        [SerializeField] private OpenCutscene _openCutscene;
        [SerializeField] private PayForOrderCutscene _payForOrderCutscene;
        [SerializeField] private BuyNextStageCutscene _buyNextStageCutscene;

        private Camera _camera;
        private Action _endCutsceneCallback;

        internal void Init(Camera camera, Action endCutsceneCallback)
        {
            _camera = camera;
            _endCutsceneCallback = endCutsceneCallback;

            _guideUI.Disable();
        }

        internal void StartPayForOrderCutscene(Action anotherEndCutsceneCallback = null)
        {
            if(anotherEndCutsceneCallback == null)
                _payForOrderCutscene.StartCutscene(_camera, _camera.transform.position, _guideUI, _endCutsceneCallback);
            else
                _payForOrderCutscene.StartCutscene(_camera, _camera.transform.position, _guideUI, anotherEndCutsceneCallback);
        }

        internal void StartOpenCutscene(Action anotherEndCutsceneCallback = null) 
        {
            if (anotherEndCutsceneCallback == null)
                _openCutscene.StartCutscene(_camera, _camera.transform.position, _guideUI, _endCutsceneCallback);
            else
                _openCutscene.StartCutscene(_camera, _camera.transform.position, _guideUI, anotherEndCutsceneCallback);
        }

        internal void StartBuyNextStageCutscene(Action anotherEndCutsceneCallback = null) 
        {
            if (anotherEndCutsceneCallback == null)
                _buyNextStageCutscene.StartCutscene(_camera, _camera.transform.position, _guideUI, _endCutsceneCallback);
            else
                _buyNextStageCutscene.StartCutscene(_camera, _camera.transform.position, _guideUI, anotherEndCutsceneCallback);
        }
    }
}
