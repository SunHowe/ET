using System.Collections.Generic;
using FairyGUI.Dynamic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public sealed class FUIComponent : Entity, IAwake<string, string>, IDestroy
    {
        public string UIAssetKeyPrefix { get; set; }
        public string UIMappingAssetKey { get; set; }
        
        public Dictionary<FUIGroupId, FUIGroup> UIGroups = new();
        public Dictionary<FUIViewId, IFUIEventHandler> EventHandlers = new();
        public Dictionary<FUIViewId, FUI> UIDict = new();
        public Dictionary<FUIViewId, ETCancellationToken> UIAwaitDict = new();
        
        public IUIAssetManagerConfiguration UIAssetManagerConfiguration { get; set; }
        public IUIAssetManager UIAssetManager { get; set; }
        public IUIPackageHelper UIPackageHelper { get; set; }
        
        public List<FUI> UIBuffer = new();
    }
}