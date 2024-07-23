using System;

namespace ET.Client.Common
{
    public partial class GameObjectComponent
    {
        /// <summary>
        /// 渲染器类型定义
        /// </summary>
        public enum RendererType
        {
            None,

            /// <summary>
            /// GoWrapper装载器模式
            /// </summary>
            GoWrapper,

            /// <summary>
            /// 贴图模式
            /// </summary>
            RenderTexture,
        }

        /// <summary>
        /// 当前渲染器对应的资源
        /// </summary>
        public string CurrentAssetKey { get; set; }

        /// <summary>
        /// 当前渲染器类型
        /// </summary>
        public RendererType CurrentRendererType { get; set; }

        /// <summary>
        /// 是否在移除舞台时销毁渲染器
        /// </summary>
        public bool IsDisposeRendererOnRemovedStage { get; set; }
    }
}