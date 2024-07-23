using System.Collections.Generic;

namespace ET.Client.Common
{
    public static partial class GameObjectComponentSystem
    {
        /// <summary>
        /// 使用GoWrapper渲染器更新视图
        /// </summary>
        public static void UpdateGoWrapperView(this GameObjectComponent self, string assetKey)
        {
            self.UpdateGoWrapperView(assetKey, null).Coroutine();
        }

        /// <summary>
        /// 使用GoWrapper渲染器更新视图
        /// </summary>
        public static ETTask UpdateGoWrapperView(this GameObjectComponent self, string assetKey, ETCancellationToken token)
        {
            return self.UpdateView(GameObjectComponent.RendererType.GoWrapper, assetKey, token);
        }

        /// <summary>
        /// 使用RenderTexture渲染器更新视图
        /// </summary>
        public static void UpdateRenderTextureView(this GameObjectComponent self, string assetKey)
        {
            self.UpdateRenderTextureView(assetKey, null).Coroutine();
        }

        /// <summary>
        /// 使用RenderTexture渲染器更新视图
        /// </summary>
        public static ETTask UpdateRenderTextureView(this GameObjectComponent self, string assetKey, ETCancellationToken token)
        {
            return self.UpdateView(GameObjectComponent.RendererType.RenderTexture, assetKey, token);
        }

        /// <summary>
        /// 更新视图
        /// </summary>
        public static async ETTask UpdateView(this GameObjectComponent self, GameObjectComponent.RendererType rendererType, string assetKey, ETCancellationToken token)
        {
            // 更新自动加载的设置
            self.m_AutoLoadAssetKey = assetKey;
            self.m_AutoLoadRendererType = rendererType;

            if (string.IsNullOrEmpty(assetKey) || rendererType == GameObjectComponent.RendererType.None)
            {
                // 如果要加载的资源为空，则销毁当前的渲染器并打断当前进行中的加载任务
                self.InterruptLoading();
                self.DisposeCurrentView();
                return;
            }

            // 和当前加载中的资源一样 则等待加载完成即可
            if (self.IsLoading && self.m_LoadingAssetKey == assetKey && self.m_LoadingRendererType == rendererType)
            {
                await self.AwaitLoadingTask(token);
                return;
            }

            // 打断进行中的加载任务
            self.InterruptLoading();

            // 和当前显示中的资源一样 则直接返回
            if (self.CurrentAssetKey == assetKey && self.CurrentRendererType == rendererType)
            {
                return;
            }

            // 发起新的加载任务
            self.CreateNewLoadTask(rendererType, assetKey).Coroutine();

            // 等待加载任务完成
            await self.AwaitLoadingTask(token);
        }

        public static void OnInitialize(this GameObjectComponent self)
        {
            self.onAddedToStage.Add(self.OnAddedToStage);
            self.onRemovedFromStage.Add(self.OnRemoveFromStage);
        }

        public static void OnDispose(this GameObjectComponent self)
        {
            self.onAddedToStage.Remove(self.OnAddedToStage);
            self.onRemovedFromStage.Remove(self.OnRemoveFromStage);
            self.InterruptLoading();
            self.DisposeCurrentView();
            self.DisposeGoWrapper();
        }

        private static void DisposeCurrentView(this GameObjectComponent self)
        {
            self.DisposeRenderer();
            self.DisposeViewObject();
        }

        private static void OnAddedToStage(this GameObjectComponent self)
        {
            self.Graph.pivot = self.pivot;
            self.InitEditorSettings();
            self.onSizeChanged.Add(self.UpdateResizeScale);
            self.UpdateResizeScale();
            self.UpdateViewWithAutoLoadConfig();
        }

        private static void OnRemoveFromStage(this GameObjectComponent self)
        {
            if (!self.IsDisposeRendererOnRemovedStage)
                return;

            self.InterruptLoading();
            self.DisposeCurrentView();
        }

        private static void InitEditorSettings(this GameObjectComponent self)
        {
            if (self.IsInitEditorSettings)
                return;

            self.IsInitEditorSettings = true;
            string str = self.data as string;

            if (string.IsNullOrEmpty(str))
                return;

            var jsonObject = MongoHelper.FromJson<Dictionary<string, object>>(str);
            if (jsonObject == null)
                return;

            const string JsonKey_ResizeMode = "GO_ResizeMode";
            const string JsonKey_ResizeWithInitSize = "GO_ResizeWithInitSize";
            const string JsonKey_ObjectPoolType = "GO_ObjectPoolType";

            object value;

            #region [缩放模式]

            if (jsonObject.TryGetValue(JsonKey_ResizeMode, out value))
            {
                self.CurrentResizeMode = value switch
                {
                    int intValue => (GameObjectComponent.ResizeMode)intValue,
                    double doubleValue => (GameObjectComponent.ResizeMode)(int)doubleValue,
                    _ => GameObjectComponent.ResizeMode.None,
                };
            }
            else
            {
                self.CurrentResizeMode = GameObjectComponent.ResizeMode.None;
            }

            #endregion

            #region [缩放基准]

            self.IsResizeWithInitSize = jsonObject.TryGetValue(JsonKey_ResizeWithInitSize, out value) && value is bool b2 && b2;

            #endregion

            #region [对象池类型]

            // if (jsonObject.TryGetValue(JsonKey_ObjectPoolType, out value))
            // {
            //     CurrentObjectPoolMode = value switch
            //     {
            //         int intValue => (ObjectPoolMode)intValue,
            //         double doubleValue => (ObjectPoolMode)(int)doubleValue,
            //         _ => ObjectPoolMode.Form,
            //     };
            // }

            #endregion

            #region [自动加载配置]

            self.DeserializeAutoLoadConfig(jsonObject);

            #endregion
        }
    }
}