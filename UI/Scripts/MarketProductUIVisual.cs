using Modules.Common.Scripts;
using Modules.YandexGames.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI.Scripts
{
    public class MarketProductUIVisual : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _soldOutImage;
        [SerializeField] private Image _videoImage;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _buyButton;

        private const string PRICE_TEXT_KEY = "Price";
        private const string SOLD_OUT_TEXT_KEY = "SoldOut";

        public Button BuyButton => _buyButton;

        internal void Init(MarketProductSO marketProductSO, int currentPrice) 
        {
            _titleText.text = marketProductSO.Title;
            _iconImage.sprite = marketProductSO.Icon;
            _soldOutImage.gameObject.SetActive(false);
            _videoImage.gameObject.SetActive(false);
            
            UpdatePriceText(currentPrice);
        }

        internal void UpdatePriceText(float nextPrice) 
        {
            var result = NumberFormatter.FormatNumber(nextPrice);

            if (result.Item2 == string.Empty)
                _priceText.text = LocalizationController.Instance.GetByKey(PRICE_TEXT_KEY) + ": " + result.Item1.ToString();
            else
                _priceText.text = LocalizationController.Instance.GetByKey(PRICE_TEXT_KEY) + ": " + result.Item1.ToString() + " " + result.Item2;
        }

        internal void SoldOut()
        {
            _priceText.text = LocalizationController.Instance.GetByKey(SOLD_OUT_TEXT_KEY) + "!";
            _soldOutImage.gameObject.SetActive(true);
            _videoImage.gameObject.SetActive(false);
        }

        internal void VideoImageState(bool state, int nextPrice) 
        {
            if (state == true) 
            {
                _videoImage.gameObject.SetActive(true);
                _priceText.text = LocalizationController.Instance.GetByKey(PRICE_TEXT_KEY) + ": ";
            }
            else 
            {
                _videoImage.gameObject.SetActive(false);
                UpdatePriceText(nextPrice);
            }
        }
    }
}
