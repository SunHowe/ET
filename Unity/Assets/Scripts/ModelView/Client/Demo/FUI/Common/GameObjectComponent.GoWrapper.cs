using FairyGUI;

namespace ET.Client.Common
{
    public partial class GameObjectComponent
    {
        /// <summary>
        /// GoWrapper实例
        /// </summary>
        public GoWrapper GoWrapper { get; set; }

        /// <summary>
        /// 是否在实例化时激活对应GameObject
        /// </summary>
        public bool IsActiveGameObjectOnInstantiate { get; set; } = true;
    }
}