using System;
using UnityEngine;

namespace ET.Client.Common
{
    public partial class GameObjectComponentSystem
    {
        /// <summary>
        /// 判断指定视图对象的类型
        /// </summary>
        private static GameObjectComponent.ViewObjectType GetViewObjectType(GameObject go)
        {
            if (go == null)
                return GameObjectComponent.ViewObjectType.None;

            // 判断是否是UGUI
            if (go.GetComponent<RectTransform>() != null)
                return GameObjectComponent.ViewObjectType.UGUI;
            
            // 判断是否是特效对象
            if (go.GetComponent<VfxObject>() != null)
                return GameObjectComponent.ViewObjectType.VFX;

            // 普通对象
            return GameObjectComponent.ViewObjectType.Normal;
        }

        private static void DisposeViewObject(this GameObjectComponent self)
        {
            if (self.CurrentViewObjectDisposeAction != null)
            {
                Action disposeAction = self.CurrentViewObjectDisposeAction;
                self.CurrentViewObjectDisposeAction = null;
                disposeAction();
            }
            
            self.CurrentViewObjectType = GameObjectComponent.ViewObjectType.None;
            self.CurrentViewObject = null;
        }

        /// <summary>
        /// 设置视图对象
        /// </summary>
        private static void SetupViewObject(this GameObjectComponent self)
        {
            self.CurrentViewObjectType = GetViewObjectType(self.CurrentViewObject);
            self.SetupBasicViewObject();
            self.UpdateViewObjectScale();
        }

        private static void SetupBasicViewObject(this GameObjectComponent self)
        {
            Transform transform = self.CurrentViewObject.transform;
            
            transform.localPosition = new Vector3(0, 0, 1000);
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        /// <summary>
        /// 更新视图对象缩放尺寸
        /// </summary>
        private static void UpdateViewObjectScale(this GameObjectComponent self)
        {
            if (self.CurrentViewObject == null)
                return;

            self.CurrentViewObject.transform.localScale = new Vector3(self.CurrentResizeScale.x, self.CurrentResizeScale.y, self.CurrentResizeScale.x);
        }
    }
}