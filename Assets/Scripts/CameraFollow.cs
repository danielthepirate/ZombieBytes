using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] float smoothing = 5f;

	Vector3 offset;

	private void Start() {
		offset = transform.position - target.position;
	}

	private void Update() {
		Vector3 targetCamPos = target.position + offset;
		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
