namespace ET.Client
{
    [FUIEvent(FUIViewId.{{ name }})]
    public partial class {{ name }}EventHandler : AFUIEventHandler<{{ name }}>
    {
        public override string UIAssetURL => {{ name }}.URL;
        public override FUIGroupId FUIGroupId => FUIGroupId.Main;
    }
}