using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour {

	ParticleSystem particleLauncher;
	Gradient particleColorGradient;
	public GameObject decalPrefab;
	//public ParticleDecalPool dropletDecalPool;

	List<ParticleCollisionEvent> collisionEvents;

	// Use this for initialization
	void Start () {
		particleLauncher = GetComponent<ParticleSystem>();
		collisionEvents = new List<ParticleCollisionEvent>();
	}
	
	// Update is called once per frame
	void Update () {
		//particleLauncher.Emit(1);
	}

	private void OnParticleCollision(GameObject other) {
		int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);

		CreateBloodSplat();
	}

	private void CreateBloodSplat() {
		float randomAngle = Random.Range(0f, 360f);
		float randomScale = Random.Range(-0.2f, 0.2f);

		GameObject decal = Instantiate(decalPrefab);
		decal.transform.position = collisionEvents[0].intersection;
		decal.transform.Rotate(0f, 0f, randomAngle);
		decal.transform.localScale += new Vector3(randomScale, randomScale, randomScale);
	}
}
