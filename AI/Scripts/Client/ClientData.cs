using UnityEngine;

namespace Modules.AI.Scripts
{
    public class ClientData : MonoBehaviour
    {
        [SerializeField] private GameObject _clientPrefab;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private Transform[] _exitPoints;
        [SerializeField] private Transform[] _wanderPoints;

        public GameObject Prefab => _clientPrefab;
        public Transform[] SpawnPoints => _spawnPoints;
        public Transform[] ExitPoints => _exitPoints;
        public Transform[] WanderPoints => _wanderPoints;
    }
}
