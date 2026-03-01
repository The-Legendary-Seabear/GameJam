using UnityEngine;

public class AIPatrolState : AiState
{
    public AIPatrolState(StateAgent agent) : base(agent) { }
    public override void OnEnter()
    {
        agent.animator.SetFloat("Speed", 3);
        agent.Destination = NavNode.GetRandomNavNode().transform.position;

    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        if (agent.distanceToDestination <= 0.5f)
        {
            //set state to idle
            agent.stateMachine.SetState<AIIdleState>();
            //agent.stateMachine.PopState();
        }

        //if enemy seen, chase
        if(agent.enemy != null)
        {
            agent.stateMachine.SetState<AIChaseState>();
            //agent.stateMachine.PushState<AIChaseState>();
        }

        }
}
