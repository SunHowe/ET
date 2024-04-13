using FairyGUI.Dynamic;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// FUI资源加载器
    /// </summary>
    public sealed class FUIAssetLoader : IUIAssetLoader
    {
        private readonly FUIComponent m_FUIComponent;
        private readonly string m_AssetKeyPrefix;
        
        public FUIAssetLoader(FUIComponent fuiComponent)
        {
            this.m_FUIComponent = fuiComponent;
            this.m_AssetKeyPrefix = fuiComponent.UIAssetKeyPrefix;
        }
        
        /// <summary>
        /// 获取资源加载器组件
        /// </summary>
        private ResourcesLoaderComponent GetResourcesLoaderComponent()
        {
            return m_FUIComponent.Scene().GetComponent<ResourcesLoaderComponent>();
        }

        /// <summary>
        /// 获取包资源键值
        /// </summary>
        private string GetPackageAssetKey(string packageName)
        {
            return $"{m_AssetKeyPrefix}{packageName}_fui.bytes";
        }

        /// <summary>
        /// 获取包内资源键值
        /// </summary>
        private string GetResAssetKey(string packageName, string assetName, string extension)
        {
            return $"{m_AssetKeyPrefix}{packageName}_{assetName}{extension}";
        }
        
        public void LoadUIPackageBytesAsync(string packageName, LoadUIPackageBytesCallback callback)
        {
            InnerLoadUIPackageBytesAsync(packageName, callback).Coroutine();
        }

        private async ETTask InnerLoadUIPackageBytesAsync(string packageName, LoadUIPackageBytesCallback callback)
        {
            TextAsset asset = await GetResourcesLoaderComponent().LoadAssetAsync<TextAsset>(GetPackageAssetKey(packageName));
            callback?.Invoke(asset != null ? asset.bytes : null, string.Empty);
        }

        public void LoadUIPackageBytes(string packageName, out byte[] bytes, out string assetNamePrefix)
        {
            TextAsset asset = GetResourcesLoaderComponent().LoadAssetSync<TextAsset>(GetPackageAssetKey(packageName));
            assetNamePrefix = string.Empty;
            bytes = asset != null ? asset.bytes : null;
        }

        public void LoadTextureAsync(string packageName, string assetName, string extension, LoadTextureCallback callback)
        {
            InnerLoadTextureAsync(packageName, assetName, extension, callback).Coroutine();
        }
        
        private async ETTask InnerLoadTextureAsync(string packageName, string assetName, string extension, LoadTextureCallback callback)
        {
            Texture texture = await GetResourcesLoaderComponent().LoadAssetAsync<Texture>(GetResAssetKey(packageName, assetName, extension));
            callback?.Invoke(texture);
        }

        public void UnloadTexture(Texture texture)
        {
            // TODO 目前没有实现释放资源的方法
            // GetResourcesLoaderComponent().Release(texture);
        }

        public void LoadAudioClipAsync(string packageName, string assetName, string extension, LoadAudioClipCallback callback)
        {
            InnerLoadAudioClipAsync(packageName, assetName, extension, callback).Coroutine();
        }
        
        private async ETTask InnerLoadAudioClipAsync(string packageName, string assetName, string extension, LoadAudioClipCallback callback)
        {
            AudioClip audioClip = await GetResourcesLoaderComponent().LoadAssetAsync<AudioClip>(GetResAssetKey(packageName, assetName, extension));
            callback?.Invoke(audioClip);
        }

        public void UnloadAudioClip(AudioClip audioClip)
        {
            // TODO 目前没有实现释放资源的方法
            // GetResourcesLoaderComponent().Release(audioClip);
        }
    }
}