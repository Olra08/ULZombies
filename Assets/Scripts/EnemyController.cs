using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private Transform followAt;
    public float distanceToFollow;
    public EnemySO data;
    private Text puntaje;
    private int valorPuntaje = 0;

    private NavMeshAgent mNavMeshAgent;
    private Animator mAnimator;

    private void Awake()
    {
        mNavMeshAgent = GetComponent<NavMeshAgent>();
        mAnimator = transform.Find("Mutant").GetComponent<Animator>();
    }

    private void Start()
    {
        followAt = GameObject.FindGameObjectWithTag("Jugador").transform;
        puntaje = GameObject.FindGameObjectWithTag("Canvas").transform.Find("Puntaje").GetComponent<Text>();

        mNavMeshAgent.speed = data.speed;
        if (data.name == "EnemySmall")
        {
            data.health = 5;
            //transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (data.name == "EnemyBig")
        {
            data.health = 15;
        }
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
        if (data.health <= 0)
        {
            Destroy(gameObject);
            valorPuntaje += 10;
            puntaje.text = valorPuntaje.ToString();
        }
    }
}
