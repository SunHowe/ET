namespace ET.Client
{
    public static class FUIHelper
    {
        /// <summary>
        /// 异步打开UI
        /// </summary>
        [EnableAccessEntiyChild]
        public static async ETTask<FUI> OpenUIAsync(this Scene scene, FUIViewId viewId, object userdata, ETCancellationToken cancellationToken)
        {
            return await scene.Root().GetComponent<FUIComponent>().OpenUIAsync(viewId, userdata, cancellationToken);
        }

        public static ETTask<FUI> OpenUIAsync(this Scene scene, FUIViewId viewId, object userdata)
        {
            return scene.OpenUIAsync(viewId, userdata, default);
        }

        public static ETTask<FUI> OpenUIAsync(this Scene scene, FUIViewId viewId, ETCancellationToken cancellationToken)
        {
            return scene.OpenUIAsync(viewId, null, cancellationToken);
        }
        
        public static ETTask<FUI> OpenUIAsync(this Scene scene, FUIViewId viewId)
        {
            return scene.OpenUIAsync(viewId, null, default);
        }
        
        /// <summary>
        /// 获取已打开的UI
        /// </summary>
        [EnableAccessEntiyChild]
        public static FUI GetOpenedUI(this Scene scene, FUIViewId viewId)
        {
            return scene.Root().GetComponent<FUIComponent>().GetOpenedUI(viewId);
        }
        
        /// <summary>
        /// 关闭UI
        /// </summary>
        [EnableAccessEntiyChild]
        public static async ETTask CloseUI(this Scene scene, FUIViewId viewId, bool includeOpening = true)
        {
            scene.Root().GetComponent<FUIComponent>().CloseUI(viewId, includeOpening);
            await ETTask.CompletedTask;
        }
        
        /// <summary>
        /// 关闭所有UI
        /// </summary>
        [EnableAccessEntiyChild]
        public static async ETTask CloseAllUI(this Scene scene, bool includeOpening = true)
        {
            scene.Root().GetComponent<FUIComponent>().CloseAllUI(includeOpening);
            await ETTask.CompletedTask;
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        [EnableAccessEntiyChild]
        public static ETTask RemoveUI(this Scene scene, FUIViewId viewId, bool includeOpening = true)
        {
            return scene.CloseUI(viewId, includeOpening);
        }
        
        /// <summary>
        /// 关闭所有UI
        /// </summary>
        [EnableAccessEntiyChild]
        public static ETTask RemoveAllUI(this Scene scene, bool includeOpening = true)
        {
            return scene.CloseAllUI(includeOpening);
        }
        
        /// <summary>
        /// 销毁已关闭的UI
        /// </summary>
        [EnableAccessEntiyChild]
        public static async ETTask DestroyClosedUI(this Scene scene)
        {
            scene.Root().GetComponent<FUIComponent>().DestroyClosedUI();
            await ETTask.CompletedTask;
        }
        
        /// <summary>
        /// 卸载所有未使用的资源
        /// </summary>
        [EnableAccessEntiyChild]
        public static async ETTask UnloadAllUnusedAssets(this Scene scene)
        {
            scene.Root().GetComponent<FUIComponent>().UnloadAllUnusedAssets();
            await ETTask.CompletedTask;
        }
    }
}