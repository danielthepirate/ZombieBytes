﻿using UnityEngine;

public class DetectPlayerProximity : MonoBehaviour {

	[SerializeField] GameObject unit;

	EnemyController enemy;
	PlayerController player;

	private void Start() {
		player = FindObjectOfType<PlayerController>();
		enemy = unit.GetComponent<EnemyController>();
	}

	private void OnTriggerEnter(Collider other) {
		if (player == null) { return; }

		if (other.gameObject == player.gameObject) {
			enemy.playerInProximity = true;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (player == null) { return; }

		if (other.gameObject == player.gameObject) {
			enemy.playerInProximity = false;
		}
	}
}
