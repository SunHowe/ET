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
        /// 绑定的组件类型
        /// </summary>
        Type ComponentType { get; } 

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
    }

    public abstract class AFUIEventHandler<T> : HandlerObject, IFUIEventHandler where T : Entity
    {
        public abstract string UIAssetURL { get; }
        public Type ComponentType => typeof(T);

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

        protected abstract void OnOpen(FUI ui, T component, object userData);
        protected abstract void OnClose(FUI ui, T component);
        protected abstract void OnPause(FUI ui, T component);
        protected abstract void OnResume(FUI ui, T component);
        protected abstract void OnCover(FUI ui, T component);
        protected abstract void OnReveal(FUI ui, T component);
        protected abstract void OnRefocus(FUI ui, T component, object userData);
    }
}