namespace ET.Client.Login
{
    [FUICompLogic(typeof(DemoComponent))]
    public partial class DemoComponentLogic : AFUICompLogic<DemoComponent>
    {
        protected override void OnInitialize() 
        {
        }

        protected override void OnDispose() 
        {
        }

        protected override void OnSetupAfterAdd() 
        {
        }
    }

    public static partial class DemoComponentSystem
    {
        public static DemoComponentLogic GetCompLogic(this DemoComponent self)
        {
            return (DemoComponentLogic)self.CompLogic;
        }
    }
}