using System.Collections.Generic;
using FairyGUI;

namespace ET.Client
{
    /// <summary>
    /// FUI组 用于管理同组的FUI
    /// </summary>
    public sealed class FUIGroup : Entity, IAwake<string, int>
    {
        public GComponent ContentPane { get; set; }
        public string Name { get; set; }
        public int Depth { get; set; }
        public LinkedList<FUIInfo> UIs = new();
        public LinkedListNode<FUIInfo> CacheNode;
    }

    public sealed class FUIInfo : IPool
    {
        public bool IsFromPool { get; set; }
        
        public FUI UI { get; set; }
        public bool Paused { get; set; }
        public bool Covered { get; set; }
    }
}