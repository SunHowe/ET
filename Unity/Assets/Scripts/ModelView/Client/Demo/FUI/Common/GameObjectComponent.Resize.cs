using UnityEngine;

namespace ET.Client.Common
{
    public partial class GameObjectComponent
    {
        /// <summary>
        /// 缩放模式定义
        /// </summary>
        public enum ResizeMode
        {
            /// <summary>
            /// 不做缩放 原始尺寸
            /// </summary> 
            None,

            /// <summary>
            /// 自由缩放 根据组件的宽高缩放 100为基准大小
            /// </summary>
            Free,

            /// <summary>
            /// 和UI组件的宽度等比放大 100为基准大小
            /// </summary>
            MatchWidth,

            /// <summary>
            /// 和UI组件的高度等比放大 100为基准大小
            /// </summary>
            MatchHeight,

            /// <summary>
            /// 和UI组件的宽高中的最小值等比放大 100为基准大小
            /// </summary>
            MinOfWidthAndHeight,

            /// <summary>
            /// 和UI组件的宽高中的最大值等比放大 100为基准大小
            /// </summary>
            MaxOfWidthAndHeight,
        }
        
        /// <summary>
        /// 缩放模式设置
        /// </summary>
        public ResizeMode CurrentResizeMode { get; set; }

        /// <summary>
        /// 是否根据初始化的尺寸进行缩放 即scale=当前的尺寸/初始化的尺寸 仅在resizeMode不为None时有效
        /// </summary>
        public bool IsResizeWithInitSize { get; set; }

        /// <summary>
        /// 缩放比例
        /// </summary>
        public Vector2 CurrentResizeScale { get; set; }
    }
}