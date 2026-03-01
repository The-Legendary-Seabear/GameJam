using UnityEngine;

public class AIIdleState : AiState
{
    public AIIdleState(StateAgent agent) : base(agent) { }
    public override void OnEnter()
    {
        agent.animator.SetFloat("Speed", 0);
        agent.timer = Random.Range(1.0f, 3.0f);
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        if(agent.timer <= 0)
        {
            //set state to patrol
            agent.stateMachine.SetState<AIPatrolState>();
            //agent.stateMachine.PushState<AIPatrolState>();
        }

        if (agent.enemy != null)
        {
            agent.stateMachine.SetState<AIChaseState>();
            //agent.stateMachine.PushState<AIChaseState>();
        }
    }

    
}
