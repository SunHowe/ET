using System.Threading;

namespace ET.Client.Common
{
    public partial class GameObjectComponent
    {
        /// <summary>
        /// 是否在加载中
        /// </summary>
        public bool IsLoading { get; set; }

        public string m_LoadingAssetKey { get; set; }
        public RendererType m_LoadingRendererType { get; set; }

        public ETCancellationToken m_LoadingToken { get; set; }
    }
}