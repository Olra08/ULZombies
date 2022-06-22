using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private Transform followAt;
    public float distanceToFollow;
    
    public EnemySO data;
    private float health;
    public Slider healthbar;

    private Text puntaje;

    private PlayerController player;

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

        player = GameManager.GetInstance().player;

        mNavMeshAgent.speed = data.speed;
        health = data.health;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ExplosionM"))
        {
            health--;
            healthbar.value -= 0.1f;
            //Debug.Log(health);
            if (health <= 0)
            {
                Destroy(gameObject);
                player.setPuntaje(10);
                puntaje.text = player.getPuntaje().ToString();
            }
        }
    }
}
