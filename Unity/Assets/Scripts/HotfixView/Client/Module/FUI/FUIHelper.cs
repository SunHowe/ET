namespace ET.Client
{
    public static class FUIHelper
    {
        /// <summary>
        /// 异步打开UI
        /// </summary>
        [EnableAccessEntiyChild]
        public static async ETTask<FUI> OpenUIAsync(this Entity scene, FUIViewId viewId, object userdata, ETCancellationToken cancellationToken)
        {
            return await scene.GetComponent<FUIComponent>().OpenUIAsync(viewId, userdata, cancellationToken);
        }

        public static ETTask<FUI> OpenUIAsync(this Entity scene, FUIViewId viewId, object userdata)
        {
            return scene.OpenUIAsync(viewId, userdata, default);
        }

        public static ETTask<FUI> OpenUIAsync(this Entity scene, FUIViewId viewId, ETCancellationToken cancellationToken)
        {
            return scene.OpenUIAsync(viewId, null, cancellationToken);
        }
        
        public static ETTask<FUI> OpenUIAsync(this Entity scene, FUIViewId viewId)
        {
            return scene.OpenUIAsync(viewId, null, default);
        }
        
        /// <summary>
        /// 获取已打开的UI
        /// </summary>
        [EnableAccessEntiyChild]
        public static FUI GetOpenedUI(Entity scene, FUIViewId viewId)
        {
            return scene.GetComponent<FUIComponent>().GetOpenedUI(viewId);
        }
        
        /// <summary>
        /// 关闭UI
        /// </summary>
        [EnableAccessEntiyChild]
        public static async ETTask CloseUI(Entity scene, FUIViewId viewId, bool includeOpening = true)
        {
            scene.GetComponent<FUIComponent>().CloseUI(viewId, includeOpening);
            await ETTask.CompletedTask;
        }
        
        /// <summary>
        /// 关闭所有UI
        /// </summary>
        [EnableAccessEntiyChild]
        public static async ETTask CloseAllUI(Entity scene, bool includeOpening = true)
        {
            scene.GetComponent<FUIComponent>().CloseAllUI(includeOpening);
            await ETTask.CompletedTask;
        }
        
        /// <summary>
        /// 销毁已关闭的UI
        /// </summary>
        [EnableAccessEntiyChild]
        public static async ETTask DestroyClosedUI(Entity scene)
        {
            scene.GetComponent<FUIComponent>().DestroyClosedUI();
            await ETTask.CompletedTask;
        }
        
        /// <summary>
        /// 卸载所有未使用的资源
        /// </summary>
        [EnableAccessEntiyChild]
        public static async ETTask UnloadAllUnusedAssets(Entity scene)
        {
            scene.GetComponent<FUIComponent>().UnloadAllUnusedAssets();
            await ETTask.CompletedTask;
        }
    }
}