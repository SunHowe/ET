using FairyGUI;
using UnityEngine;

namespace ET.Client.Common
{
    public partial class GameObjectComponent : IColorGear
    {
        /// <summary>
        /// 叠加颜色
        /// </summary>
        public Color color { get; set; }
    }
}