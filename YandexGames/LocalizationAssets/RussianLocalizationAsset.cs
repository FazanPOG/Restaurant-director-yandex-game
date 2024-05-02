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
                ["Cool"] = "�����",
                ["Wow"] = "���",
                ["Amazing"] = "����������",
                ["Super"] = "�����",
                ["Legendary"] = "����������",
                ["PressWASD"] = "������� WASD",
                ["Or"] = "���",
                ["Settings"] = "���������",
                ["ClientLevel"] = "������� �������",
                ["Cashier"] = "������",
                ["Waiter"] = "��������",
                ["Level"] = "�������",
                ["Price"] = "����",
                ["SoldOut"] = "����������",
                ["GrabCoffee"] = "�������� ����",
                ["BringOrder"] = "��������� �����",
                ["StandAtTheCashRegisterAndTakeYourProfits"] = "�������� �������",
                ["UpgradeYourStore"] = "���������� �������",
                ["Thousand"] = "���.",
                ["Million"] = "���.",
                ["Billion"] = "����.",
                ["Unknown"] = "����������",
                ["AdAfter"] = "������� �����",
                ["WatchTheVideo"] = "������� �� ��������",
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
