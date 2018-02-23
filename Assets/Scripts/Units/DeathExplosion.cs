using UnityEngine;

public class DeathExplosion : MonoBehaviour {

	[SerializeField] float power = 50f;
	[SerializeField] float radius = 5f;
	[SerializeField] float upwardsForce = 1f;

	void Start () {
		float powerRandom;

		Vector3 explosionPoint = transform.position;
		explosionPoint += new Vector3(
			Random.Range(-0.25f, 0.25f), 
			Random.Range(-0.25f, 0.25f), 
			Random.Range(-0.25f, 0.25f)
		);


		Collider[] colliders = Physics.OverlapSphere(explosionPoint, radius);

		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody>();
			if (rb) {
				powerRandom = Random.Range(power * 0.85f, power * 1.15f);
				rb.AddExplosionForce(powerRandom, explosionPoint, radius, upwardsForce);
			}
		}
	}
}
