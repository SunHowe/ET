namespace ET.Client.Login
{
    [FUICompLogic(typeof(SpecialButton))]
    public partial class SpecialButtonLogic : AFUICompLogic<SpecialButton>
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

    public static partial class SpecialButtonSystem
    {
        public static SpecialButtonLogic GetCompLogic(this SpecialButton self)
        {
            return (SpecialButtonLogic)self.CompLogic;
        }
    }
}