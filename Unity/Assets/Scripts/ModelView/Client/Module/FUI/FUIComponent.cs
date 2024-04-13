using System.Collections.Generic;
using FairyGUI.Dynamic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public sealed class FUIComponent : Entity, IAwake<string, string>, IDestroy
    {
        public string UIAssetKeyPrefix { get; set; }
        public string UIMappingAssetKey { get; set; }
        
        public Dictionary<FUIGroupId, EntityRef<FUIGroup>> UIGroups = new();
        public Dictionary<FUIViewId, IFUIEventHandler> EventHandlers = new();
        public Dictionary<FUIViewId, EntityRef<FUI>> UIDict = new();
        public Dictionary<FUIViewId, ETCancellationToken> UIAwaitDict = new();
        
        public IUIAssetManager UIAssetManager { get; set; }
        
        public List<EntityRef<FUI>> UIBuffer = new();
    }
}