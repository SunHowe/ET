using System;

namespace ET.Client
{
    /// <summary>
    /// 用于标记组件事件实例
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class FUICompEventAttribute : BaseAttribute
    {
        /// <summary>
        /// 绑定的 FUI组件类型
        /// </summary>
        public Type ComponentType { get; }

        public FUICompEventAttribute(Type componentType)
        {
            this.ComponentType = componentType;
        }
    }
}