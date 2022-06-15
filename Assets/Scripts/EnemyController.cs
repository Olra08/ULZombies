using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform followAt;
    public float distanceToFollow;
    public EnemySO data;

    private NavMeshAgent mNavMeshAgent;
    private Animator mAnimator;

    private void Awake()
    {
        mNavMeshAgent = GetComponent<NavMeshAgent>();
        mAnimator = transform.Find("Mutant").GetComponent<Animator>();
    }

    private void Start()
    {
        mNavMeshAgent.speed = data.speed;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, followAt.position);
        if (distance<= distanceToFollow)
        {
            mNavMeshAgent.destination = followAt.position;
            mNavMeshAgent.isStopped = false;
            mAnimator.SetTrigger("Walk");

        }
        else
        {
            mNavMeshAgent.isStopped = true;
            mAnimator.SetTrigger("Stop");
        }
    }
}
