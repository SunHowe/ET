﻿namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class LoginFinish_RemoveLoginForm : AEvent<Scene, LoginFinish>
    {
        protected override async ETTask Run(Scene scene, LoginFinish args)
        {
            await scene.CloseUI(FUIViewId.LoginForm);
        }
    }
}
