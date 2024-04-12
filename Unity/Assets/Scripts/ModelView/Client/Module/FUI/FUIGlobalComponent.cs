using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public sealed class FUIGlobalComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<string, FUIGroup> UIGroups = new();
        public Dictionary<FUIViewId, IFUIEventHandler> EventHandlers = new();
    }
}