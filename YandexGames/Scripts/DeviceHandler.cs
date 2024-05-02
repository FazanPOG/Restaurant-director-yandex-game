using System.Runtime.InteropServices;

namespace Modules.YandexGames.Scripts
{
    public class DeviceHandler
    {
        [DllImport("__Internal")]
        private static extern bool IsDesktopDevice();

        public bool IsDesktop() 
        {
            return IsDesktopDevice();
        }
    }
}
