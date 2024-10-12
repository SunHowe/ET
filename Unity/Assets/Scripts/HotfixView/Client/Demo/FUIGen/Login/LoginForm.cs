﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FairyGUI;

namespace ET.Client
{
    [EntitySystemOf(typeof(LoginForm))]
    [FriendOf(typeof(LoginForm))]
    public static partial class LoginFormSystem
    {
		[EntitySystem]
        public static void Awake(this LoginForm self)
        {
            FUI fui = self.GetParent<FUI>();
            GComponent contentPane = fui.ContentPane;

            #region [子节点]
     
            self.BtnLogin = (FairyGUI.GButton)contentPane.GetChildAt(2);
    
            self.InputAccount = (FairyGUI.GTextInput)contentPane.GetChildAt(4);
  
            self.InputPassword = (FairyGUI.GTextInput)contentPane.GetChildAt(5);
  
            self.ComDemo = (ET.Client.Login.DemoComponent)contentPane.GetChildAt(6);
  
            self.BtnSpecial = (ET.Client.Login.SpecialButton)contentPane.GetChildAt(7);
 
            #endregion

            #region [控制器]

            #endregion

            #region [动效]

            #endregion

            self.OnCreate(fui, contentPane);
        }

        [EntitySystem]
        public static void Destroy(this LoginForm self)
        {
            self.OnDestroy();
        }
    }

    [FUIEvent(FUIViewId.LoginForm)]
    public partial class LoginFormEventHandler : AFUIEventHandler<LoginForm>
    {
        public override string UIAssetURL => LoginForm.URL;

        protected override void OnOpen(FUI ui, LoginForm component, object userData)
        {
            component.OnOpen(ui, userData);
        }

        protected override void OnClose(FUI ui, LoginForm component)
        {
            component.OnClose(ui);
        }

        protected override void OnPause(FUI ui, LoginForm component)
        {
            component.OnPause(ui);
        }

        protected override void OnResume(FUI ui, LoginForm component)
        {
            component.OnResume(ui);
        }

        protected override void OnCover(FUI ui, LoginForm component)
        {
            component.OnCover(ui);
        }

        protected override void OnReveal(FUI ui, LoginForm component)
        {
            component.OnReveal(ui);
        }

        protected override void OnRefocus(FUI ui, LoginForm component, object userData)
        {
            component.OnRefocus(ui, userData);
        }

        protected override void OnMaskLayerClicked(FUI ui, LoginForm component)
        {
            component.OnMaskLayerClicked(ui);
        }
    }
}