namespace ET.Client
{
    [ChildOf]
    public sealed class FUI : Entity, IAwake<FUIViewId, FUIForm, IFUIEventHandler>, IDestroy
    {
        #region [框架处理的属性]
        public FUIViewId ViewId { get; set; }
        public FUIForm ContentPane { get; set; }
        public IFUIEventHandler EventHandler { get; set; }
        #endregion

        /// <summary>
        /// 所属分组
        /// </summary>
        public FUIGroupId FUIGroupId { get; set; }
        
        /// <summary>
        /// 是否需要暂停覆盖的界面
        /// </summary>
        public bool PauseCoveredUIForm { get; set; }

        /// <summary>
        /// 是否需要显示遮罩层
        /// </summary>
        public bool NeedDisplayMaskLayer { get; set; }
    }
}