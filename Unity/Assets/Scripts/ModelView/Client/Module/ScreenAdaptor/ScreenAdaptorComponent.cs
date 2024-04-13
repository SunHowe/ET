using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 屏幕适配组件
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public sealed class ScreenAdaptorComponent : Entity, IAwake
    {
        /// <summary>
        /// 屏幕安全区域
        /// </summary>
        public Rect SafeArea { get; set; }
    }
    
    /// <summary>
    /// 屏幕安全区域更新事件
    /// </summary>
    public struct ScreenSafeAreaUpdatedEvent
    {
        public Rect SafeArea;
    }
}