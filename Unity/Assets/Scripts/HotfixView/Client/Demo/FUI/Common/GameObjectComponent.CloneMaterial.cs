using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace ET.Client.Common
{
    public partial class GameObjectComponentSystem
    {
        /// <summary>
        /// 获取渲染器上的材质球
        /// </summary>
        public static void GetRendererMaterials(this GameObjectComponent self, ICollection<Material> buffer)
        {
            buffer.Clear();

            if (self.m_CachedRenderers == null)
                return;

            if (self.CloneMaterial)
            {
                // 从备份中获取
                if (self.m_MaterialsBackupDict == null || self.m_MaterialsBackupDict.Count == 0)
                    return;

                foreach (Material material in self.m_MaterialsBackupDict.Values)
                {
                    buffer.Add(material);
                }

                return;
            }
            
            // 从renderer上获取
            var materials = GameObjectComponent.SharedMaterialBuffer;
            materials.Clear();
            
            foreach (Renderer renderer in self.m_CachedRenderers)
            {
                if (renderer == null)
                    continue;
                
                materials.Clear();
                renderer.GetSharedMaterials(materials);

                if (materials.Count == 0)
                    continue;
                
                foreach (Material material in materials)
                {
                    if (material == null)
                        continue;
                    
                    buffer.Add(material);
                }
            }
        }

        /// <summary>
        /// 缓存渲染器
        /// </summary>
        public static void CacheRawRenderers(this GameObjectComponent self)
        {
            if (self.CurrentRendererType != GameObjectComponent.RendererType.GoWrapper)
                return;
            
            if (self.CurrentViewObject == null)
                return;
            
            if (self.CurrentViewObjectType == GameObjectComponent.ViewObjectType.UGUI)
                return;
            
            self.RecoverMaterials();

            if (self.m_CachedRenderers == null)
                self.m_CachedRenderers = new List<Renderer>();
            else
                self.m_CachedRenderers.Clear();
            self.CurrentViewObject.GetComponentsInChildren(true, self.m_CachedRenderers);
            
            if (self.CloneMaterial)
                self.CloneMaterials();
            
            if (self.GoWrapper != null && self.GoWrapper.wrapTarget == self.CurrentViewObject)
                self.GoWrapper.CacheRenderers();
        }
        
        /// <summary>
        /// 克隆材质球
        /// </summary>
        private static void CloneMaterials(this GameObjectComponent self)
        {
            if (self.m_CachedRenderers == null || self.m_CachedRenderers.Count == 0)
                return;
            
            var materials = GameObjectComponent.SharedMaterialBuffer;
            materials.Clear();
            
            foreach (Renderer renderer in self.m_CachedRenderers)
            {
                if (renderer == null)
                    continue;
                
                materials.Clear();
                renderer.GetSharedMaterials(materials);

                if (materials.Count == 0)
                    continue;
                
                bool shouldSetRenderQueue = renderer is SkinnedMeshRenderer || renderer is MeshRenderer;
                bool anyModify = false;

                for (int index = 0; index < materials.Count; index++)
                {
                    Material material = materials[index];
                    if (material == null)
                        continue;

                    anyModify = true;
                    self.m_MaterialsBackupDict ??= new Dictionary<Material, Material>();
                    self.m_MaterialsBackupRevertDict ??= new Dictionary<Material, Material>();

                    // 确保相同的材质不会复制两次
                    if (!self.m_MaterialsBackupDict.TryGetValue(material, out Material newMaterial))
                    {
                        newMaterial = new Material(material);
                        self.m_MaterialsBackupDict[material] = newMaterial;
                        self.m_MaterialsBackupRevertDict[newMaterial] = material;
                    }
                    
                    materials[index] = newMaterial;

                    if (shouldSetRenderQueue && material.renderQueue < 3000) //Set the object rendering in Transparent Queue as UI objects
                        newMaterial.renderQueue = 3000;
                }

                if (!anyModify)
                    continue;
                
                if (self.CustomCloneMaterials != null)
                    self.CustomCloneMaterials(renderer, materials, self.m_MaterialsBackupDict);
                else
                    renderer.sharedMaterials = materials.ToArray();
            }
        }

        /// <summary>
        /// 还原材质球
        /// </summary>
        private static void RecoverMaterials(this GameObjectComponent self)
        {
            if (self.m_MaterialsBackupDict == null || self.m_MaterialsBackupDict.Count == 0)
                return;
            
            var materials = GameObjectComponent.SharedMaterialBuffer;
            materials.Clear();
            
            foreach (Renderer renderer in self.m_CachedRenderers)
            {
                if (renderer == null)
                    continue;
                
                materials.Clear();
                renderer.GetSharedMaterials(materials);
                
                if (materials.Count == 0)
                    continue;
                
                for (int index = 0; index < materials.Count; index++)
                {
                    Material material = materials[index];
                    if (material == null)
                        continue;
                    
                    if (!self.m_MaterialsBackupRevertDict.TryGetValue(material, out Material backUpMaterial))
                        continue;

                    materials[index] = backUpMaterial;
                }
                
                if (self.CustomRecoverMaterials != null)
                    self.CustomRecoverMaterials(renderer, materials, self.m_MaterialsBackupDict);
                else
                    renderer.sharedMaterials = materials.ToArray();
            }

            foreach (Material material in self.m_MaterialsBackupDict.Values)
            {
                UnityEngine.Object.Destroy(material);
            }

            self.m_MaterialsBackupDict.Clear();
            self.m_MaterialsBackupRevertDict.Clear();
        }
    }
}