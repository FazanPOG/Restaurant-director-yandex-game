using Modules.Common.Scripts;
using Modules.YandexGames.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.YandexGames.Scripts
{
    public class EnglishLocalizationAsset : ILocalizationAsset
    {
        public LanguageType Type => LanguageType.English;

        private readonly Dictionary<string, string> _localizationMap;

        public EnglishLocalizationAsset()
        {
            _localizationMap = new Dictionary<string, string>()
            {
                ["Cool"] = "COOL",
                ["Wow"] = "WOW",
                ["Amazing"] = "AMAZING",
                ["Super"] = "SUPER",
                ["Legendary"] = "LEGENDARY",
                ["PressWASD"] = "Press WASD",
                ["Or"] = "or",
                ["Settings"] = "Settings",
                ["ClientLevel"] = "Client level",
                ["Cashier"] = "Cashier",
                ["Waiter"] = "Waiter",
                ["Level"] = "Waiter level",
                ["Price"] = "Price",
                ["SoldOut"] = "Sold out",
                ["GrabCoffee"] = "Grab a coffee",
                ["BringOrder"] = "Bring your order",
                ["StandAtTheCashRegisterAndTakeYourProfits"] = "Take your profits",
                ["UpgradeYourStore"] = "Upgrade your store",
                ["Thousand"] = "k",
                ["Million"] = "kk",
                ["Billion"] = "kkk",
                ["Unknown"] = "unknown",
                ["AdAfter"] = "Ad in",
                ["WatchTheVideo"] = "Watch the video",
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
