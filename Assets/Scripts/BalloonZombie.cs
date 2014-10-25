using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class BalloonZombie : MonoBehaviour
{

    enum BalloonZombieStateMachine { Idle, Chase, Grab, Carrying, Dying };

    bool isActive = true;

    Animator anim;
    public float lagTimeBeforeGrabbingEnemy = 5.0F;
    public float lagTimeAfterGettingHit = 2.0F;
    public float lagTimeAfterDying = 3.0F;
    private float timer = 0F;
    public GameObject villager;
    CharacterController controller;

    private float startTime;
    public float speed = 10F;
    private float journeyLength;
    public float smooth = 5.0F;

    BalloonZombieStateMachine currState;

    public Transform exitPosition;

    public float minGrabbingRange = 0.5F;
    public float grabAnimationLength = 2.0F;

    bool startedTime = false;

    public Transform villagerLookAt;

    bool playedDeadAnimation = false;
    bool playedGotHitAnimation = false;

    // Use this for initialization
    void Start()
    {

        villagerLookAt = (Transform)Instantiate(exitPosition, new Vector3(0, -100, 50), Quaternion.identity);

        // villagerLookAt = exitPosition;
        // villagerLookAt.position = new Vector3(0, -100, 50);

        currState = BalloonZombieStateMachine.Idle;

        startTime = Time.time;

        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        journeyLength = Vector3.Distance(transform.position, villager.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        switch (currState)
        {
            case BalloonZombieStateMachine.Idle:

                timer += Time.deltaTime;

                if (timer > lagTimeBeforeGrabbingEnemy)
                {
                    timer = 0;
                    currState = BalloonZombieStateMachine.Chase;
                }

                break;

            case BalloonZombieStateMachine.Chase:

                float distCovered = (Time.time - startTime) * speed;
                float fracJourney = distCovered / journeyLength;

                transform.position = Vector3.Lerp(transform.position, villager.transform.position, fracJourney);

                if (InGrabbingRange())
                    currState = BalloonZombieStateMachine.Grab;

                break;

            case BalloonZombieStateMachine.Dying:
                timer += Time.deltaTime;

                if (startedTime == false)
                {
                    startTime = Time.time;
                    startedTime = true;
                }

                anim.SetBool("IsDead", true);
                playedDeadAnimation = true;

                villager.GetComponent<NavMeshAgent>().enabled = true;
                villager.transform.parent = null;

                journeyLength = Vector3.Distance(transform.position, exitPosition.position);

                distCovered = (Time.time - startTime) * speed;
                fracJourney = distCovered / journeyLength;

                transform.position = Vector3.Lerp(transform.position, exitPosition.position, fracJourney);

                if (playedDeadAnimation)
                {
                    if (timer > lagTimeAfterDying)
                        anim.SetBool("PlayDeadAnimation", true);
                }

                break;

            case BalloonZombieStateMachine.Grab:

                anim.SetBool("GrabVillager", true);

                if (startedTime == false)
                {
                    startTime = Time.time;
                    startedTime = true;
                }

                journeyLength = Vector3.Distance(villager.transform.position, transform.position);

                distCovered = (Time.time - startTime) * 1;
                fracJourney = distCovered / journeyLength;

                villager.transform.position = Vector3.Lerp(villager.transform.position, transform.position, fracJourney);

                villager.GetComponent<NavMeshAgent>().enabled = false;
                villager.transform.parent = transform;

                villager.transform.LookAt(villagerLookAt);

                if (GrabAnimationIsOver())
                {
                    currState = BalloonZombieStateMachine.Carrying;
                    startedTime = false;
                }

                break;

            case BalloonZombieStateMachine.Carrying:

                anim.SetBool("Carrying", true);

                if (startedTime == false)
                {
                    startTime = Time.time;
                    startedTime = true;
                }

                journeyLength = Vector3.Distance(transform.position, exitPosition.position);

                distCovered = (Time.time - startTime) * speed;
                fracJourney = distCovered / journeyLength;

                transform.position = Vector3.Lerp(transform.position, exitPosition.position, fracJourney);

                break;
        }
    }

    public void Activate()
    {
        isActive = true;
    }

    public bool InGrabbingRange()
    {
        if (Vector3.Distance(villager.transform.position, transform.position) < minGrabbingRange)
            return true;
        else
            return false;
    }

    public bool GrabAnimationIsOver()
    {
        timer += Time.deltaTime;

        if (timer > grabAnimationLength)
            return true;
        else
            return false;
    }

    void TakeDamage(int damage)
    {
        GetComponent<Stats>().ApplyDamage(damage);
        anim.SetBool("GotHit", true);
        if (GetComponent<Stats>().m_currHealth <= 0)
        {
            startedTime = false;
            timer = 0;
            currState = BalloonZombieStateMachine.Dying;
        }
    }
}

