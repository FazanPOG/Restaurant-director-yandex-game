using Modules.YandexGames.Enums;

namespace Modules.Common.Scripts
{
    public interface ILocalizationAsset
    {
        LanguageType Type { get; }
        string GetByKey(string key);
    }
}