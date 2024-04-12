using System;
using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(FUIGlobalComponent))]
    [FriendOf(typeof(FUIGlobalComponent))]
    public static partial class FUIGlobalComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FUIGlobalComponent self)
        {
            var uiEvents = CodeTypes.Instance.GetTypes(typeof(FUIEventAttribute));
            foreach (Type type in uiEvents)
            {
                object[] attrs = type.GetCustomAttributes(typeof(FUIEventAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                FUIEventAttribute uiEventAttribute = attrs[0] as FUIEventAttribute;
                IFUIEventHandler aUIEvent = Activator.CreateInstance(type) as IFUIEventHandler;
                self.EventHandlers.Add(uiEventAttribute.ViewId, aUIEvent);
            }
        }

        [EntitySystem]
        private static void Destroy(this FUIGlobalComponent self)
        {
            foreach (FUIGroup uiGroup in self.UIGroups.Values)
            {
                uiGroup.Dispose();
            }

            self.UIGroups.Clear();
        }

        /// <summary>
        /// 创建UI组
        /// </summary>
        public static FUIGroup CreateGroup(this FUIGlobalComponent self, string name)
        {
            if (self.UIGroups.TryGetValue(name, out FUIGroup group))
            {
                return group;
            }

            group = self.AddChild<FUIGroup, string, int>(name, self.UIGroups.Count);
            self.UIGroups.Add(name, group);

            return group;
        }

        /// <summary>
        /// 获取UI组
        /// </summary>
        public static FUIGroup GetGroup(this FUIGlobalComponent self, string name)
        {
            return self.UIGroups.GetValueOrDefault(name);
        }
        
        /// <summary>
        /// 获取界面事件处理器
        /// </summary>
        public static IFUIEventHandler GetEventHandler(this FUIGlobalComponent self, FUIViewId viewId)
        {
            return self.EventHandlers.GetValueOrDefault(viewId);
        }
    }
}