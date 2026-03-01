using UnityEngine;

public class AIDeathState : AiState
{
    public AIDeathState(StateAgent agent) : base(agent) { }
    public override void OnEnter()
    {
        agent.animator.SetTrigger("Death");
        agent.movement.Destination = agent.transform.position;

        //var navMeshAgent = agent.GetComponent<NavMeshAgent>();
        //if(navMeshAgent != null) navMeshAgent.enabled = false;

        GameObject.Destroy(agent.gameObject, 5.0f);
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        
    }
}
