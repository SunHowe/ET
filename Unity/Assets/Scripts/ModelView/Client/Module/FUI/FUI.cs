using FairyGUI;

namespace ET.Client
{
    [ChildOf]
    public sealed class FUI: Entity, IAwake<FUIViewId, GComponent, IFUIEventHandler>, IDestroy
    {
        public FUIViewId ViewId { get; set; }
        public GComponent ContentPane { get; set; }
		public IFUIEventHandler EventHandler { get; set; }
    }
}