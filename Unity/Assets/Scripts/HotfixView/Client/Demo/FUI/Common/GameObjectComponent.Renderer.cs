using System;

namespace ET.Client.Common
{
    public partial class GameObjectComponentSystem
    {
        /// <summary>
        /// 销毁当前的渲染器
        /// </summary>
        private static void DisposeRenderer(this GameObjectComponent self)
        {
            switch (self.CurrentRendererType)
            {
                case GameObjectComponent.RendererType.GoWrapper:
                    self.DisposeGoWrapperRenderer();
                    break;
                case GameObjectComponent.RendererType.RenderTexture:
                    break;
            }
            
            self.CurrentRendererType = GameObjectComponent.RendererType.None;
        }

        /// <summary>
        /// 设置渲染器
        /// </summary>
        private static void SetupRenderer(this GameObjectComponent self)
        {
            // 设置渲染器
            switch (self.CurrentRendererType)
            {
                case GameObjectComponent.RendererType.GoWrapper:
                    self.SetupGoWrapperRenderer();
                    break;
                case GameObjectComponent.RendererType.RenderTexture:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self.CurrentRendererType), self.CurrentRendererType, null);
            }
        }
    }
}