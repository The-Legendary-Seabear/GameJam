using UnityEngine;

public class AIDamageState : AiState
{
    public AIDamageState(StateAgent agent) : base(agent) { }
    public override void OnEnter()
    {
        agent.timer = 1.0f;
        agent.animator.SetTrigger("Damage");
        agent.movement.Destination = agent.transform.position;
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {

        // if health is zero or below, immediately die
        if (agent.health <= 0)
        {
            agent.stateMachine.SetState<AIDeathState>();
            return;
        }

        if (agent.timer <= 0)
        {
            agent.stateMachine.SetState<AIPatrolState>();
            //agent.stateMachine.PopState();
        }
    }
}
