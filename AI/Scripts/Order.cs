using Modules.Items.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.AI.Scripts
{
    public class Order : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public Item Item { get; private set; }

        internal void Init(Item item) 
        {
            Item = item;
            _image.sprite = Item.ItemSO.Sprite;
        }

        internal void DestroySelf() 
        {
            Destroy(gameObject);
        }

    }
}
