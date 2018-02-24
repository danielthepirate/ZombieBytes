using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//putting this here so I can access it from EnemyHealth... there's gotta be a smarter way to do this
public enum State { Pursue, Attack, Stun, Dead };

public class Zombie : MonoBehaviour {

	[SerializeField] float attackCooldown = 0.8f;
	[SerializeField] float attackDamage = 20f;

	public State state;

	PlayerController player;
	Animator animator;

	Quaternion targetRotation;
	float lookAngle = 50f;
	float lookSpeed = 1.4f;

	NavMeshAgent nav;
	bool playerInRange;
	float attackTimer;

	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();

		player = FindObjectOfType<PlayerController>();
		state = State.Pursue;
	}

	// Update is called once per frame
	void Update () {
		if (state == State.Dead) { return; }

		if (playerInRange) {
			AttemptAttack();
		}

		LookTowardsPlayer();
		MoveTowardsPlayer();
	}

	private void AttemptAttack() {
		if (player.isDead) { return; }

		attackTimer += Time.deltaTime;
		if (attackTimer >= attackCooldown) {
			Attack();
		}
	}

	private void Attack() {
		PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
		state = State.Attack;

		attackTimer = 0f;
		playerHealth.healthCurrent -= attackDamage;
	}

	public void ResetStateAfter(float delay) {
		//todo have a singlular state reset timer control this, so we dont get states interrupting each other's resets
		Invoke("ResetState", delay);
	}

	private void ResetState() {
		state = State.Pursue;
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject == player.gameObject) {
			playerInRange = true;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject == player.gameObject) {
			playerInRange = false;
		}
	}

	private void MoveTowardsPlayer() {
		if (state == State.Pursue) {
			nav.enabled = true;
			nav.SetDestination(player.Position());
			animator.SetFloat("walkSpeed", 0.5f);
		}
		else {
			nav.enabled = false;
			animator.SetFloat("walkSpeed", 0f);
		}
	}

	private void LookTowardsPlayer() {
		Vector3 look = player.Position() - transform.position;
		look.y = 0f;

		Transform head = transform.Find("Head");

		Quaternion rotationToTarget = Quaternion.LookRotation(look);
		if (Quaternion.Angle(rotationToTarget, transform.localRotation) <= lookAngle) {
			targetRotation = rotationToTarget;
		}

		if (Quaternion.Angle(transform.rotation, rotationToTarget) > lookAngle) {
			//this keeps head angle within natural limits
			head.transform.rotation = Quaternion.Slerp(head.transform.rotation, transform.localRotation, Time.deltaTime * lookSpeed);
		}
		else {
			head.transform.rotation = Quaternion.Slerp(head.transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
		}
	}
}
