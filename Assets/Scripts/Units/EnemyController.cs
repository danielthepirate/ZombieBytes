using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//putting this here so I can access it from EnemyHealth... there's gotta be a smarter way to do this

public class EnemyController : MonoBehaviour {

	[SerializeField] float attackDamage = 20f;
	[SerializeField] float attackCooldown = 1f;
	[SerializeField] float attackDuration = 1f;
	[SerializeField] float attackDamagePoint = 0.4f;
	[SerializeField] float attackKnockback = 6f;

	public enum State { Pursue, Attack, Stun, Dead };
	public State state;
	public bool playerInRange;
	public bool playerInProximity;

	float stateResetTimer;
	float stateResetDelay;

	PlayerController player;
	Animator animator;

	float lookAngle = 80f;
	float lookSpeed = 1.4f;

	NavMeshAgent nav;
	float turnSpeed = 1.4f;
	float attackTimer;

	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();

		player = FindObjectOfType<PlayerController>();
	}

	// Update is called once per frame
	void Update () {
		if (state == State.Dead) { return; }

		AttemptToAttack();
		LookTowardsPlayer();
		MoveTowardsPlayer();

		ProcessStateResetTimer();
	}

	private void ProcessStateResetTimer() {
		if (stateResetDelay != 0f) {
			stateResetTimer += Time.deltaTime;
		}
		if (stateResetTimer >= stateResetDelay) {
			stateResetDelay = 0f;
			stateResetTimer = 0f;
			state = State.Pursue;
		}
	}

	public void ResetStateAfter(float delay) {
		stateResetTimer = 0f;
		stateResetDelay = delay;
	}

	private void AttemptToAttack() {
		attackTimer -= Time.deltaTime;
		if (player.isDead) { return; }

		if (CanAttack() && playerInRange && attackTimer < 0f) {
			Attack();
		}
	}

	private bool CanAttack() {
		if(state != State.Dead && state != State.Stun) { return true; }
		else { return false; }
	}

	private void Attack() {

		attackTimer = attackCooldown;
		state = State.Attack;
		ResetStateAfter(attackCooldown);

		animator.SetTrigger("Attack");
		animator.SetFloat("AttackSpeed", 1.2f / attackDuration);
		Invoke("DealDamage", attackDamagePoint);
	}

	private void DealDamage() {
		PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

		if (playerInRange) {
			playerHealth.Damage(attackDamage, attackKnockback, transform.position);
		}
	}

	private void MoveTowardsPlayer() {
		if (player.isDead) { return; }

		Vector3 look = player.Position() - transform.position;
		look.y = 0f;
		Quaternion rotationToTarget = Quaternion.LookRotation(look);

		//nav mesh agent doesnt seem to care about agent facing as much as we do
		//this corrects it so the zombie will turn to put the player into attack range
		if (playerInProximity && !playerInRange) {
			transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, Time.deltaTime * turnSpeed);
		}

		if (state == State.Pursue) {
			nav.enabled = true;
			nav.SetDestination(player.Position());
			animator.SetFloat("WalkSpeed", 0.5f);
		}
		else {
			nav.enabled = false;
			animator.SetFloat("WalkSpeed", 0f);
		}
	}

	private void LookTowardsPlayer() {
		if (player.isDead) { return; }

		Vector3 look = player.Position() - transform.position;
		look.y = 0f;

		Transform head = transform.Find("Head");
		Quaternion rotationToTarget = Quaternion.LookRotation(look);

		//this keeps head angle within natural limits
		if (Quaternion.Angle(transform.rotation, rotationToTarget) > lookAngle) {
			head.transform.rotation = Quaternion.Slerp(head.transform.rotation, transform.rotation, Time.deltaTime * lookSpeed);
		}
		else {
			head.transform.rotation = Quaternion.Slerp(head.transform.rotation, rotationToTarget, Time.deltaTime * lookSpeed);
		}
	}
}
