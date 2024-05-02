using Modules.Common.Scripts;
using Modules.YandexGames.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.YandexGames.Scripts
{
    public class RussianLocalizationAsset : ILocalizationAsset
    {
        public LanguageType Type => LanguageType.Russian;

        private readonly Dictionary<string, string> _localizationMap;

        public RussianLocalizationAsset()
        {
            _localizationMap = new Dictionary<string, string>()
            {
                ["Cool"] = "КРУТО",
                ["Wow"] = "ОГО",
                ["Amazing"] = "ПОТРЯСАЮЩЕ",
                ["Super"] = "СУПЕР",
                ["Legendary"] = "ЛЕГЕНДАРНО",
                ["PressWASD"] = "Нажмите WASD",
                ["Or"] = "или",
                ["Settings"] = "Настройки",
                ["ClientLevel"] = "Уровень клиента",
                ["Cashier"] = "Кассир",
                ["Waiter"] = "Официант",
                ["Level"] = "Уровень",
                ["Price"] = "Цена",
                ["SoldOut"] = "Распродано",
                ["GrabCoffee"] = "Возьмите кофе",
                ["BringOrder"] = "Принесите заказ",
                ["StandAtTheCashRegisterAndTakeYourProfits"] = "Заберите прибыль",
                ["UpgradeYourStore"] = "Прокачайте магазин",
                ["Thousand"] = "тыс.",
                ["Million"] = "млн.",
                ["Billion"] = "млрд.",
                ["Unknown"] = "неизвестно",
                ["AdAfter"] = "Реклама через",
                ["WatchTheVideo"] = "Открыть за просмотр",
            };
        }

        public string GetByKey(string key)
        {
            if (_localizationMap.TryGetValue(key, out var result))
                return result;

            Debug.LogError($"Key [{key}] not found in localization asset [{Type}]");

            return "Error in localization";
        }

        public string GetByValue(string value)
        {
            var result = _localizationMap.FirstOrDefault(x => x.Value == value);

            if (result.Key == null)
            {
                Debug.LogError($"Key [{result.Key}] not found in localization asset [{Type}]");

                return "Error in localization";
            }
            else
            {
                return result.Value;
            }
        }
    }
}
