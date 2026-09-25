using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health = 3;
    public int maxHealth = 3;
    public float speed = 5f;
    public bool run;

    public float detectionRange = 5;

    public PlayerControler player;
    public NavMeshAgent agent;

    bool isFollowing = false;
   

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.isStopped = false;
        run = false;
    }

    
    void Update()
    {
        float targetDistance = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));

        isFollowing = targetDistance <= detectionRange;

        if (isFollowing)
        {
            agent.destination = player.transform.position;
            Sprint(Run);
            
            if (isFollowing != true)
            {
                agent.speed != ;
                agent.speed = speed;
            }
        }

    }
    
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("Player"))
            speed = 0;
    }

    void Sprint(float speed)
    {
        agent.speed = speed;
    }

    void Run()
    {
        speed = 12;
    }
}
