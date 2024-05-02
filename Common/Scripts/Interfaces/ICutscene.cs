using Modules.Player.Scripts;
using Modules.UI.Scripts;
using System;
using UnityEngine;

namespace Modules.Common.Scripts.Interfaces
{
    public interface ICutscene
    {
        public CutsceneDataSO CutsceneDataSO { get; }
        public void StartCutscene(Camera camera, Vector3 cameraDefaultPosition, CutsceneGuideUI cutsceneGuideUI, Action EndCutsceneCallback);
        public void StopCutscene();
    }
}
