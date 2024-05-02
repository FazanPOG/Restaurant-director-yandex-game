using UnityEngine;

namespace Modules.Tables.Scripts
{
    public class ClientSeatPlaceVisual : MonoBehaviour
    {
        [SerializeField] private GameObject[] _visualObjectsToHide;

        internal void Disable() 
        {
            foreach (GameObject obj in _visualObjectsToHide) 
            {
                obj.SetActive(false);
            }
        }
        
        internal void Enable() 
        {
            foreach (GameObject obj in _visualObjectsToHide) 
            {
                obj.SetActive(true);
            }
        }
    }
}
