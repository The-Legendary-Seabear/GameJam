using UnityEngine;

public class AIChaseState : AiState
{
    public AIChaseState(StateAgent agent) : base(agent) { }
    public override void OnEnter()
    {
        agent.animator.SetFloat("Speed", 5);
        agent.movement.maxSpeed *= 2.0f;
    }

    public override void OnExit()
    {
        agent.movement.maxSpeed /= 2.0f;
    }

    public override void OnUpdate()
    {
        if(agent.enemy != null)
        {
        agent.movement.Destination = agent.enemy.transform.position;
            //if close to enemy, attack
            if(agent.distanceToEnemy <= 1.5f)
            {
                agent.stateMachine.SetState<AIAttackState>();
                //agent.stateMachine.PushState<AIAttackState>();
            }

        } else
        {
            //enemy is no longer seen, go to idle
            agent.stateMachine.SetState<AIIdleState>();
           // agent.stateMachine.PopState();
        }
    }
}
