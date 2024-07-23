namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class LoginFinish_CreateLobbyForm : AEvent<Scene, LoginFinish>
    {
        protected override async ETTask Run(Scene scene, LoginFinish args)
        {
            await scene.OpenUIAsync(FUIViewId.LobbyForm);
        }
    }
}
