using System;
using FairyGUI;

namespace ET.Client
{
    /// <summary>
    /// FUI自定义组件逻辑接口
    /// </summary>
    public interface IFUICompLogic
    {
        /// <summary>
        /// 初始化回调
        /// </summary>
        void Initialize(GComponent contentPane);

        /// <summary>
        /// 销毁回调
        /// </summary>
        void Dispose();

        /// <summary>
        /// 编辑器设置后回调
        /// </summary>
        void SetupAfterAdd();
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class FUICompLogicAttribute : BaseAttribute
    {
        /// <summary>
        /// 绑定的 FUI组件类型
        /// </summary>
        public Type ComponentType { get; }

        public FUICompLogicAttribute(Type componentType)
        {
            this.ComponentType = componentType;
        }
    }

    [EnableClass]
    public abstract class AFUICompLogic<T> : IFUICompLogic where T : GComponent
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

        public void SetupAfterAdd()
        {
            OnSetupAfterAdd();
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnDispose() { }
        protected virtual void OnSetupAfterAdd() { }
    }
}