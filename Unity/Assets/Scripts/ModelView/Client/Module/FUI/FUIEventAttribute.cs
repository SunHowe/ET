using System;

namespace ET.Client
{
    /// <summary>
    /// 用于标记界面事件实例
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class FUIEventAttribute : BaseAttribute
    {
        public FUIViewId ViewId { get; }

        public FUIEventAttribute(FUIViewId viewId)
        {
            this.ViewId = viewId;
        }
    }
}