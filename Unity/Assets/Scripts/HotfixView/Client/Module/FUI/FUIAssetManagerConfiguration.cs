using FairyGUI.Dynamic;

namespace ET.Client
{
    /// <summary>
    /// FUI资源管理器配置
    /// </summary>
    public sealed class FUIAssetManagerConfiguration : IUIAssetManagerConfiguration
    {
        public IUIPackageHelper PackageHelper { get; }
        public IUIAssetLoader AssetLoader { get; }
        public bool UnloadUnusedUIPackageImmediately => false;

        public FUIAssetManagerConfiguration(FUIComponent fuiComponent)
        {
            PackageHelper = fuiComponent.UIPackageHelper;
            AssetLoader = new FUIAssetLoader(fuiComponent);
        }
    }
}