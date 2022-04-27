using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAEnemy : MonoBehaviour
{
    public enum WanderType { Random, Waypoint};

    [Header ("--Behavior--")]
    public WanderType wanderType = WanderType.Random; //Default value
    public Transform[] waypoints; //Solo en el WanderType.Waypoint

    [Header("--Stats--")]
    public int health = 100;
    public int damageToPlayer;
    public float setTimerForDeath;
    public float cooldownTimerDeath;
    public float losePlayerTimer = 5f; //Tiempo en el que el zombie perderá al player luego de parar
    public float loseTimer = 0f;

    [Header ("--References--")]
    public Transform lookPoint;
    public Transform player;
    public SphereColliderPlayer sphereColliderPlayer;
    PlayerHealth playerHealth;
    bool _bIsHiting;
    bool _bIsAttacking;
    NavMeshAgent agent;
    Vector3 wanderPoint;
    Animator animator;

    [Header("--WanderStats--")]  
    public float wanderSpeed = 3f;
    public float wanderRadius = 2;
    public float chaseSpeed = 4.5f;
    public float fieldOfView = 120f;
    public float distanceOfView = 10f;
    public float minDistanceToPlayer;
    int waypointIndex = 0; 
    bool reviveEnable;
    bool targetOn;
    bool isDetecting;
    bool canMove = true;

    [Header("--Attack--")]
    public LayerMask playerMask;
    public Transform sphereCastSpawn;
    public float radiusSphereCast;
    public float setTimerAttack;
    public float cooldownAttack;
    bool canAttack = true;

    [Header("--Sound--")]
    public AudioClip[] hitSound;
    public AudioClip[] deathSound;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        wanderPoint = RandomWanderPoint();

        playerHealth = player.gameObject.GetComponent<PlayerHealth>();
    }
    private void Update()
    {
        TimerRevive();

        if (health <= 0)
        {
            return;
        }

        if(!_bIsHiting && !_bIsAttacking && canMove)
        {
            //Condición para cuando vea o escuche al player
            if (targetOn && !playerHealth.bDeath)
            {
                agent.SetDestination(player.position); //Destino del enemigo
                agent.speed = chaseSpeed;              //Cambio de velocidad

                animator.SetBool("Walk", false);
                animator.SetBool("Idle", false);

                //Condición para cuando el enemigo siga en persecución pero no esté viendo o escuchando al player
                if (!isDetecting)
                {
                    loseTimer += Time.deltaTime;

                    //Condición para cuando el enemigo pierde al player, lo seguirá por 2.5 s más, luego estará quieto otro 2.5s y volverá al estado de Wander()
                    if (loseTimer >= (losePlayerTimer / 2))
                    {
                        agent.isStopped = true;
                        animator.SetBool("Idle", true);

                        if (loseTimer >= losePlayerTimer)
                        {
                            animator.SetBool("Idle", false);
                            targetOn = false;
                            loseTimer = 0f;
                        }
                    }
                }
            }
            else if (!targetOn) //Condición para cuando detecta al player
            {
                agent.isStopped = false;
                
                Wander();
                agent.speed = wanderSpeed;

                animator.SetBool("Walk", true);
                animator.SetBool("Idle", false);
            }

            //Condición para que si el player está muerto deje de perseguirlo
            if (!playerHealth.bDeath) FindPlayer();
            else targetOn = false;
        }

        TimerAttack();
    }
    private void LateUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit"))
        {
            _bIsHiting = true;
        }
        else
        {
            _bIsHiting = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Die"))
        {
            canMove = false;
            gameObject.layer = 12;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            _bIsAttacking = true;
        }
        else
        {
            agent.isStopped = false;
            _bIsAttacking = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("StandUp"))
        {
            canMove = false;
        }
    }

    void TimerAttack() //Función para el timer de atacar
    {
        if(!canAttack)
        {
            setTimerAttack += Time.deltaTime;

            if (setTimerAttack >= cooldownAttack)
            {
                canAttack = true;
                setTimerAttack = 0f;
            }
        }
    }
    void TimerRevive() //Función para el timer de revivir
    {
        if (reviveEnable)
        {
            setTimerForDeath += Time.deltaTime;

            if (setTimerForDeath >= cooldownTimerDeath)
            {
                animator.SetTrigger("StandUp");
                health = 100;

                reviveEnable = false;
                setTimerForDeath = 0f;
            }
        }
    }
    public void FindPlayer() //Función por la cual el enemigo encuentra al player
    {
        //Compara el ángulo en el espacio local del enemigo
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(player.position)) < fieldOfView /2)
        {
            float distaceToPlayer = Vector3.Distance(transform.position, player.position);

            //Distancia con el player
            if (distaceToPlayer < distanceOfView)
            {
                RaycastHit hit;
                if(Physics.Linecast(transform.position, player.position, out hit, -1))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        PlayerOnRange();

                        if(distaceToPlayer <= minDistanceToPlayer && !_bIsAttacking && canAttack && canMove)
                        {
                            agent.isStopped = true;

                            //Random number para disparar el trigger de uno de los 3 ataques posibles
                            int randomNumber = Random.Range(1, 4);

                            if (randomNumber == 1) animator.SetTrigger("Attack");
                            else if (randomNumber == 2) animator.SetTrigger("Attack2");
                            else animator.SetTrigger("Attack3");

                            canAttack = false;

                        }
                    }
                    else
                    {
                        if (!sphereColliderPlayer.onRange)
                        {
                            isDetecting = false;
                        }
                    }
                }
            }
            else
            {
                if (!sphereColliderPlayer.onRange)
                {
                    isDetecting = false;
                }
            }
        }
        else
        {
            if (!sphereColliderPlayer.onRange)
            {
                isDetecting = false;
            }
        }
    }
    void FaceTarget() //Función test para la rotación del enemigo cuando tiene cerca al player
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    public void PlayerOnRange() //Función para cambiar los booleanos cuando detecte al player
    {
        targetOn = true;
        isDetecting = true;
        loseTimer = 0f;
    }
    public void Attack() //Función para el ataque, llamada en el script EnemyAttack
    {
        //Esfera para que cuando se dañe al enemigo este pase al estado de persecución
        Collider[] player = Physics.OverlapSphere(sphereCastSpawn.position, radiusSphereCast, playerMask);
        for (int i = 0; i < player.Length; i++)
        {
            player[i].GetComponent<PlayerHealth>().OnHit(damageToPlayer);
        }
    }
    public void Revive() //Función llamada en la animación Zombie Scream desde el script EnemyAttack
    {
        health = 100;

        targetOn = false;
        agent.isStopped = false;
        canMove = true;
        gameObject.layer = 10;
    }
    public void Wander() //Función para el comportamiento del enemigo (Random o Wander)
    {
        //Condición para el comportamiento Random
        if(wanderType == WanderType.Random) //Random behavior
        {
            if(Vector3.Distance(transform.position, wanderPoint) < minDistanceToPlayer)
            {
                wanderPoint = RandomWanderPoint();
            }
            else
            {
                agent.SetDestination(wanderPoint);
                lookPoint.position = wanderPoint; //Para visualizar el punto en escena
            }
        }
        else  //Waypoint behavior
        {
            //Condición para evitar errores. Longitud mínima del array: 2
            if(waypoints.Length >= 2)
            {
                if(Vector3.Distance(waypoints[waypointIndex].position, transform.position) < minDistanceToPlayer)
                {
                    //Condición para cuando el enemigo llegue al último punto del array, este vuelva a comenzar desde el principio
                    if(waypointIndex == waypoints.Length - 1)
                    {
                        waypointIndex = 0;
                    }
                    else
                    {
                        waypointIndex++;
                    }
                }
                else
                {
                    agent.SetDestination(waypoints[waypointIndex].position);
                }
            }
            else
            {
                Debug.LogError("No hay suficientes waypoints para el enemigo. Waypoints actuales " + waypoints.Length + ". IA: " + gameObject.name);
            }
        }
    }
    public Vector3 RandomWanderPoint() //Función para generar un punto random dentro de al esfera declarada (Random Behavior)
    {
        Vector3 randomPoint = (Random.insideUnitSphere * wanderRadius) + transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, wanderRadius, -1);

        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }
    public void OnHit(int damage) //Función para el daño hacia el enemigo
    {
        PlayerOnRange();
        health -= damage;

        if(health <= 0)
        {
            agent.isStopped = true;

            animator.SetTrigger("Death");

            SoundManager.instance.PlaySound(deathSound[Random.Range(0,deathSound.Length)]);

            reviveEnable = true; //Booleano para iniciar el timer de revive
        }
        else
        {
            animator.SetTrigger("Hit");
            SoundManager.instance.PlaySound(hitSound[Random.Range(0, hitSound.Length)]);
        }
    }
    private void OnDrawGizmos() //Función para visualizar el radio de ataque y detección
    {
        Gizmos.DrawWireSphere(sphereCastSpawn.position, radiusSphereCast);

        Gizmos.DrawLine(transform.position, player.position);
    }
}
