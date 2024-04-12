using FairyGUI;

namespace ET.Client
{
    [EntitySystemOf(typeof(FUI))]
    [FriendOf(typeof(FUI))]
    public static partial class FUISystem
    {
        [EntitySystem]
        private static void Awake(this FUI self, FUIViewId viewId, GComponent contentPane, IFUIEventHandler eventHandler)
        {
            self.ViewId = viewId;
            self.ContentPane = contentPane;
            self.EventHandler = eventHandler;
            self.EventHandler.OnCreate(self);
        }
        
        [EntitySystem]
        private static void Destroy(this FUI self)
        {
            self.EventHandler.OnDestroy(self);
            self.ContentPane.Dispose();
        }
    }
}