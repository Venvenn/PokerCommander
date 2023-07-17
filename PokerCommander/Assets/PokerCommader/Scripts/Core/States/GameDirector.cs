using Siren;

/// <summary>
/// Game director for kicking off the state machine, should do very little else
/// </summary>
public class GameDirector : Director
{
    public override void OnStart()
    {
        m_flowStateMachine = new FlowStateMachine();
        m_flowStateMachine.Push(new FSSystem());
        
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
    }
}