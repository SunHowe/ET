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
            self.AddComponent(eventHandler.ComponentType);
            self.OnCreate();
        }

        [EntitySystem]
        private static void Destroy(this FUI self)
        {
            self.OnDestroy();
            self.ContentPane.Dispose();
        }

        /// <summary>
        /// 设置深度
        /// </summary>
        public static void SetDepth(this FUI self, int depth)
        {
            self.ContentPane.sortingOrder = depth + 1;
        }

        /// <summary>
        /// 是否需要暂停覆盖的界面
        /// </summary>
        public static bool IsPauseCoveredUIForm(this FUI ui)
        {
            return ui.EventHandler.IsPauseCoveredUIForm(ui);
        }

        /// <summary>
        /// 是否需要显示遮罩层
        /// </summary>
        public static bool IsNeedDisplayMaskLayer(this FUI ui)
        {
            return ui.EventHandler.IsNeedDisplayMaskLayer(ui);
        }

        /// <summary>
        /// 界面创建时调用
        /// </summary>
        public static void OnCreate(this FUI self)
        {
            self.EventHandler.OnCreate(self);
        }

        /// <summary>
        /// 界面销毁时调用
        /// </summary>
        public static void OnDestroy(this FUI self)
        {
            self.EventHandler.OnDestroy(self);
        }

        /// <summary>
        /// 界面打开时调用
        /// </summary>
        public static void OnOpen(this FUI self, object userData)
        {
            self.EventHandler.OnOpen(self, userData);
        }

        /// <summary>
        /// 界面关闭时调用
        /// </summary>
        public static void OnClose(this FUI self)
        {
            self.EventHandler.OnClose(self);
        }

        /// <summary>
        /// 界面暂停
        /// </summary>
        public static void OnPause(this FUI self)
        {
            self.EventHandler.OnPause(self);
        }

        /// <summary>
        /// 界面恢复
        /// </summary>
        public static void OnResume(this FUI self)
        {
            self.EventHandler.OnResume(self);
        }

        /// <summary>
        /// 界面遮挡
        /// </summary>
        public static void OnCover(this FUI self)
        {
            self.EventHandler.OnCover(self);
        }

        /// <summary>
        /// 界面遮挡恢复
        /// </summary>
        public static void OnReveal(this FUI self)
        {
            self.EventHandler.OnReveal(self);
        }

        /// <summary>
        /// 界面激活
        /// </summary>
        public static void OnRefocus(this FUI self, object userdata)
        {
            self.EventHandler.OnRefocus(self, userdata);
        }

        /// <summary>
        /// 界面轮询
        /// </summary>
        public static void OnUpdate(this FUI self)
        {
            self.EventHandler.OnUpdate(self);
        }
    }
}