using Modules.Shops.Scripts;
using Modules.Tables.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.AI.Scripts
{
    public class WaiterData : MonoBehaviour
    {
        [SerializeField] private GameObject _waiterPrefab;
        [SerializeField] private Transform[] _waitingClientPoints;

        public GameObject Prefab => _waiterPrefab;
        public Transform[] WaitingClientPoints => _waitingClientPoints;

        internal List<Table> GetShopTables(Shop shop)
        {
            List<Table> tables = new List<Table>();

            foreach (Stage stage in shop.OpenStages)
            {
                foreach (Table table in stage.Tables)
                {
                    tables.Add(table);
                }
            }

            return tables;
        }
        internal List<ClientSeatPlace> GetShopSeatPlaces(Shop shop)
        {
            List<ClientSeatPlace> seatPlaces = new List<ClientSeatPlace>();

            foreach (Stage stage in shop.OpenStages)
            {
                foreach (ClientSeatPlace seatPlace in stage.OpenClientSeatPlaces)
                {
                    seatPlaces.Add(seatPlace);
                }
            }

            return seatPlaces;
        }

        private void OnValidate()
        {
            if(_waiterPrefab.TryGetComponent<WaiterRoot>(out WaiterRoot waiterRoot) == false) 
            {
                _waiterPrefab = null;
                throw new MissingReferenceException("Wrong prefab");
            }
                
        }
    }
}
