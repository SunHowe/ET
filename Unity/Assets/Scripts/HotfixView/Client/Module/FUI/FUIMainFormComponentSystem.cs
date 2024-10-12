
namespace ET.Client
{
    [EntitySystemOf(typeof(FUIMainFormComponent))]
    [FriendOf(typeof(FUIMainFormComponent))]
    public static partial class FUIMainFormComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FUIMainFormComponent self)
        {
            FUI fui = self.GetParent<FUI>();

            fui.PauseCoveredUIForm = true;
        }
    }
}
