using CGL.Actor;
using UnityEngine;

public class AIAttackState : AiState
{
    public AIAttackState(StateAgent agent) : base(agent) { }
    public override void OnEnter()
    {
        agent.animator.SetTrigger("Attack");
        agent.movement.Destination = agent.transform.position;
        agent.timer = 1.0f;

        if (agent.enemy != null)
        {
            agent.transform.rotation = Quaternion.LookRotation(agent.enemy.transform.position - agent.transform.position);
        }

        Attack(); //could delay call
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        if(agent.timer <= 0.0f)
        {
           agent.stateMachine.SetState<AIChaseState>();
           //agent.stateMachine.PopState();
        }
        if (agent.enemy != null)
        {
            agent.transform.rotation = Quaternion.LookRotation(agent.enemy.transform.position - agent.transform.position);
        }
    }

    void Attack()
    {
        var colliders = Physics.OverlapSphere(agent.transform.position, 2.0f);

        foreach (var collider in colliders)
        {
            if (collider.gameObject == agent.gameObject)
                continue; // don't hit self

            if (collider.TryGetComponent<Health>(out var health))
            {
                float damage = Random.Range(10f, 30f);
                health.TakeDamage(damage);
                Debug.Log("Player took damage: " + damage);
            }
        }
    }
}
