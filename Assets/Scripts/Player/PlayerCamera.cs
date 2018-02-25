using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	[Header("Follow Target")]
	[SerializeField] Camera mainCamera;
	[SerializeField] Transform target;
	[SerializeField] float smoothing = 5f;

	[Header("Camera Shake")]
	[SerializeField] float reductionFactor = 2f;
	[SerializeField] float maxOffset = 0.25f;
	[SerializeField] float maxAngle = 3.5f;

	[Header("Camera Shake Presets")]
	[SerializeField][Range(0, 1)] float shakeLight = 0.45f;
	[SerializeField][Range(0, 1)] float shakeModer = 0.70f;
	[SerializeField][Range(0, 1)] float shakeHeavy = 0.95f;

	[Header("Debug Purposes")]
	[SerializeField][Range(0, 1)] float trauma;
	[SerializeField][Range(0, 1)] float shakeAmount;

	PlayerController player;
	Vector3 offset;
	Vector3 cameraPosition;
	Quaternion cameraRotation;

	private void Start() {
		player = target.gameObject.GetComponent<PlayerController>();
		offset = mainCamera.transform.position - target.position;
		cameraPosition = mainCamera.transform.position;
		cameraRotation = mainCamera.transform.rotation;

		AddTrauma(trauma);
	}

	private void FixedUpdate() {
		float shakeX = 0f;
		float shakeY = 0f;
		float shakeZ = 0f;
		float angle = 0f;
		shakeAmount = Mathf.Pow(trauma, 1.5f);

		if (player.isDead) { return; }
		Vector3 targetPosition = target.position + offset;
		cameraPosition = Vector3.Lerp(cameraPosition, targetPosition, smoothing * Time.deltaTime);

		if(trauma > 0.01f) {
			float reductionPerTick = reductionFactor * Time.deltaTime;
			trauma = Mathf.Clamp((trauma - reductionPerTick),0f,1f);

			shakeX = maxOffset * shakeAmount * Random.Range(-1f, 1f);
			shakeZ = maxOffset * shakeAmount * Random.Range(-1f, 1f);
			angle = maxAngle * shakeAmount * Random.Range(-1f, 1f);
		}
		else {
			trauma = 0f;
		}

		Quaternion shakeAngle = Quaternion.Euler(0, 0, angle);
		Vector3 shakeVector = new Vector3(shakeX, shakeY, shakeZ);

		mainCamera.transform.rotation = cameraRotation * shakeAngle;
		mainCamera.transform.position = cameraPosition + shakeVector;
	}

	public void AddTrauma(float t) {
		trauma = Mathf.Clamp(trauma + t, 0f, 1f);
	}

	//Preset shake levels
	public void ShakeLight() {
		AddTrauma(shakeLight);
	}

	public void ShakeModerate() {
		AddTrauma(shakeModer);
	}

	public void ShakeHeavy() {
		AddTrauma(shakeHeavy);
	}
}
