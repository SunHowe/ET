using FairyGUI;

namespace ET.Client.Login
{
    [FUICompLogic(typeof(SpecialButton))]
    public partial class SpecialButtonLogic : AFUICompLogic<SpecialButton>
    {
        protected override void OnInitialize() 
        {
            this.ContentPane.onAddedToStage.Add(OnAddedToStage);
        }

        protected override void OnDispose() 
        {
            this.ContentPane.onAddedToStage.Remove(OnAddedToStage);
        }

        protected override void OnSetupAfterAdd() 
        {
        }

        private void OnAddedToStage(EventContext context)
        {
            Log.Info("SpecialButtonLogic OnAddedToStage");
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