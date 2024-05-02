using Modules.Shops.Scripts;
using Modules.Tables.Scripts;
using UnityEngine;

namespace Modules.YandexGames.Scripts
{
    public class SaveSystem : MonoBehaviour
    {
        private const string MONEY_KEY = "Money";

        public static SaveSystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                throw new MissingReferenceException("There is more then one SaveSystem instance");

            //PlayerPrefs.DeleteAll();
        }

        public void SaveMoney(int money) 
        {
            PlayerPrefs.SetInt(MONEY_KEY, money);
            PlayerPrefs.Save();
        }

        public int LoadMoney() 
        {
            return PlayerPrefs.GetInt(MONEY_KEY, 0);
        }

        public void SaveOpenObject(GameObject obj)
        {
            string key = $"{obj.name} ; {obj.transform.parent.transform.position.x} ; {obj.transform.parent.transform.position.y}" +
                $"{obj.transform.position.x} ; {obj.transform.position.y}";

            PlayerPrefs.SetString(key, key);
            PlayerPrefs.Save();
        }

        public bool LoadOpenObject(GameObject obj)
        {
            string key = $"{obj.name} ; {obj.transform.parent.transform.position.x} ; {obj.transform.parent.transform.position.y}" +
                $"{obj.transform.position.x} ; {obj.transform.position.y}";

            string result = PlayerPrefs.GetString(key);

            if (result == string.Empty)
                return false;

            return true;
        }

        public void SaveMarketProduct(Shop shop, Market.ProductType productType, int purchasesCount)
        {
            PlayerPrefs.SetInt($"{shop.name} {productType}", purchasesCount);
        }

        public int LoadMarketProduct(Shop shop, Market.ProductType productType)
        {
            return PlayerPrefs.GetInt($"{shop.name} {productType}", 0);
        }

        public bool HasSaves() 
        {
            if (PlayerPrefs.HasKey(MONEY_KEY))
                return true;

            return false;
        }

        private void OnDisable()
        {
            PlayerPrefs.Save();
        }
    }
}
