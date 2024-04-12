using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 管理Scene上的UI
    /// </summary>
    [ComponentOf]
    public sealed class FUIComponent : Entity, IAwake
    {
        public Dictionary<string, FUI> UIs = new();
    }
}