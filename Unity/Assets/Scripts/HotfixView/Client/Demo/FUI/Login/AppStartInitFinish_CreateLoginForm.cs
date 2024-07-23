namespace ET.Client
{
	[Event(SceneType.Demo)]
	public class AppStartInitFinish_CreateLoginForm: AEvent<Scene, AppStartInitFinish>
	{
		protected override async ETTask Run(Scene root, AppStartInitFinish args)
		{
			await root.OpenUIAsync(FUIViewId.LoginForm);
		}
	}
}
