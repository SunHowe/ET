namespace ET.Client
{
    [FUIEvent(FUIViewId.LoginForm)]
    public partial class LoginFormEventHandler : AFUIEventHandler<LoginForm>
    {
        public override string UIAssetURL => LoginForm.URL;
        public override FUIGroupId FUIGroupId => FUIGroupId.Main;
    }
}