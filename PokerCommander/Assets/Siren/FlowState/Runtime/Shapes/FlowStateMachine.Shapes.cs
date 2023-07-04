
namespace Siren
{
    public partial class FlowStateMachine
    {
        public override void Render()
        {
            if (m_stateStack.Count > 0 && m_stateStack.Peek().m_stage == FlowState.StateStage.ACTIVE)
            {
                m_stateStack.Peek().OnRender();
            }
        }
    }
}