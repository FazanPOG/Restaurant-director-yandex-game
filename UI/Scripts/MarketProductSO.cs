using Modules.Tables.Scripts;
using Modules.YandexGames.Scripts;
using UnityEngine;

namespace Modules.UI.Scripts
{
    [CreateAssetMenu(menuName = "SO/UI/MarketProduct")]
    public class MarketProductSO : ScriptableObject
    {
        [SerializeField, Min(1)] private int _maxProductCount;
        [SerializeField, Min(1)] private int _startPrice;
        [SerializeField] private Market.ProductType _productType;
        [Header("Visual")]
        [SerializeField] private string _titleKey;
        [SerializeField] private Sprite _icon;

        public int MaxProductCount => _maxProductCount;
        public int StartPrice => _startPrice;
        public Market.ProductType ProductType => _productType;
        public string Title => LocalizationController.Instance.GetByKey(_titleKey);
        public Sprite Icon => _icon;
    }
}
