using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterState))]
[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemieAI : MonoBehaviour
{
    [SerializeField]
    private float visionRange = 70f;
    [SerializeField]
    private float attackRange = 60f;
    [SerializeField]
    private float aimSpeed = 60f;
    [SerializeField][Range(0f, 1f)]
    private float precision = 0.4f;
    [SerializeField]
    private float moveSpeed = 20f;
    [SerializeField]
    private GameObject destroyedVersion;

    private enum Behaviour
    {
        IDLE, RUNNING, ATTACK
    }

    private Behaviour behaviour;
    private WeaponController weapon;
    private CharacterState playerState;
    private CharacterState state;
    private NavMeshAgent motor;

    private void Start()
    {
        behaviour = Behaviour.IDLE;
        state = GetComponent<CharacterState>();
        playerState = PlayerController.Instance.GetComponent<CharacterState>();
        weapon = GetComponent<WeaponController>();
        motor = GetComponent<NavMeshAgent>();

        motor.speed = moveSpeed;
        motor.stoppingDistance = attackRange;
        motor.Warp(transform.position);
    }

    private void Update()
    {
        if(state.Health == 0)
        {
            Die();
        }
        else if (playerState.Health == 0)
        {
            behaviour = Behaviour.IDLE;
        }

        switch (behaviour)
        {
            case Behaviour.IDLE:
                UpdateIdle();
                break;

            case Behaviour.RUNNING:
                UpdateRunning();
                break;

            case Behaviour.ATTACK:
                UpdateAttack();
                break;
        }
    }

    private float DistanceToPlayer
    {
        get => Vector3.Distance(transform.position, playerState.transform.position);
    }

    private Vector3 DirectionToPlayer
    {
        get => (playerState.transform.position - transform.position).normalized;
    }

    private void UpdateIdle()
    {
        if(DistanceToPlayer <= visionRange && playerState.Health > 0)
        {
            behaviour = Behaviour.RUNNING;
        }
    }

    private void UpdateRunning()
    {
        if(DistanceToPlayer <= attackRange)
        {
            behaviour = Behaviour.ATTACK;
        }

        if(DistanceToPlayer > visionRange)
        {
            behaviour = Behaviour.IDLE;
        }

        motor.SetDestination(playerState.transform.position);
    }

    private void UpdateAttack()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(DirectionToPlayer), Time.deltaTime * aimSpeed);

        if (Vector3.Dot(transform.forward, DirectionToPlayer) > .9f)
        {
            float p = (1f - precision) * 20f;
            transform.forward = Quaternion.AngleAxis(Random.Range(-p, p), Vector3.up) *
                                Quaternion.AngleAxis(Random.Range(-p, p), Vector3.right) *
                                Quaternion.AngleAxis(Random.Range(-p, p), Vector3.forward) * transform.forward;
            weapon.Fire();
        }

        if(DistanceToPlayer > attackRange)
        {
            behaviour = Behaviour.RUNNING;
        }
    }

    private void Die()
    {
        if(destroyedVersion != null) Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
