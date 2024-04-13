namespace ET.Client
{
    [Event(SceneType.Main)]
    public class ScreenSafeAreaUpdated_UpdateUISafeArea: AEvent<Scene, ScreenSafeAreaUpdatedEvent>
    {
        protected override async ETTask Run(Scene scene, ScreenSafeAreaUpdatedEvent a)
        {
            FUIScreenAdaptorComponent fuiScreenAdaptorComponent = scene.GetComponent<FUIScreenAdaptorComponent>();
            fuiScreenAdaptorComponent.UpdateSafeArea();
            await ETTask.CompletedTask;
        }
    }
}