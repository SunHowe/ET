namespace ET.Client
{
    public static class FUIHelper
    {
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
    }
}