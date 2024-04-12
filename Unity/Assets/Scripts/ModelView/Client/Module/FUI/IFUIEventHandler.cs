namespace ET.Client
{
    /// <summary>
    /// FUI事件处理基类
    /// </summary>
    public interface IFUIEventHandler
    {
        /// <summary>
        /// 是否需要暂停覆盖的界面
        /// </summary>
        bool IsPauseCoveredUIForm(FUI ui);
        
        /// <summary>
        /// 是否需要显示遮罩层
        /// </summary>
        bool IsNeedDisplayMaskLayer(FUI ui);
        
        /// <summary>
        /// 界面创建时调用
        /// </summary>
        void OnCreate(FUI ui);
        
        /// <summary>
        /// 界面销毁时调用
        /// </summary>
        void OnDestroy(FUI ui);
        
        /// <summary>
        /// 界面暂停
        /// </summary>
        void OnPause(FUI ui);
        
        /// <summary>
        /// 界面恢复
        /// </summary>
        void OnResume(FUI ui);
        
        /// <summary>
        /// 界面遮挡
        /// </summary>
        void OnCover(FUI ui);
        
        /// <summary>
        /// 界面遮挡恢复
        /// </summary>
        void OnReveal(FUI ui);
        
        /// <summary>
        /// 界面激活
        /// </summary>
        void OnRefocus(FUI ui);
        
        /// <summary>
        /// 界面轮询
        /// </summary>
        void OnUpdate(FUI ui);
    }
    
    public abstract class AFUIEventHandler<T> : IFUIEventHandler where T : Entity
    {
        public bool IsPauseCoveredUIForm(FUI ui)
        {
            return IsPauseCoveredUIForm(ui, ui.GetComponent<T>());
        }

        public bool IsNeedDisplayMaskLayer(FUI ui)
        {
            return IsNeedDisplayMaskLayer(ui, ui.GetComponent<T>());
        }

        public void OnCreate(FUI ui)
        {
            OnCreate(ui, ui.GetComponent<T>());
        }

        public void OnDestroy(FUI ui)
        {
            OnDestroy(ui, ui.GetComponent<T>());
        }

        public void OnPause(FUI ui)
        {
            OnPause(ui, ui.GetComponent<T>());
        }

        public void OnResume(FUI ui)
        {
            OnResume(ui, ui.GetComponent<T>());
        }

        public void OnCover(FUI ui)
        {
            OnCover(ui, ui.GetComponent<T>());
        }

        public void OnReveal(FUI ui)
        {
            OnReveal(ui, ui.GetComponent<T>());
        }

        public void OnRefocus(FUI ui)
        {
            OnRefocus(ui, ui.GetComponent<T>());
        }
        
        // 这里因为有的界面可能不需要轮询 无用的轮询查询组件会带来额外开销
        public abstract void OnUpdate(FUI ui);
        
        protected abstract bool IsPauseCoveredUIForm(FUI ui, T component);
        protected abstract bool IsNeedDisplayMaskLayer(FUI ui, T component);
        
        protected abstract void OnCreate(FUI ui, T component);
        protected abstract void OnDestroy(FUI ui, T component);
        protected abstract void OnPause(FUI ui, T component);
        protected abstract void OnResume(FUI ui, T component);
        protected abstract void OnCover(FUI ui, T component);
        protected abstract void OnReveal(FUI ui, T component);
        protected abstract void OnRefocus(FUI ui, T component);
    }
}