using Modules.Tables.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.AI.Scripts
{
    public class WaiterClientSeatPlaceHandler : MonoBehaviour
    {
        private WaiterRoot _waiterRoot;
        private WaiterData _waiterData;
        private List<ClientSeatPlace> _clientSeatPlaces;
        private ClientSeatPlace _currentSeatPlace;
        private ClientSeatPlace _previousSeatPlace;

        internal ClientSeatPlace CurrentSeatPlace => _currentSeatPlace;

        internal void Init(WaiterRoot waiterRoot, WaiterData waiterData)
        {
            _waiterRoot = waiterRoot;
            _waiterData = waiterData;

            _clientSeatPlaces = _waiterData.GetShopSeatPlaces(_waiterRoot.Shop);
        }

        internal void UpdateShopData() 
        {
            _clientSeatPlaces = _waiterData.GetShopSeatPlaces(_waiterRoot.Shop);
        }

        internal void SetCurrentSeatPlace(ClientSeatPlace place) 
        {
            _currentSeatPlace = place;
            _currentSeatPlace.TakeSeatPlace(this);
        }

        internal void SetDefaultState()
        {
            _previousSeatPlace = _currentSeatPlace;
            _currentSeatPlace.FreeUpSeatPlace(this);
            _currentSeatPlace = null;
        }

        internal ClientSeatPlace GetNonServiceSeatPlace()
        {
            foreach (ClientSeatPlace place in _clientSeatPlaces)
            {
                if (place.HasClient() && place.IsBusyByWaiter == false && place != _previousSeatPlace)
                    return place;
            }

            return null;
        }
    }
}
