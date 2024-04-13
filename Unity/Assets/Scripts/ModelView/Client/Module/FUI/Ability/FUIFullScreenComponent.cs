using FairyGUI;

namespace ET.Client
{
    /// <summary>
    /// FUI 全屏组件 带有该组件的实体将会全屏显示 并且会根据屏幕适配组件的安全区域进行适配
    /// </summary>
    [ComponentOf(typeof(FUI))]
    public sealed class FUIFullScreenComponent : Entity, IAwake, IDestroy
    {
        public GObject SafeAreaObject { get; set; }
    }
}