using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// FUI 屏幕适配组件
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public sealed class FUIScreenAdaptorComponent : Entity, IAwake<UIContentScaler.ScreenMatchMode, int, int>
    {
        public UIContentScaler.ScreenMatchMode ScreenMatchMode { get; set; }
        public int DesignResolutionX { get; set; }
        public int DesignResolutionY { get; set; }
        
        /// <summary>
        /// UI 安全区域。(逻辑尺寸)
        /// </summary>
        public Rect UISafeArea { get; set; }
    }
    
    /// <summary>
    /// UI 安全区域更新事件
    /// </summary>
    public struct FUIScreenAdaptorChangedEvent
    {
        /// <summary>
        /// UI 安全区域。(逻辑尺寸)
        /// </summary>
        public Rect UISafeArea { get; set; }
    }
}