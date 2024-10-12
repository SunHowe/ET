using FairyGUI;

namespace ET.Client
{
    public static partial class {{ name }}System
    {
        private static void OnCreate(this {{ name }} self, FUI ui, GComponent contentPane)
        {
            ui.AddComponent<FUIFullScreenComponent>();
            ui.AddComponent<FUIMainFormComponent>();
        }
        
        private static void OnDestroy(this {{ name }} self)
        {
        }

		public static void OnOpen(this {{ name }} self, FUI ui, object userData)
        {
        }

        public static void OnClose(this {{ name }} self, FUI ui)
        {
        }

        public static void OnPause(this {{ name }} self, FUI ui)
        {
        }

        public static void OnResume(this {{ name }} self, FUI ui)
        {
        }

        public static void OnCover(this {{ name }} self, FUI ui)
        {
        }

        public static void OnReveal(this {{ name }} self, FUI ui)
        {
        }

        public static void OnRefocus(this {{ name }} self, FUI ui, object userData)
        {
        }
    }
}