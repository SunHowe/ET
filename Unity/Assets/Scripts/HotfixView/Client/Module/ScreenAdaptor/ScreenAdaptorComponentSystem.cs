using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(ScreenAdaptorComponent))]
    [FriendOf(typeof(ScreenAdaptorComponent))]
    public static partial class ScreenAdaptorComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ScreenAdaptorComponent self)
        {
            self.SafeArea = Screen.safeArea;
        }
        
        private static void UpdateSafeArea(this ScreenAdaptorComponent self)
        {
            Rect safeArea = Screen.safeArea;
            if (self.SafeArea == safeArea)
            {
                return;
            }

            self.SafeArea = safeArea;
            EventSystem.Instance.Publish(self.Scene(), new ScreenSafeAreaUpdatedEvent { SafeArea = safeArea });
        }
    }
}