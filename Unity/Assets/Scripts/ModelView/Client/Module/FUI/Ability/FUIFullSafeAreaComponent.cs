namespace ET.Client
{
    /// <summary>
    /// FUI 填满安全区域组件 带有该组件的实体将会填满安全区域显示
    /// </summary>
    [ComponentOf(typeof(FUI))]
    public sealed class FUIFullSafeAreaComponent : Entity, IAwake, IDestroy
    {
    }
}