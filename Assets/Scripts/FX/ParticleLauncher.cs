using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour {

	public GameObject decalPrefab;

	[SerializeField] float sprayDistance = 5f;

	ParticleSystem particleLauncher;
	List<ParticleCollisionEvent> collisionEvents;

	// Use this for initialization
	void Start () {
		particleLauncher = GetComponent<ParticleSystem>();
		collisionEvents = new List<ParticleCollisionEvent>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	private void OnParticleCollision(GameObject other) {
		ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);

		for (int i = 0; i < collisionEvents.Count; i++) {
			SpawnDecalBloodDroplets(collisionEvents[i]);	
		}
	}

	private void SpawnDecalBloodDroplets(ParticleCollisionEvent particleCollisionEvent) {
		float randomAngle = Random.Range(0f, 360f);
		float randomScale = Random.Range(-0.2f, 0.2f);

		Ray precise = new Ray();
		RaycastHit hit;

		//raising the collision height slightly makes the blood droplet positioning look better
		Vector3 rayOrigin = particleCollisionEvent.intersection;
		rayOrigin.y += 0.1f;
		precise.origin = rayOrigin;
		precise.direction = (particleCollisionEvent.intersection - transform.position).normalized;

		print(1);

		if (Physics.Raycast(precise, out hit, sprayDistance)) {
			print(2);
			GameObject decal = Instantiate(decalPrefab);
			decal.transform.position = hit.point;

			decal.transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal * -1f);
			decal.transform.Rotate(0f, 0f, randomAngle);

			decal.transform.localScale += new Vector3(randomScale, randomScale, randomScale);
		}
	}
}
