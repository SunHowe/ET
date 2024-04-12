using System;

namespace ET.Client
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FUIEventAttribute : BaseAttribute
    {
        public FUIViewId ViewId { get; }

        public FUIEventAttribute(FUIViewId viewId)
        {
            this.ViewId = viewId;
        }
    }
}