using FairyGUI;

namespace ET.Client
{
    public static partial class LobbyFormSystem
    {
        private static void OnCreate(this LobbyForm self, FUI ui, GComponent contentPane)
        {
            ui.AddComponent<FUIFullScreenComponent>();
            
            self.BtnEnter.onClick.Add(self.OnEnter);
        }
        
        private static void OnDestroy(this LobbyForm self)
        {
            self.BtnEnter.onClick.Remove(self.OnEnter);
        }

		public static void OnOpen(this LobbyForm self, FUI ui, object userData)
        {
        }

        public static void OnClose(this LobbyForm self, FUI ui)
        {
        }

        public static void OnPause(this LobbyForm self, FUI ui)
        {
        }

        public static void OnResume(this LobbyForm self, FUI ui)
        {
        }

        public static void OnCover(this LobbyForm self, FUI ui)
        {
        }

        public static void OnReveal(this LobbyForm self, FUI ui)
        {
        }

        public static void OnRefocus(this LobbyForm self, FUI ui, object userData)
        {
        }

        private static void OnEnter(this LobbyForm self)
        {
            self.EnterMap().Coroutine();
        }

        private static async ETTask EnterMap(this LobbyForm self)
        {
            Scene root = self.Root();
            await EnterMapHelper.EnterMapAsync(root);
            await root.CloseUI(FUIViewId.LobbyForm);
        }
    }
}