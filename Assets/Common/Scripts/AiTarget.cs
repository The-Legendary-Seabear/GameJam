using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AiTarget : MonoBehaviour
{
    public Transform target;
    public float AttackDistance;
    private NavMeshAgent m_Agent;
    private Animator m_Animator;
    private float m_Distance;
    private Vector3 m_StartingPoint;
    private bool m_PathCalculate = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        m_StartingPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        m_Distance = Vector3.Distance(m_Agent.transform.position, target.position);
        if (m_Distance < AttackDistance)
        {
            m_Agent.isStopped = true;
            m_Animator.SetBool("Attack", true);
        } else
        {
            m_Agent.isStopped = false;
            if(!m_Agent.hasPath && m_PathCalculate)
            {
                m_Agent.destination = m_StartingPoint;
                m_PathCalculate = false;
            } else
            {
                m_Animator.SetBool("Attack", false);
                m_Agent.destination = target.position;
                m_PathCalculate = true;
            }

        }
        
    }

    private void OnAnimatorMove()
    {
        m_Agent.speed = (m_Animator.deltaPosition / Time.deltaTime).magnitude;
    }
}
