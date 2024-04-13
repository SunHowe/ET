using System;

namespace ET.Client
{
    /// <summary>
    /// FUI事件处理基类
    /// </summary>
    public interface IFUIEventHandler
    {
        /// <summary>
        /// UI资源路径
        /// </summary>
        string UIAssetURL { get; }

        /// <summary>
        /// 所属的分组
        /// </summary>
        FUIGroupId FUIGroupId { get; }
        
        /// <summary>
        /// 绑定的组件类型
        /// </summary>
        Type ComponentType { get; } 

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
        /// 界面打开时调用
        /// </summary>
        void OnOpen(FUI ui, object userData);

        /// <summary>
        /// 界面关闭时调用
        /// </summary>
        void OnClose(FUI ui);

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
        void OnRefocus(FUI ui, object userData);

        /// <summary>
        /// 界面轮询
        /// </summary>
        void OnUpdate(FUI ui);
    }

    public abstract class AFUIEventHandler<T> : IFUIEventHandler where T : Entity
    {
        public abstract string UIAssetURL { get; }
        public abstract FUIGroupId FUIGroupId { get; }
        public Type ComponentType => typeof(T);

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

        public void OnOpen(FUI ui, object userData)
        {
            OnOpen(ui, ui.GetComponent<T>(), userData);
        }

        public void OnClose(FUI ui)
        {
            OnClose(ui, ui.GetComponent<T>());
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

        public void OnRefocus(FUI ui, object userData)
        {
            OnRefocus(ui, ui.GetComponent<T>(), userData);
        }

        // 这里因为有的界面可能不需要轮询 无用的轮询查询组件会带来额外开销
        public virtual void OnUpdate(FUI ui) { }

        protected virtual bool IsPauseCoveredUIForm(FUI ui, T component) => false;
        protected virtual bool IsNeedDisplayMaskLayer(FUI ui, T component) => false;

        protected virtual void OnCreate(FUI ui, T component) { }
        protected virtual void OnDestroy(FUI ui, T component) { }
        protected virtual void OnOpen(FUI ui, T component, object userData) { }
        protected virtual void OnClose(FUI ui, T component) { }
        protected virtual void OnPause(FUI ui, T component) { }
        protected virtual void OnResume(FUI ui, T component) { }
        protected virtual void OnCover(FUI ui, T component) { }
        protected virtual void OnReveal(FUI ui, T component) { }
        protected virtual void OnRefocus(FUI ui, T component, object userData) { }
    }
}