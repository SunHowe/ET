using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(FUIScreenAdaptorComponent))]
    [FriendOf(typeof(FUIScreenAdaptorComponent))]
    public static partial class FUIScreenAdaptorComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FUIScreenAdaptorComponent self, FairyGUI.UIContentScaler.ScreenMatchMode screenMatchMode, int designResolutionX, int designResolutionY)
        {
            self.ScreenMatchMode = screenMatchMode;
            self.DesignResolutionX = designResolutionX;
            self.DesignResolutionY = designResolutionY;

            GRoot.inst.SetContentScaleFactor(designResolutionX, designResolutionY, screenMatchMode);
            GRoot.inst.onSizeChanged.Add(self.UpdateSafeArea);

            self.UISafeArea = self.CalculateSafeArea();
        }
        
        [EntitySystem]
        private static void Destroy(this ET.Client.FUIScreenAdaptorComponent self)
        {
            GRoot.inst.onSizeChanged.Remove(self.UpdateSafeArea);
        }

        /// <summary>
        /// 更新 UI 安全区域
        /// </summary>
        public static void UpdateSafeArea(this FUIScreenAdaptorComponent self)
        {
            Rect safeArea = self.CalculateSafeArea();
            if (self.UISafeArea == safeArea)
            {
                return;
            }

            self.UISafeArea = safeArea;
        }

        public static Rect CalculateSafeArea(this FUIScreenAdaptorComponent self)
        {
            ScreenAdaptorComponent screenAdaptorComponent = self.Scene().GetComponent<ScreenAdaptorComponent>();
            Rect safeArea = screenAdaptorComponent.SafeArea;
            float factor = 1f / GRoot.contentScaleFactor;
            safeArea.x = Mathf.CeilToInt(safeArea.x * factor);
            safeArea.y = Mathf.CeilToInt(safeArea.y * factor);
            safeArea.width = Mathf.CeilToInt(safeArea.width * factor);
            safeArea.height = Mathf.CeilToInt(safeArea.height * factor);

            return safeArea;
        }
    }
}