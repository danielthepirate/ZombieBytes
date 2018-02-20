using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerController : MonoBehaviour {

	[Header("Unit")]
	[SerializeField] float accuracy = 0.9f;

	[Header("Movement")]
	[SerializeField] float speed = 4f;
	[SerializeField] float turnRate = 8f;

	Vector3 targetVector;
	bool isFiring;

	Rigidbody rigidBody;
	float camRayLength = 100f;
	float hThrow, vThrow;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		ProcesssInput();
	}

	private void ProcesssInput() {
		ProcessMovement();
		ProcessTurning();
	}

	private void ProcessMovement() {
		Vector3 movement = new Vector3();

		hThrow = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		vThrow = CrossPlatformInputManager.GetAxisRaw("Vertical");

		movement.Set(hThrow, 0f, vThrow);
		movement = movement.normalized * speed * Time.deltaTime;

		rigidBody.MovePosition(transform.position + movement);
	}

	private void ProcessTurning() {
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit floorHit;

		//if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)) {		//source uses a floorMask for..?
		if (Physics.Raycast(camRay, out floorHit, camRayLength)) {
			targetVector = floorHit.point - transform.position;
			targetVector.y = 0f;

			var rotation = Quaternion.LookRotation(targetVector);
			rigidBody.MoveRotation(Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnRate));
		}
	}

	public float Accuracy() {
		return accuracy;
	}

	public Vector3 TargetVector() {
		return targetVector;
	}
}
