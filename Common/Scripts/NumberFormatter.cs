using Modules.YandexGames.Scripts;
using System;
using UnityEngine;

namespace Modules.Common.Scripts
{
    public class NumberFormatter
    {
        private const string THOUSAND_KEY = "Thousand";
        private const string MILION_KEY = "Million";
        private const string BILLION_KEY = "Billion";
        private const string UNKNOWN_KEY = "Unknown";

        public static (float, string) FormatNumber(float value)
        {
            string suffix = string.Empty;
            string formattedValue = value.ToString("0.00");

            if (value >= 1000)
            {
                int magnitude = (int)Math.Log10(value) / 3;
                double divisor = Math.Pow(10, magnitude * 3);
                float floatValue = (float)(value / divisor);
                formattedValue = floatValue.ToString("0.00");

                switch (magnitude)
                {
                    case 1:
                        suffix = LocalizationController.Instance.GetByKey(THOUSAND_KEY);
                        break;
                    case 2:
                        suffix = LocalizationController.Instance.GetByKey(MILION_KEY);
                        break;
                    case 3:
                        suffix = LocalizationController.Instance.GetByKey(BILLION_KEY);
                        break;
                    default:
                        suffix = LocalizationController.Instance.GetByKey(UNKNOWN_KEY);
                        break;
                }
            }

            return (float.Parse(formattedValue), suffix);
        }
    }
}
