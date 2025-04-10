using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_controller : MonoBehaviour
{
    [SerializeField] private float EnemySpeed=6f;
    [SerializeField] private float DetectionRange =10f;
    [SerializeField] private float StopDistance = 2.9f;
    [SerializeField] private Animator animator;
    [SerializeField] private bool Attack;

    [Header("Enemy Health")]
    public float Enemy_current_Health=100f;

    public Transform Player;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = EnemySpeed;
        agent.stoppingDistance = StopDistance;
        Attack = false;
    }

    
    void Update()
    {
        if (Player != null)
        {
            float DistancetoPlayer = Vector3.Distance(transform.position, Player.position);

           if(DistancetoPlayer <= DetectionRange && DistancetoPlayer > StopDistance)
            {
                agent.SetDestination(Player.position);
                EnemySpeed = 6f;
                Attack=false;
            }
           else if(DistancetoPlayer <= StopDistance)
            {
                agent.ResetPath();
                EnemySpeed = 0f;
                Attack=true;
            }
           else
            {
                EnemySpeed = 0f;
                agent.ResetPath();
            }
            animator.SetBool("Attack", Attack);
            animator.SetFloat("Enemyspeed", EnemySpeed);
        }

    }
    public void death()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);
    }
}
