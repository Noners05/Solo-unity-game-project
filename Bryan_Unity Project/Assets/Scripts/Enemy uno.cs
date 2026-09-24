using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health = 3;
    public int maxHealth = 3;
    public float speed = 6.5f;

    public float detectionRange = 5;

    public PlayerControler player;
    public NavMeshAgent agent;

    bool isFollowing = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
        agent = GetComponent<NavMeshAgent>();

    }

    
    void Update()
    {
        float targetDistance = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));

        isFollowing = targetDistance <= detectionRange;

        if (isFollowing)
        {
            agent.destination = player.transform.position;
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("Player"))
            speed = 0;
    }
}
