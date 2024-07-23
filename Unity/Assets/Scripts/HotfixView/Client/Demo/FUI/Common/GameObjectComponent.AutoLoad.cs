using System;
using System.Collections.Generic;
using System.Threading;

namespace ET.Client.Common
{
    public partial class GameObjectComponentSystem
    {
        private static void DeserializeAutoLoadConfig(this GameObjectComponent self, Dictionary<string, object> jsonObject)
        {
            if (jsonObject == null)
                return;
            
            const string JsonKey_AssetKey = "GO_AssetKey";
            const string JsonKey_RendererType = "GO_RendererType";
            
            self.m_AutoLoadAssetKey = jsonObject.TryGetValue(JsonKey_AssetKey, out object value) && value is string str ? str : string.Empty;
            if (string.IsNullOrEmpty(self.m_AutoLoadAssetKey))
                return;

            GameObjectComponent.AutoLoadRendererSetting setting;
            
            if (jsonObject.TryGetValue(JsonKey_RendererType, out value))
            {
                setting = value switch
                {
                    int intValue => (GameObjectComponent.AutoLoadRendererSetting)intValue,
                    double doubleValue => (GameObjectComponent.AutoLoadRendererSetting)(int)doubleValue,
                    _ => GameObjectComponent.AutoLoadRendererSetting.CloneMaterialGoWrapper,
                };
            }
            else
            {
                setting = GameObjectComponent.AutoLoadRendererSetting.CloneMaterialGoWrapper;
            }

            switch (setting)
            {
                case GameObjectComponent.AutoLoadRendererSetting.GoWrapper:
                    self.CloneMaterial = false;
                    self.m_AutoLoadRendererType = GameObjectComponent.RendererType.GoWrapper;
                    break;
                case GameObjectComponent.AutoLoadRendererSetting.CloneMaterialGoWrapper:
                    self.CloneMaterial = true;
                    self.m_AutoLoadRendererType = GameObjectComponent.RendererType.GoWrapper;
                    break;
                case GameObjectComponent.AutoLoadRendererSetting.RenderTexture:
                    // CloneMaterial = false;
                    self.m_AutoLoadRendererType = GameObjectComponent.RendererType.RenderTexture;
                    break;
                case GameObjectComponent.AutoLoadRendererSetting.SharedRenderTexture:
                    // CloneMaterial = false;
                    self.m_AutoLoadRendererType = GameObjectComponent.RendererType.RenderTexture;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void UpdateViewWithAutoLoadConfig(this GameObjectComponent self)
        {
            if (string.IsNullOrEmpty(self.m_AutoLoadAssetKey) || self.m_AutoLoadRendererType == GameObjectComponent.RendererType.None)
                return;

            self.UpdateView(self.m_AutoLoadRendererType, self.m_AutoLoadAssetKey, null).Coroutine();
        }
    }
}