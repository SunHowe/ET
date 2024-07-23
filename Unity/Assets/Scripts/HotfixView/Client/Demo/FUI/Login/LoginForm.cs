using FairyGUI;

namespace ET.Client
{
    public static partial class LoginFormSystem
    {
        private static void OnCreate(this LoginForm self, FUI ui, GComponent contentPane)
        {
            ui.AddComponent<FUIFullScreenComponent>();
            
            self.BtnLogin.onClick.Add(self.OnLogin);
        }
        
        private static void OnDestroy(this LoginForm self)
        {
            self.BtnLogin.onClick.Remove(self.OnLogin);
        }

        public static void OnOpen(this LoginForm self, FUI ui, object userData)
        {
        }

        public static void OnClose(this LoginForm self, FUI ui)
        {
        }

        public static void OnPause(this LoginForm self, FUI ui)
        {
        }

        public static void OnResume(this LoginForm self, FUI ui)
        {
        }

        public static void OnCover(this LoginForm self, FUI ui)
        {
        }

        public static void OnReveal(this LoginForm self, FUI ui)
        {
        }

        public static void OnRefocus(this LoginForm self, FUI ui, object userData)
        {
        }

        private static void OnLogin(this LoginForm self)
        {
            LoginHelper.Login(self.Root(), self.InputAccount.text, self.InputPassword.text).Coroutine();
        }
    }
}