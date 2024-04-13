using System.Collections.Generic;
using System.Linq;
using FairyGUI;

namespace ET.Client
{
    [EntitySystemOf(typeof(FUIGroup))]
    [FriendOf(typeof(FUIGroup))]
    public static partial class FUIGroupSystem
    {
        [EntitySystem]
        public static void Awake(this FUIGroup self, FUIGroupId groupId)
        {
            self.GroupId = groupId;

            GRoot groot = GRoot.inst;

            self.ContentPane = new GComponent();
            self.ContentPane.gameObjectName = self.ContentPane.name = groupId.ToString();
            self.ContentPane.size = groot.size;
            self.ContentPane.Center(true);
            self.ContentPane.AddRelation(groot, RelationType.Size);
            self.ContentPane.sortingOrder = (int)(groupId + 1);

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
        /// 是否包含UI
        /// </summary>
        public static bool HasUI(this FUIGroup self, FUI ui)
        {
            return self.UIs.Any(info => (FUI)info.UI == ui);
        }
        
        /// <summary>
        /// 是否包含UI
        /// </summary>
        public static bool HasUI(this FUIGroup self, FUIViewId viewId)
        {
            return self.UIs.Any(info => ((FUI)info.UI).ViewId == viewId);
        }

        /// <summary>
        /// 添加UI到分组
        /// </summary>
        public static void AddUI(this FUIGroup self, FUI ui)
        {
            self.UIs.AddFirst(CreateUIInfo(ui));
            self.ContentPane.AddChild(ui.ContentPane);
        }

        /// <summary>
        /// 获取分组中的UI
        /// </summary>
        public static FUI GetUI(this FUIGroup self, FUIViewId viewId)
        {
            return self.UIs.FirstOrDefault(info => ((FUI)info.UI).ViewId == viewId)?.UI;
        }

        /// <summary>
        /// 获取分组中的所有UI
        /// </summary>
        public static int GetUIList(this FUIGroup self, List<EntityRef<FUI>> buffer)
        {
            buffer.Clear();
            buffer.AddRange(self.UIs.Select(info => info.UI));
            return buffer.Count;
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
                ui.OnCover();
            }

            if (!info.Paused)
            {
                info.Paused = true;
                ui.OnPause();
            }

            if (self.CacheNode != null && self.CacheNode.Value == info)
            {
                self.CacheNode = self.CacheNode.Next;
            }

            if (!self.UIs.Remove(info))
            {
                return;
            }

            self.ContentPane.RemoveChild(ui.ContentPane);
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
            LinkedListNode<FUIInfo> current = self.UIs.First;
            bool pause = false;
            bool cover = false;
            int depth = self.UIs.Count;
            FUIInfo maskLayerFUIInfo = null;

            while (current != null && current.Value != null)
            {
                LinkedListNode<FUIInfo> next = current.Next;
                FUIInfo fuiInfo = current.Value;
                if (fuiInfo == null)
                {
                    return;
                }
                
                FUI fui = fuiInfo.UI;
                if (fui == null)
                {
                    continue;
                }
                
                fui.SetDepth(depth);

                if (pause)
                {
                    if (!fuiInfo.Covered)
                    {
                        fuiInfo.Covered = true;
                        fui.OnCover();
                        if (current.Value == null)
                        {
                            return;
                        }
                    }

                    if (!fuiInfo.Paused)
                    {
                        fuiInfo.Paused = true;
                        fui.OnPause();
                        if (current.Value == null)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    if (fuiInfo.Paused)
                    {
                        fuiInfo.Paused = false;
                        fui.OnResume();
                        if (current.Value == null)
                        {
                            return;
                        }
                    }

                    if (fui.PauseCoveredUIForm)
                    {
                        pause = true;
                    }

                    if (cover)
                    {
                        if (!fuiInfo.Covered)
                        {
                            fuiInfo.Covered = true;
                            fui.OnCover();
                            if (current.Value == null)
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (fuiInfo.Covered)
                        {
                            fuiInfo.Covered = false;
                            fui.OnReveal();
                            if (current.Value == null)
                            {
                                return;
                            }
                        }

                        cover = true;
                    }

                    if (maskLayerFUIInfo == null && fui.NeedDisplayMaskLayer)
                    {
                        maskLayerFUIInfo = fuiInfo;
                    }
                }

                current = next;
            }

            self.SetMaskLayerBelongTo(maskLayerFUIInfo);
        }
        
        /// <summary>
        /// 设置遮罩层归属
        /// </summary>
        private static void SetMaskLayerBelongTo(this FUIGroup self, FUIInfo ui)
        {
            // TODO
        }

        /// <summary>
        /// 获取UI信息
        /// </summary>
        private static FUIInfo GetUIInfo(this FUIGroup self, FUI ui)
        {
            return self.UIs.FirstOrDefault(info => (FUI)info.UI == ui);
        }

        /// <summary>
        /// 创建UI信息
        /// </summary>
        private static FUIInfo CreateUIInfo(FUI ui)
        {
            FUIInfo info = ObjectPool.Instance.Fetch<FUIInfo>();
            info.UI = ui;
            info.Paused = true;
            info.Covered = true;
            return info;
        }
    }
}