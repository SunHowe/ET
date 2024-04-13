using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(FUIFullSafeAreaComponent))]
    [FriendOf(typeof(FUIFullSafeAreaComponent))]
    public static partial class FUIFullSafeAreaComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FUIFullSafeAreaComponent self)
        {
            FUI fui = self.GetParent<FUI>();
            GRoot root = GRoot.inst;
            
            fui.ContentPane.relations.ClearAll();
            
            self.UpdateSafeArea();
            root.AddEventListener(nameof(FUIScreenAdaptorChangedEvent), self.UpdateSafeArea);
        }

        [EntitySystem]
        private static void Destroy(this FUIFullSafeAreaComponent self)
        {
            GRoot root = GRoot.inst;
            
            root.RemoveEventListener(nameof(FUIScreenAdaptorChangedEvent), self.UpdateSafeArea);
        }
        
        private static void UpdateSafeArea(this FUIFullSafeAreaComponent self)
        {
            FUI fui = self.GetParent<FUI>();
            FUIScreenAdaptorComponent screenAdaptor = self.Scene().GetComponent<FUIScreenAdaptorComponent>();
            Rect safeArea = screenAdaptor.UISafeArea;
            
            fui.ContentPane.SetSize(safeArea.width, safeArea.height);
            fui.ContentPane.SetXY(safeArea.x, safeArea.y);
        }
    }
}