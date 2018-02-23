using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerController : Unit {

	[Header("Unit")]
	[SerializeField] float accuracy = 0.9f;

	[Header("Movement")]
	[SerializeField] float speed = 4f;
	[SerializeField] float turnRate = 8f;

	Animator animator;

	Vector3 targetVector;
	bool isFiring;

	Rigidbody rigidBody;
	float camRayLength = 100f;
	float hThrow, vThrow;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
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
		float walkSpeedFactor = 12f;

		hThrow = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		vThrow = CrossPlatformInputManager.GetAxisRaw("Vertical");

		movement.Set(hThrow, 0f, vThrow);
		movement = movement.normalized * speed * Time.deltaTime;

		Vector3 heading = transform.InverseTransformDirection(movement);
		float localVelocity = heading.z;

		animator.SetFloat("walkDirection", localVelocity);
		animator.SetFloat("walkSpeed", movement.magnitude * walkSpeedFactor);

		rigidBody.MovePosition(transform.position + movement);
	}

	private void ProcessTurning() {
		int hitTest = LayerMask.GetMask("HitTest");
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit floorHit;

		if (Physics.Raycast(camRay, out floorHit, camRayLength, hitTest)) {
			targetVector = floorHit.point - transform.position;
			targetVector.y = 0f;

			var rotation = Quaternion.LookRotation(targetVector);
			rigidBody.MoveRotation(Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnRate));
		}
	}

	public float Accuracy() {
		return 1f - accuracy;
	}

	public Vector3 TargetVector() {
		return targetVector;
	}

	public Vector3 Position() {
		return transform.position;
	}
}
