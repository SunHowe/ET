namespace ET.Client
{
    [ChildOf]
    public sealed class FUI : Entity, IAwake<FUIViewId, FUIForm, IFUIEventHandler>, IDestroy
    {
        #region [框架处理的属性]
        public FUIViewId ViewId { get; set; }
        public FUIForm ContentPane { get; set; }
        public IFUIEventHandler EventHandler { get; set; }
        public int Depth { get; set; }
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

        /// <summary>
        /// 界面持有的CancellationToken 它会在界面被关闭时设置为取消
        /// </summary>
        public ETCancellationToken CancellationToken
        {
            get
            {
                this.CancellationTokenSource ??= new ETCancellationToken();
                return this.CancellationTokenSource;
            }
        }

        /// <summary>
        /// 不允许业务对该属性进行访问 应该由框架进行赋值
        /// </summary>
        public ETCancellationToken CancellationTokenSource { get; set; }
    }
}