using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(FUIFullScreenComponent))]
    [FriendOf(typeof(FUIFullScreenComponent))]
    public static partial class FUIFullScreenComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FUIFullScreenComponent self)
        {
            FUI fui = self.GetParent<FUI>();
            GRoot root = GRoot.inst;

            fui.ContentPane.size = root.size;
            fui.ContentPane.xy = Vector2.zero;
            fui.ContentPane.AddRelation(root, RelationType.Size);
            
            self.SafeAreaObject = fui.ContentPane.GetChild("safeArea");
            if (self.SafeAreaObject != null)
            {
                self.SafeAreaObject.relations.ClearAll();
                self.UpdateSafeArea();
                
                root.AddEventListener(nameof(FUIScreenAdaptorChangedEvent), self.UpdateSafeArea);
            }
        }

        [EntitySystem]
        private static void Destroy(this FUIFullScreenComponent self)
        {
            FUI fui = self.GetParent<FUI>();
            GRoot root = GRoot.inst;
            
            fui.ContentPane.RemoveRelation(root, RelationType.Size);
            
            if (self.SafeAreaObject != null)
            {
                root.RemoveEventListener(nameof(FUIScreenAdaptorChangedEvent), self.UpdateSafeArea);
            }
        }
        
        private static void UpdateSafeArea(this FUIFullScreenComponent self)
        {
            if (self.SafeAreaObject == null)
            {
                return;
            }
            
            FUIScreenAdaptorComponent screenAdaptor = self.Scene().GetComponent<FUIScreenAdaptorComponent>();
            Rect safeArea = screenAdaptor.UISafeArea;
            
            self.SafeAreaObject.SetSize(safeArea.width, safeArea.height);
            self.SafeAreaObject.SetXY(safeArea.x, safeArea.y);
        }
    }
}