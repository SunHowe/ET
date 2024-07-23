using System.Threading;
using FairyGUI;
using UnityEngine;

namespace ET.Client.Common
{
    public partial class GameObjectComponentSystem
    {
        private static void DisposeGoWrapper(this GameObjectComponent self)
        {
            if (self.GoWrapper == null)
                return;
            
            self.GoWrapper.Dispose();
            self.GoWrapper = null;
        }

        /// <summary>
        /// 销毁GoWrapper渲染器
        /// </summary>
        private static void DisposeGoWrapperRenderer(this GameObjectComponent self)
        {
            if (self.GoWrapper != null && self.GoWrapper.wrapTarget == self.CurrentViewObject)
                self.GoWrapper.wrapTarget = null;

            self.RecoverMaterials();
            self.m_CachedRenderers?.Clear();
        }

        /// <summary>
        /// 更新GoWrapper渲染器
        /// </summary>
        private static async ETTask<GameObject> UpdateGoWrapperRenderer(this GameObjectComponent self, string assetKey, ETCancellationToken token)
        {
            FUI fuiEntity = self.GetFUIFormEntity();
            ResourcesLoaderComponent resourcesLoaderComponent = fuiEntity.GetComponent<ResourcesLoaderComponent>();

            GameObject prefab = await resourcesLoaderComponent.LoadAssetAsync<GameObject>(assetKey);
            if (prefab == null)
            {
                return null;
            }

            if (token.IsCancel())
            {
                return null;
            }

            return UnityEngine.Object.Instantiate(prefab);
        }

        /// <summary>
        /// 设置GoWrapper渲染器
        /// </summary>
        private static void SetupGoWrapperRenderer(this GameObjectComponent self)
        {
            if (self.CurrentRendererType != GameObjectComponent.RendererType.GoWrapper)
                return;
            
            // 初始化GoWrapper
            if (self.GoWrapper == null)
            {
                self.GoWrapper = new GoWrapper();
                self.Graph.SetNativeObject(self.GoWrapper);
            }
            
            // 缓存原生渲染器
            self.CacheRawRenderers();
            
            self.GoWrapper.SetWrapTarget(self.CurrentViewObject, false);
            
            // 激活视图对象
            self.CurrentViewObject.SetActive(self.IsActiveGameObjectOnInstantiate);
        }
    }
}