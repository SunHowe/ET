using System;
using System.Collections.Generic;
using System.IO;
using FairyGUI;

namespace ET.Client
{
    [Event(SceneType.Main)]
    public class EntryEvent3_InitClient: AEvent<Scene, EntryEvent3>
    {
        protected override async ETTask Run(Scene root, EntryEvent3 args)
        {
            FUIConfig.EnableBlurMaskLayer = true;
            
            GlobalComponent globalComponent = root.AddComponent<GlobalComponent>();
            root.AddComponent<ResourcesLoaderComponent>();
            root.AddComponent<FUIComponent, string, string>("Assets/Bundles/FUI/", "Assets/Bundles/FUI/UIPackageMapping.asset");
            root.AddComponent<PlayerComponent>();
            root.AddComponent<CurrentScenesComponent>();
            root.AddComponent<ScreenAdaptorComponent>();
            root.AddComponent<FUIScreenAdaptorComponent, UIContentScaler.ScreenMatchMode, int, int>(UIContentScaler.ScreenMatchMode.MatchWidthOrHeight, 1280, 720);
            
            // 根据配置修改掉Main Fiber的SceneType
            SceneType sceneType = EnumHelper.FromString<SceneType>(globalComponent.GlobalConfig.AppType.ToString());
            root.SceneType = sceneType;

            // 初始化UI组件
            await root.GetComponent<FUIComponent>().InitializeAsync();
            
            await EventSystem.Instance.PublishAsync(root, new AppStartInitFinish());
        }
    }
}