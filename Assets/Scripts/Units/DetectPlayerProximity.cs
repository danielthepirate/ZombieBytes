using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerProximity : MonoBehaviour {

	[SerializeField] GameObject unit;

	EnemyController enemy;
	PlayerController player;

	private void Start() {
		player = FindObjectOfType<PlayerController>();
		enemy = unit.GetComponent<EnemyController>();

	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject == player.gameObject) {
			enemy.playerInProximity = true;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject == player.gameObject) {
			enemy.playerInProximity = false;
		}
	}
}
