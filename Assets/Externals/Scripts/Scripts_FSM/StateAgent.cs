using UnityEngine;
using CGL.Actor;

public class StateAgent : AIAgent
{
   


    public Movement movement;
    public Perception perception;
    public Animator animator;
    public Transform attackPoint;

    [Header("Parmeters")]
    public float timer;
    public float maxHealth = 1;
    public float health;
    public float distanceToDestination;
    public float distanceToEnemy;
    public Player enemy;


    public AiStateMachine stateMachine { get; private set; }
    //public AiPushdownStateMachine stateMachine { get; private set; } = new AiPushdownStateMachine();


    public Vector3 Destination
    {
        get { return movement.Destination; }
        set { movement.Destination = value; }
    }

    private void Start()
    {

        stateMachine = new AiStateMachine();
        //stateMachine = new AiPushdownStateMachine();

        health = maxHealth;
       
        stateMachine.AddState( new AIIdleState(this));
        stateMachine.AddState( new AIPatrolState(this));
        stateMachine.AddState( new AIAttackState(this));
        stateMachine.AddState( new AIDeathState(this));
        stateMachine.AddState( new AIDamageState(this));
        stateMachine.AddState( new AIChaseState(this));

        stateMachine.SetState<AIIdleState>();
        //stateMachine.PushState<AIIdleState>();
    }

    private void Update()
    {
        UpdateParameters();
        stateMachine.Update();
    }

    private void UpdateParameters()
    {
        //update parameters
        timer -= Time.deltaTime;
        distanceToDestination = Vector3.Distance(transform.position, Destination);
        var gameObjects = perception.GetGameObjects();
        enemy = null;
        if (gameObjects.Length > 0)
        {
            if (gameObjects[0].TryGetComponent<Player>(out var player))
            {
                enemy = player;
            }
        }
            distanceToEnemy = (enemy != null) ? Vector3.Distance(transform.position, enemy.transform.position) : float.MaxValue;
        
    }

    public void OnDamage(float damage)
    {
        Debug.Log(damage);
        health -= damage;

        if (health <= 0)
        {
            stateMachine.SetState<AIDeathState>();
        }
        else if (!(stateMachine.CurrentState is AIDeathState))
        {
            // only go to damage state if not already dying
            stateMachine.SetState<AIDamageState>();
        }
    }


    /*
     */
    private void OnGUI()
 {
  GUI.skin.label.alignment = TextAnchor.MiddleCenter;
  Rect rect = new Rect(0, 0, 100, 60);
  // transform world position of agent to screen position
  Vector3 point = Camera.main.WorldToScreenPoint(transform.position);
  rect.x = point.x;// - (rect.width / 2);
  rect.y = Screen.height - point.y - rect.height - 40;

  // get current state
  string str = stateMachine.GetString();

  // set box and label (text)
  GUI.backgroundColor = Color.black;
  GUI.Box(rect, GUIContent.none);
  GUI.Label(rect, str);
 }

   
}
