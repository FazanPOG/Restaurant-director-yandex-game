using Modules.UI.Scripts;
using System;
namespace Modules.Common.Scripts
{
    public interface IOpenable
    {
        public OpenUI OpenUI { get; }
        public bool IsOpen { get; }
        public void Open();
        public void Disable();
    }
}
