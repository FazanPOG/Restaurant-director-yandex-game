using Modules.YandexGames.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Modules.AI.Scripts
{
    public class ClientPool : MonoBehaviour
    {
        [SerializeField] private Transform _firstClientSpawnPoint;
        [SerializeField, Range(2, 50)] private int _maxCountClients;
        [SerializeField, Range(1, 60)] private int _spawnDelay;

        private List<ClientRoot> _allClients;
        private ClientData _clientData;
        private GameObject _clientPrefab;
        private Transform[] _spawnPoints;
        private ObjectPool<ClientRoot> _pool;
        private int _clientLevel;

        public List<ClientRoot> AllClients => _allClients;
        public int MaxCountClients => _maxCountClients;
        public int SpawnDelay => _spawnDelay;
        public int CountAll => _pool.CountAll;
        public int CountInactive => _pool.CountInactive;
        public int CountActive => _pool.CountActive;

        public void Init(ClientData clientData)
        {
            _clientData = clientData;

            _allClients = new List<ClientRoot>();

            _clientPrefab = _clientData.Prefab;
            _spawnPoints = _clientData.SpawnPoints;

            int prewarmObjectsCount = 2;
            _pool = new ObjectPool<ClientRoot>(OnCreateClient, OnGetClient, OnRelease, OnClientDestroy, false, prewarmObjectsCount, _maxCountClients);
        }

        public ClientRoot Get() 
        {
            var client = _pool.Get();
            client.OrderHandler.ClientLevelUp(_clientLevel);
            return client;
        }

        public void UpdateClientLevel(int level) 
        {
            _clientLevel = level;

            foreach (var client in _allClients)
                client.OrderHandler.ClientLevelUp(_clientLevel);
        }

        public void Release(ClientRoot client) 
        {
            client.SetDefaultState();
            _pool.Release(client);
        }

        private ClientRoot OnCreateClient() 
        {
            GameObject instance;
            ClientRoot clientRoot;

            int randomPointIndex = Random.Range(0, _spawnPoints.Length);

            if (_pool.CountAll == 0 && SaveSystem.Instance.HasSaves() == false) 
                instance = Instantiate(_clientPrefab, _firstClientSpawnPoint.position, Quaternion.identity);
            else
                instance = Instantiate(_clientPrefab, _spawnPoints[randomPointIndex].position, Quaternion.identity);

            instance.transform.SetParent(this.gameObject.transform);
            clientRoot = instance.GetComponent<ClientRoot>();

            if (clientRoot == null)
                throw new MissingReferenceException("Wrong prefab");

            _allClients.Add(clientRoot);

            return clientRoot;
        }

        private void OnGetClient(ClientRoot client) 
        {
            client.gameObject.SetActive(true);
        }

        private void OnRelease(ClientRoot client) 
        {
            client.gameObject.SetActive(false);
        }

        private void OnClientDestroy(ClientRoot client) 
        {
            Destroy(client);
        }

    }
}
