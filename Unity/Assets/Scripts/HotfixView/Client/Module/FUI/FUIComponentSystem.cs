using System;
using System.Collections.Generic;
using FairyGUI;
using FairyGUI.Dynamic;

namespace ET.Client
{
    [EntitySystemOf(typeof(FUIComponent))]
    [FriendOf(typeof(FUIComponent))]
    public static partial class FUIComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FUIComponent self, string uiAssetKeyPrefix, string uiMappingAssetKey)
        {
            Type fuiFormType = typeof(FUIForm);
            
            var uiEvents = CodeTypes.Instance.GetTypes(typeof(FUIEventAttribute));
            foreach (Type type in uiEvents)
            {
                object[] attrs = type.GetCustomAttributes(typeof(FUIEventAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                FUIEventAttribute uiEventAttribute = attrs[0] as FUIEventAttribute;
                IFUIEventHandler aUIEvent = Activator.CreateInstance(type) as IFUIEventHandler;
                self.EventHandlers.Add(uiEventAttribute.ViewId, aUIEvent);
                
                // 将界面注册到FUIForm实例
                UIObjectFactory.SetPackageItemExtension(aUIEvent.UIAssetURL, fuiFormType);
            }
            
            var uiCustomComponents = CodeTypes.Instance.GetTypes(typeof(FUICustomComponentAttribute));
            foreach (Type type in uiCustomComponents)
            {
                object[] attrs = type.GetCustomAttributes(typeof(FUICustomComponentAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                FUICustomComponentAttribute uiCustomComponentAttribute = attrs[0] as FUICustomComponentAttribute;
                UIObjectFactory.SetPackageItemExtension(uiCustomComponentAttribute.URL, type);
            }
            
            self.UIAssetKeyPrefix = uiAssetKeyPrefix;
            self.UIMappingAssetKey = uiMappingAssetKey;
        }

        [EntitySystem]
        private static void Destroy(this FUIComponent self)
        {
            UIObjectFactory.Clear();
            
            if (self.UIAssetManager == null)
                return;
            
            self.UIAssetManager.Dispose();
        }

        /// <summary>
        /// 初始化UI组件
        /// </summary>
        public static async ETTask InitializeAsync(this FUIComponent self)
        {
            ResourcesLoaderComponent resourcesLoaderComponent = self.Root().GetComponent<ResourcesLoaderComponent>();
            
            UIPackageMapping packageMapping = await resourcesLoaderComponent.LoadAssetAsync<UIPackageMapping>(self.UIMappingAssetKey);
            FUIAssetLoader assetLoader = new FUIAssetLoader(self);
            
            self.UIAssetManager = new UIAssetManager();
            self.UIAssetManager.Initialize(new FUIAssetManagerConfiguration(packageMapping, assetLoader));

            for (FUIGroupId id = 0; id < FUIGroupId.Count; id++)
            {
                FUIGroup group = self.AddChild<FUIGroup, FUIGroupId>(id);
                self.UIGroups.Add(id, group);
            }
        }

        /// <summary>
        /// 获取UI组
        /// </summary>
        public static FUIGroup GetGroup(this FUIComponent self, FUIGroupId group)
        {
            return self.UIGroups.GetValueOrDefault(group);
        }

        /// <summary>
        /// 获取界面事件处理器
        /// </summary>
        private static IFUIEventHandler GetEventHandler(this FUIComponent self, FUIViewId viewId)
        {
            return self.EventHandlers.GetValueOrDefault(viewId);
        }

        /// <summary>
        /// 获取已打开的UI
        /// </summary>
        public static FUI GetOpenedUI(this FUIComponent self, FUIViewId viewId)
        {
            FUI fui = self.GetLoadedUI(viewId);
            if (fui == null)
            {
                return null;
            }
            
            FUIGroup fuiGroup = self.GetGroup(fui.FUIGroupId);
            if (fuiGroup == null)
            {
                return null;
            }

            return fuiGroup.HasUI(fui) ? fui : null;
        }

        /// <summary>
        /// 异步打开UI
        /// </summary>
        public static async ETTask<FUI> OpenUIAsync(this FUIComponent self, FUIViewId viewId, object userdata = null, ETCancellationToken cancellationToken = null)
        {
            ETCancellationToken awaitCancellationToken = self.GetAwaitCancellationToken(viewId);

            // 处理相同UI的并发打开
            using CoroutineLock _ = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.UI, (long)viewId);

            if (cancellationToken.IsCancel() || awaitCancellationToken.IsCancel())
            {
                return null;
            }

            IFUIEventHandler eventHandler = self.GetEventHandler(viewId);
            if (eventHandler == null)
            {
                Log.Error($"未找到UI事件处理器: {viewId}");
                return null;
            }

            FUI fui = self.GetLoadedUI(viewId);
            while (true)
            {
                if (fui == null)
                {
                    fui = await self.LoadFUIAsync(viewId, eventHandler);
                
                    if (fui == null)
                    {
                        return null;
                    }
                
                    if (cancellationToken.IsCancel() || awaitCancellationToken.IsCancel())
                    {
                        return null;
                    }
                }

                // 处理全局UI的并发打开
                using CoroutineLock __ = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.UI, 0);

                if (cancellationToken.IsCancel() || awaitCancellationToken.IsCancel())
                {
                    return null;
                }
                
                // 再次检查是否已加载
                fui = self.GetLoadedUI(viewId);

                // 在等待协程锁的过程中界面实例被销毁了 但由于请求未取消，这里需要重新加载
                if (fui == null)
                {
                    continue;
                }

                FUIGroup uiGroup = self.GetGroup(fui.FUIGroupId);
                if (uiGroup == null)
                {
                    return null;
                }
                
                if (uiGroup.HasUI(fui))
                {
                    // 已经打开的UI 重新聚焦
                    uiGroup.RefocusUI(fui);
                    uiGroup.Refresh();

                    fui.OnRefocus(userdata);
                    return fui;
                }

                // 未打开的UI
                uiGroup.AddUI(fui);
                uiGroup.Refresh();
                return fui;
            }
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        public static void CloseUI(this FUIComponent self, FUIViewId viewId, bool includeOpening = true)
        {
            CloseUIAsync(self, viewId, includeOpening).Coroutine();
        }

        /// <summary>
        /// 关闭所有UI
        /// </summary>
        public static void CloseAllUI(this FUIComponent self, bool includeOpening = true)
        {
            CloseAllUIAsync(self, includeOpening).Coroutine();
        }
        
        /// <summary>
        /// 销毁已关闭的UI
        /// </summary>
        public static void DestroyClosedUI(this FUIComponent self)
        {
            self.UIBuffer.Clear();
            
            foreach (FUI fui in self.UIDict.Values)
            {
                FUIGroup uiGroup = self.GetGroup(fui.FUIGroupId);

                if (uiGroup == null || !uiGroup.HasUI(fui))
                {
                    self.UIBuffer.Add(fui);
                }
            }
            
            foreach (FUI fui in self.UIBuffer)
            {
                self.UIDict.Remove(fui.ViewId);
                fui.Dispose();
            }
        }
        
        /// <summary>
        /// 卸载所有未使用的资源
        /// </summary>
        public static void UnloadAllUnusedAssets(this FUIComponent self)
        {
            UIPackage.UnloadAllUnusedAssets();
        }

        /// <summary>
        /// 加载UI
        /// </summary>
        private static async ETTask<FUI> LoadFUIAsync(this FUIComponent self, FUIViewId viewId, IFUIEventHandler eventHandler)
        {
            ETTask<GObject> task = ETTask<GObject>.Create(true);
            UIPackage.CreateObjectFromURLAsync(eventHandler.UIAssetURL, task.SetResult);
            
            GObject gObject = await task;
            
            if (gObject == null)
            {
                Log.Error($"加载UI失败: {viewId}");
                return null;
            }
            
            FUIForm fuiForm = gObject as FUIForm;
            if (fuiForm == null)
            {
                Log.Error($"UI实例类型不是FUIForm: {viewId}");
                gObject.Dispose();
                return null;
            }

            try
            {
                FUI fui = self.AddChild<FUI, FUIViewId, FUIForm, IFUIEventHandler>(viewId, fuiForm, eventHandler);
                self.UIDict.Add(viewId, fui);
                return fui;
            }
            catch (Exception e)
            {
                Log.Error($"创建UI失败: {viewId}\n{e}");
                gObject.Dispose();
                return null;
            }
        }
        
        /// <summary>
        /// 关闭UI
        /// </summary>
        private static async ETTask CloseUIAsync(this FUIComponent self, FUIViewId viewId, bool includeOpening = true)
        {
            // 中止打开中的UI
            if (includeOpening)
                RemoveAwaitCancellationToken(self, viewId);
            
            FUI fui = self.GetLoadedUI(viewId);
            if (fui == null)
            {
                return;
            }
            
            FUIGroup uiGroup = self.GetGroup(fui.FUIGroupId);
            if (uiGroup == null)
            {
                return;
            }

            // 未包含UI则忽略
            if (!uiGroup.HasUI(viewId))
            {
                return;
            }

            // 关闭UI需要等待全局的协程锁
            using CoroutineLock _ = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.UI, 0);

            fui = uiGroup.GetUI(viewId);
            if (fui == null)
            {
                return;
            }

            // 关闭UI
            fui.OnClose();
            uiGroup.RemoveUI(fui);
            uiGroup.Refresh();
        }
        
        /// <summary>
        /// 关闭所有UI
        /// </summary>
        private static async ETTask CloseAllUIAsync(this FUIComponent self, bool includeOpening = true)
        {
            // 销毁所有等待的令牌
            foreach (ETCancellationToken cancellationToken in self.UIAwaitDict.Values)
            {
                cancellationToken.Cancel();
            }
            
            self.UIAwaitDict.Clear();
            
            // 关闭所有UI 需要等待全局的协程锁
            using CoroutineLock _ = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.UI, 0);
            
            foreach (FUIGroup uiGroup in self.UIGroups.Values)
            {
                if (uiGroup.GetUIList(self.UIBuffer) == 0)
                    continue;

                foreach (FUI fui in self.UIBuffer)
                {
                    fui.OnClose();
                    uiGroup.RemoveUI(fui);
                }
                
                uiGroup.Refresh();
            }
        }

        /// <summary>
        /// 获取已加载完成的UI
        /// </summary>
        private static FUI GetLoadedUI(this FUIComponent self, FUIViewId viewId)
        {
            return self.UIDict.GetValueOrDefault(viewId);
        }

        /// <summary>
        /// 获取指定界面的等待取消令牌
        /// </summary>
        private static ETCancellationToken GetAwaitCancellationToken(this FUIComponent self, FUIViewId viewId)
        {
            ETCancellationToken cancellationToken = self.UIAwaitDict.GetValueOrDefault(viewId);

            if (cancellationToken != null)
            {
                return cancellationToken;
            }

            cancellationToken = new ETCancellationToken();
            self.UIAwaitDict.Add(viewId, cancellationToken);

            return cancellationToken;
        }

        /// <summary>
        /// 移除指定界面的等待取消令牌
        /// </summary>
        private static void RemoveAwaitCancellationToken(this FUIComponent self, FUIViewId viewId)
        {
            ETCancellationToken cancellationToken = self.UIAwaitDict.GetValueOrDefault(viewId);

            if (cancellationToken == null)
            {
                return;
            }

            self.UIAwaitDict.Remove(viewId);
            cancellationToken.Cancel();
        }
    }
}