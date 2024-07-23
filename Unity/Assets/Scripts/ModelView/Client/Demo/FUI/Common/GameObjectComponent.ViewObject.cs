using System;
using UnityEngine;

namespace ET.Client.Common
{
    public partial class GameObjectComponent
    {
        /// <summary>
        /// 视图对象类型定义
        /// </summary>
        public enum ViewObjectType
        {
            None,

            /// <summary>
            /// UGUI对象
            /// </summary>
            UGUI,

            /// <summary>
            /// 特效对象
            /// </summary>
            VFX,

            /// <summary>
            /// 正常的3d模型对象
            /// </summary>
            Normal,
        }

        /// <summary>
        /// 当前视图对象类型
        /// </summary>
        public ViewObjectType CurrentViewObjectType { get; set; }

        /// <summary>
        /// 当前视图对象实例
        /// </summary>
        public GameObject CurrentViewObject { get; set; }
        
        /// <summary>
        /// 当前视图对象销毁执行的动作
        /// </summary>
        public Action CurrentViewObjectDisposeAction { get; set; }
    }
}