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
    [SerializeField]
    [Range(0f, 1f)]
    private float precision;
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
        if (Menu.Instance.Paused)//checa se o jogo está pausado
            return;

        if (state.Health == 0)
        {
            Die();
            return;//inimigo destruído, não deve fazer mais nada
        }
        
        if (playerState.Health == 0)
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

        motor.stoppingDistance = attackRange;
        motor.SetDestination(playerState.transform.position);
    }

    private void UpdateAttack()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(DirectionToPlayer), Time.deltaTime * aimSpeed);

        if (Vector3.Dot(transform.forward, DirectionToPlayer) > .9f)
        {
            Quaternion inprecision = Quaternion.AngleAxis(Random.Range(0f, 1f - precision) * 35f, Vector3.up);
            var firePoint = weapon.FirePoint.position;
            firePoint = inprecision * (playerState.transform.position - firePoint).normalized;
            weapon.Fire(firePoint);
        }

        if (motor.velocity.magnitude == 0f)//faz o inimigo andar aleatoriamente enquanto atira
        {
            const float offset = 8f;
            Vector3 d = new Vector3(Random.Range(-offset, offset), 0f, Random.Range(-offset, offset));

            motor.stoppingDistance = 1f;
            motor.SetDestination(playerState.transform.position + d);
        }

        if(DistanceToPlayer > attackRange)
        {
            behaviour = Behaviour.RUNNING;
        }
    }

    private void Die()
    {
        EnemieManager.Instance.RegisterDeath();
        if(destroyedVersion != null) Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
