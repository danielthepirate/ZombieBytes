using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	[SerializeField] GameObject hitDecal;
	[SerializeField] GameObject ragdoll;

	public PlayerCamera playerCamera;

	public float healthCurrent = 100f;
	public float healthMaximum = 100f;
	[Tooltip("per second")]
	public float healthRegen = 1f;

	public Image healthBar;

	float healthTick = 0.0625f;
	float healthPerTick;
	float timer;

	GameObject bucketFX;
	GameObject bucketRagdolls;
	SceneController sceneController;
	PlayerController player;

	private void Start() {
		player = gameObject.GetComponent<PlayerController>();
		sceneController = FindObjectOfType<SceneController>();

		bucketFX = GameObject.Find("BucketFX");
		bucketRagdolls = GameObject.Find("BucketRagdolls");

		healthPerTick = healthRegen * healthTick;
	}

	private void Update() {
		timer -= Time.deltaTime;

		if(timer < 0) {
			timer = healthTick;
			if(healthCurrent < healthMaximum) {
				healthCurrent += healthPerTick;
				UpdateHealthBar();
			}
		}
	}

	public void Damage(float damageAmount, float knockBack, Vector3 damageOrigin) {
		ApplyKnockback(knockBack, damageOrigin);
		ApplyCameraShake();
		CreateHitDecal();

		healthCurrent -= damageAmount;
		UpdateHealthBar();

		if (healthCurrent <= 0) {
			KillUnit(knockBack, damageOrigin);
		}
	}

	private void ApplyCameraShake() {
		playerCamera.ShakeLight();
	}

	private void ApplyKnockback(float knockBack, Vector3 damageOrigin) {
		float forceFactor = 2f; //without this the force doesnt knockback enough
		Rigidbody rb = GetComponent<Rigidbody>();
		Vector3 forceDirection = transform.position - damageOrigin;
		forceDirection = forceDirection.normalized;

		Vector3 force = forceDirection * knockBack * forceFactor;
		rb.AddForce(force, ForceMode.Impulse);
	}

	private void UpdateHealthBar() {
		healthBar.fillAmount = healthCurrent / healthMaximum;
	}

	private void KillUnit(float knockBack, Vector3 damageOrigin) {
		player.isDead = true;
		BoxCollider cb = GetComponent<BoxCollider>();
		SphereCollider cs = GetComponent<SphereCollider>();
		cb.enabled = false;
		cs.enabled = false;

		GameObject newRagdoll = Instantiate(ragdoll, transform.position, transform.rotation, bucketRagdolls.transform);
		PlayerDeath playerDeath = newRagdoll.GetComponent<PlayerDeath>();
		playerDeath.damageOrigin = damageOrigin;
		playerDeath.knockback = knockBack;

		//destroying player gameobject procs errors in PlayerProximityCollider.cs even if I check for null
		//so we hide the player instead
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers) {
			renderer.enabled = false;
		}

		sceneController.DelayedReloadLevel();
	}

	private void CreateHitDecal() {
		float randomAngle = Random.Range(0f, 360f);
		float randomScale = Random.Range(-0.2f, 0.2f);

		GameObject decal = Instantiate(hitDecal, bucketFX.transform);
		decal.transform.position = transform.position;
		decal.transform.Rotate(0f, 0f, randomAngle);
		decal.transform.localScale += new Vector3(randomScale, randomScale, randomScale);
	}
}
