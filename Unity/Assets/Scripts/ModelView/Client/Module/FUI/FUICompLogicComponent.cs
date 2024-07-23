using System;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 管理自定义组件的逻辑类型
    /// </summary>
    [Code]
    public class FUICompLogicComponent : Singleton<FUICompLogicComponent>, ISingletonAwake
    {
        public Dictionary<Type, Type> CompType2LogicTypeDict { get; } = new();
        
        public void Awake()
        {
            var compLogics = CodeTypes.Instance.GetTypes(typeof(FUICompLogicAttribute));
            
            foreach (Type compLogic in compLogics)
            {
                object[] attrs = compLogic.GetCustomAttributes(typeof (FUICompLogicAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                FUICompLogicAttribute uiEventAttribute = attrs[0] as FUICompLogicAttribute;
                CompType2LogicTypeDict.Add(uiEventAttribute.ComponentType, compLogic);
            }
        }

        /// <summary>
        /// 创建UI组件对应的组件逻辑实例
        /// </summary>
        public IFUICompLogic CreateCompLogic(Type componentType)
        {
            if (!CompType2LogicTypeDict.TryGetValue(componentType, out Type compLogicType))
                return null;
            
            return Activator.CreateInstance(compLogicType) as IFUICompLogic;
        }
    }
}