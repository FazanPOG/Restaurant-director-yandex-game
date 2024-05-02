using UnityEngine;

namespace Modules.Items.Scripts
{
    [CreateAssetMenu(menuName = "SO/Item")]
    public class ItemSO : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private int _price = 1;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _id;

        public string ID => _id;
        public string Name => _name;
        public int Price => _price;
        public Sprite Sprite => _sprite;

        private void OnValidate()
        {
            if(_price <= 0)
                _price = 1;
        }
    }
}
