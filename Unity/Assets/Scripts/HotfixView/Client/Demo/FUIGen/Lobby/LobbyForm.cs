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
    [EntitySystemOf(typeof(LobbyForm))]
    [FriendOf(typeof(LobbyForm))]
    public static partial class LobbyFormSystem
    {
		[EntitySystem]
        public static void Awake(this LobbyForm self)
        {
            FUI fui = self.GetParent<FUI>();
            GComponent contentPane = fui.ContentPane;

            #region [子节点]
 
            self.BtnEnter = (FairyGUI.GButton)contentPane.GetChildAt(0);
 
            #endregion

            #region [控制器]

            #endregion

            #region [动效]

            #endregion

            self.OnCreate(fui, contentPane);
        }

        [EntitySystem]
        public static void Destroy(this LobbyForm self)
        {
            self.OnDestroy();
        }
    }

    [FUIEvent(FUIViewId.LobbyForm)]
    public partial class LobbyFormEventHandler : AFUIEventHandler<LobbyForm>
    {
        public override string UIAssetURL => LobbyForm.URL;

        protected override void OnOpen(FUI ui, LobbyForm component, object userData)
        {
            component.OnOpen(ui, userData);
        }

        protected override void OnClose(FUI ui, LobbyForm component)
        {
            component.OnClose(ui);
        }

        protected override void OnPause(FUI ui, LobbyForm component)
        {
            component.OnPause(ui);
        }

        protected override void OnResume(FUI ui, LobbyForm component)
        {
            component.OnResume(ui);
        }

        protected override void OnCover(FUI ui, LobbyForm component)
        {
            component.OnCover(ui);
        }

        protected override void OnReveal(FUI ui, LobbyForm component)
        {
            component.OnReveal(ui);
        }

        protected override void OnRefocus(FUI ui, LobbyForm component, object userData)
        {
            component.OnRefocus(ui, userData);
        }

        protected override void OnMaskLayerClicked(FUI ui, LobbyForm component)
        {
            component.OnMaskLayerClicked(ui);
        }
    }
}