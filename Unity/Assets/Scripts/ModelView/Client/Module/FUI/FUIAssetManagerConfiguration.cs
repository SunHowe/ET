using FairyGUI.Dynamic;

namespace ET.Client
{
    /// <summary>
    /// FUI资源管理器配置
    /// </summary>
    public sealed class FUIAssetManagerConfiguration : Object, IUIAssetManagerConfiguration
    {
        public IUIPackageHelper PackageHelper { get; }
        public IUIAssetLoader AssetLoader { get; }
        public bool UnloadUnusedUIPackageImmediately { get; }

        public FUIAssetManagerConfiguration(IUIPackageHelper packageHelper, IUIAssetLoader assetLoader, bool unloadUnusedUIPackageImmediately = false)
        {
            this.PackageHelper = packageHelper;
            this.AssetLoader = assetLoader;
            this.UnloadUnusedUIPackageImmediately = unloadUnusedUIPackageImmediately;
        }
    }
}