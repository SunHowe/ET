using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client.Common
{
    public partial class GameObjectComponent
    {
        public Action<Renderer, List<Material>, Dictionary<Material, Material>> CustomCloneMaterials;
        public Action<Renderer, List<Material>, Dictionary<Material, Material>> CustomRecoverMaterials;

        /// <summary>
        /// 是否复制材质球
        /// </summary>
        public bool CloneMaterial { get; set; } = true;

        public List<Renderer> m_CachedRenderers { get; set; }
        public Dictionary<Material, Material> m_MaterialsBackupDict { get; set; }
        public Dictionary<Material, Material> m_MaterialsBackupRevertDict { get; set; }

        [StaticField]
        public static readonly List<Material> SharedMaterialBuffer = new List<Material>();
    }
}