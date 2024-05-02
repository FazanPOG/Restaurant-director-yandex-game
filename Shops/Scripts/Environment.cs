using UnityEngine;

namespace Modules.Shops.Scripts
{
    public class Environment : MonoBehaviour
    {
        [SerializeField] private Bench[] _benches;
        [SerializeField] private AdBanner[] _banners;

        public void Init() 
        {
            foreach (var item in _benches)
                item.Init();

            foreach (var item in _banners)
                item.Init();
        }
    }
}
