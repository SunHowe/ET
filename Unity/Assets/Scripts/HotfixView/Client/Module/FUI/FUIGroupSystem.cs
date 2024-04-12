using System.Linq;
using FairyGUI;

namespace ET.Client
{
    [EntitySystemOf(typeof(FUIGroup))]
    [FriendOf(typeof(FUIGroup))]
    public static partial class FUIGroupSystem
    {
        [EntitySystem]
        public static void Awake(this FUIGroup self, string name, int depth)
        {
            self.Name = name;
            self.Depth = depth;
            
            GRoot groot = GRoot.inst;
            
            self.ContentPane = new GComponent();
            self.ContentPane.size = groot.size;
            self.ContentPane.Center(true);
            self.ContentPane.AddRelation(groot, RelationType.Size);
            self.ContentPane.sortingOrder = depth + 1;
            
            groot.AddChild(self.ContentPane);
        }
        
        /// <summary>
        /// 获取分组中的当前UI(栈顶)
        /// </summary>
        public static FUI GetCurrentUI(this FUIGroup self)
        {
            return self.UIs.First?.Value?.UI;
        }
        
        /// <summary>
        /// 添加UI到分组
        /// </summary>
        public static void AddUI(this FUIGroup self, FUI ui)
        {
            self.UIs.AddFirst(self.CreateUIInfo(ui));
        }
        
        /// <summary>
        /// 移除分组中的UI
        /// </summary>
        public static void RemoveUI(this FUIGroup self, FUI ui)
        {
            FUIInfo info = self.GetUIInfo(ui);
            if (info == null)
            {
                return;
            }

            if (!info.Covered)
            {
                info.Covered = true;
                ui.EventHandler.OnCover(ui);
            }
            
            if (!info.Paused)
            {
                info.Paused = true;
                ui.EventHandler.OnPause(ui);
            }
            
            if (self.CacheNode != null && self.CacheNode.Value == info)
            {
                self.CacheNode = self.CacheNode.Next;
            }

            if (!self.UIs.Remove(info))
            {
                return;
            }
            
            ObjectPool.Instance.Recycle(info);
        }

        /// <summary>
        /// 激活UI
        /// </summary>
        public static void RefocusUI(this FUIGroup self, FUI ui)
        {
            FUIInfo info = self.GetUIInfo(ui);
            if (info == null)
            {
                return;
            }
            
            self.UIs.Remove(info);
            self.UIs.AddFirst(info);
        }

        /// <summary>
        /// 刷新分组
        /// </summary>
        public static void Refresh(this FUIGroup self)
        {
            
        }
        
        /// <summary>
        /// 获取UI信息
        /// </summary>
        private static FUIInfo GetUIInfo(this FUIGroup self, FUI ui)
        {
            return self.UIs.FirstOrDefault(info => info.UI == ui);
        }

        /// <summary>
        /// 创建UI信息
        /// </summary>
        private static FUIInfo CreateUIInfo(this FUIGroup self, FUI ui)
        {
            FUIInfo info = ObjectPool.Instance.Fetch<FUIInfo>();
            info.UI = ui;
            info.Paused = true;
            info.Covered = true;
            return info;
        }
    }
}