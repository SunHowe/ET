using System;
using System.Threading;
using UnityEngine;

namespace ET.Client.Common
{
    public partial class GameObjectComponentSystem
    {
        /// <summary>
        /// 打断加载中的任务
        /// </summary>
        public static void InterruptLoading(this GameObjectComponent self)
        {
            if (!self.IsLoading)
                return;

            self.IsLoading = false;
            self.m_LoadingAssetKey = string.Empty;
            self.m_LoadingRendererType = GameObjectComponent.RendererType.None;
            
            self.m_LoadingToken?.Cancel();
            self.m_LoadingToken = null;
        }

        /// <summary>
        /// 等待当前加载任务完成
        /// </summary>
        public static ETTask AwaitLoadingTask(this GameObjectComponent self, ETCancellationToken token)
        {
            return self.GetFUIFormEntity().Root().GetComponent<TimerComponent>().WaitWhile(Predicate, token);

            bool Predicate() => self.IsLoading;
        }

        /// <summary>
        /// 发起新的加载任务
        /// </summary>
        private static async ETTask CreateNewLoadTask(this GameObjectComponent self, GameObjectComponent.RendererType rendererType, string assetKey)
        {
            self.IsLoading = true;
            self.m_LoadingAssetKey = assetKey;
            self.m_LoadingRendererType = rendererType;

            self.m_LoadingToken ??= new ETCancellationToken();
            ETCancellationToken token = self.m_LoadingToken;

            GameObject gameObject = rendererType switch
            {
                GameObjectComponent.RendererType.GoWrapper => await self.UpdateGoWrapperRenderer(assetKey, token),
                GameObjectComponent.RendererType.RenderTexture => await self.UpdateGoWrapperRenderer(assetKey, token),
                _ => throw new ArgumentOutOfRangeException(nameof(rendererType), rendererType, null)
            };

            if (token.IsCancel())
                return;
            
            // 无论成功与否 都需要销毁当前使用的旧视图
            self.DisposeCurrentView();
            
            // 清理加载中的资源标记
            self.m_LoadingAssetKey = string.Empty;
            self.m_LoadingRendererType = GameObjectComponent.RendererType.None;
            self.IsLoading = false;

            // 设置当前的资源key与渲染器类型
            self.CurrentAssetKey = assetKey;
            self.CurrentRendererType = rendererType;
            self.CurrentViewObject = gameObject;

            // 加载失败
            if (gameObject == null)
                return;

            // 更新视图对象类型
            self.SetupViewObject();
            
            // 加载成功 设置渲染器
            self.SetupRenderer();
            
            // 触发视图更新事件
            self.OnViewUpdated?.Invoke();
        }
    }
}