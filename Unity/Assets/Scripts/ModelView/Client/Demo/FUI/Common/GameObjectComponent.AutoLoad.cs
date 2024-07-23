namespace ET.Client.Common
{
    public partial class GameObjectComponent
    {
        public string m_AutoLoadAssetKey { get; set; }
        public RendererType m_AutoLoadRendererType { get; set; }

        public enum AutoLoadRendererSetting
        {
            GoWrapper,
            CloneMaterialGoWrapper,
            RenderTexture,
            SharedRenderTexture,
        }
    }
}