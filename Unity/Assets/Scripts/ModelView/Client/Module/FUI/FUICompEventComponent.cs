using System;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 管理自定义组件的逻辑类型
    /// </summary>
    [Code]
    public class FUICompEventComponent : Singleton<FUICompEventComponent>, ISingletonAwake
    {
        public Dictionary<Type, Type> CompType2EventHandlerTypeDict { get; } = new();
        
        public void Awake()
        {
            var compLogics = CodeTypes.Instance.GetTypes(typeof(FUICompEventAttribute));
            
            foreach (Type compLogic in compLogics)
            {
                object[] attrs = compLogic.GetCustomAttributes(typeof (FUICompEventAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                FUICompEventAttribute uiEventAttribute = attrs[0] as FUICompEventAttribute;
                this.CompType2EventHandlerTypeDict.Add(uiEventAttribute.ComponentType, compLogic);
            }
        }

        /// <summary>
        /// 创建UI组件对应的组件逻辑实例
        /// </summary>
        public IFUICompEventHandler CreateCompEventHandler(Type componentType)
        {
            if (!this.CompType2EventHandlerTypeDict.TryGetValue(componentType, out Type compLogicType))
                return null;
            
            return Activator.CreateInstance(compLogicType) as IFUICompEventHandler;
        }
    }
}