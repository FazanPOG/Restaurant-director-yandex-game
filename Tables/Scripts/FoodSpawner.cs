using UnityEngine;

namespace Modules.Tables.Scripts
{
    public class FoodSpawner : ItemSpawner
    {
        private float _spawnTimer;
        private float _spawnTimerMax = 2.5f;

        private void Update()
        {
            if (_spawnTimer < _spawnTimerMax)
            {
                _spawnTimer += Time.deltaTime;
            }
            else
            {
                _spawnTimer = 0f;
                TrySpawnItem();
            }
        }
    }
}
