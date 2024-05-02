using Modules.UI.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Player.Scripts
{
    [CreateAssetMenu(menuName = "SO/Cutscene/CutsceneData")]
    public class CutsceneDataSO : ScriptableObject
    {
        [SerializeField] private List<Vector3> _targets;
        [SerializeField] private List<string> _guidTextsKeys;
        [SerializeField, Range(1, 60)] private float _moveDuration;

        public List<Vector3> Targets => _targets;
        public List<string> GuidTextsKeys => _guidTextsKeys;
        public float MoveDuration => _moveDuration;
    }
}
