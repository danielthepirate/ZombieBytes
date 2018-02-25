using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] float smoothing = 5f;

	PlayerController player;
	Vector3 offset;

	private void Start() {
		player = target.gameObject.GetComponent<PlayerController>();
		offset = transform.position - target.position;
	}

	private void Update() {
		if (player.isDead) { return; }
		Vector3 targetCamPos = target.position + offset;
		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
