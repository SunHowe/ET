using FairyGUI;

namespace ET.Client
{
    [EntitySystemOf(typeof(FUIScreenCenterComponent))]
    [FriendOf(typeof(FUIScreenCenterComponent))]
    public static partial class FUIScreenCenterComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FUIScreenCenterComponent self)
        {
            FUI fui = self.GetParent<FUI>();
            GRoot root = GRoot.inst;
            
            fui.ContentPane.Center();
            
            fui.ContentPane.AddRelation(root, RelationType.Center_Center);
            fui.ContentPane.AddRelation(root, RelationType.Middle_Middle);
        }

        [EntitySystem]
        private static void Destroy(this FUIScreenCenterComponent self)
        {
            FUI fui = self.GetParent<FUI>();
            GRoot root = GRoot.inst;
            
            fui.ContentPane.RemoveRelation(root, RelationType.Center_Center);
            fui.ContentPane.RemoveRelation(root, RelationType.Middle_Middle);
        }
    }
}