using UnityEngine;

namespace Modules.Shops.Scripts
{
    public class StageVisual : MonoBehaviour
    {
        [SerializeField] private GameObject[] _visualGameObjects;

        internal void Enable()
        {
            foreach (GameObject item in _visualGameObjects) 
            {
                item.SetActive(true);
            }
        }

        internal void Disable()
        {
            foreach (GameObject item in _visualGameObjects) 
            {
                item.SetActive(false);
            }
        }
    }
}
