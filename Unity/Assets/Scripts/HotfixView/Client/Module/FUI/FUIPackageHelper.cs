using FairyGUI.Dynamic;

namespace ET.Client
{
    /// <summary>
    /// FUI包辅助工具
    /// </summary>
    public sealed class FUIPackageHelper : IUIPackageHelper
    {
        private readonly FUIComponent m_FUIComponent;

        public FUIPackageHelper(FUIComponent fuiComponent)
        {
            this.m_FUIComponent = fuiComponent;
        }

        public string GetPackageNameById(string id)
        {
            return string.Empty;
        }
    }
}