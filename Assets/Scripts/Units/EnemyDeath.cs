using UnityEngine;

public class EnemyDeath : MonoBehaviour {

	[SerializeField] float power = 1f;

	void Start () {
		float powerRandom;
		float forceFactor = 0.35f;

		Vector3 explosionPoint = transform.position;
		Vector3 force;

		explosionPoint += new Vector3(
			Random.Range(-0.3f, 0.3f), 
			Random.Range(0f, 0.5f), 
			Random.Range(-0.3f, 0.3f)
		);

		Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

		foreach (Rigidbody rb in rigidbodies) {
			powerRandom = Random.Range(power * 0.85f, power * 1.15f);
			force = rb.transform.position - transform.position;
			force = force.normalized * forceFactor * powerRandom;
			rb.transform.parent = null;
			rb.AddForceAtPosition(force, explosionPoint, ForceMode.Impulse);
		}
		Destroy(gameObject);
	}
}
