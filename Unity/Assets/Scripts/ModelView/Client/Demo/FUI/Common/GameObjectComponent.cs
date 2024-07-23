using System;

namespace ET.Client.Common
{
    [EnableClass]
    public partial class GameObjectComponent
    {
        /// <summary>
        /// 视图更新事件
        /// </summary>
        public Action OnViewUpdated;

        /// <summary>
        /// 是否已经初始化过编辑器数据
        /// </summary>
        public bool IsInitEditorSettings;
    }
}