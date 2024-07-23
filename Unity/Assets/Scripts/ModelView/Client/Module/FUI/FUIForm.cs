using FairyGUI;

namespace ET.Client
{
    /// <summary>
    /// 框架内所有界面的FUI实例都将实例化成该类型 用于存放一些特殊的属性字段
    /// </summary>
    [EnableClass]
    public class FUIForm : GComponent
    {
        /// <summary>
        /// 关联的FUI实体引用
        /// </summary>
        public EntityRef<FUI> EntityRef { get; set; }

        /// <summary>
        /// 关联的FUI实体
        /// </summary>
        public FUI Entity => EntityRef;
    }
}