namespace ET.Client
{
    /// <summary>
    /// FUI 屏幕居中组件 带有该组件的实体将会显示在屏幕中央
    /// </summary>
    [ComponentOf(typeof(FUI))]
    public sealed class FUIScreenCenterComponent : Entity, IAwake, IDestroy
    {
    }
}