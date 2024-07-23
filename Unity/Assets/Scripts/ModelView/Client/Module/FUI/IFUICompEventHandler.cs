using FairyGUI;

namespace ET.Client
{
    /// <summary>
    /// FUI自定义组件逻辑接口
    /// </summary>
    public interface IFUICompEventHandler
    {
        /// <summary>
        /// 初始化回调
        /// </summary>
        void Initialize(GComponent contentPane);

        /// <summary>
        /// 销毁回调
        /// </summary>
        void Dispose();
    }

    [EnableClass]
    public abstract class AFUICompEventHandler<T> : IFUICompEventHandler where T : GComponent
    {
        protected T ContentPane { get; private set; }
        
        public void Initialize(GComponent contentPane)
        {
            ContentPane = (T)contentPane;
            
            OnInitialize();
        }

        public void Dispose()
        {
            OnDispose();
            
            ContentPane = null;
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnDispose() { }
    }
}