using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	[SerializeField] GameObject zombie;

	GameObject[] spawnPoints;

	// Use this for initialization
	void Start () {
		spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
	}

	public void SpawnZombie() {
		int spawnIndex = Random.Range(0, spawnPoints.Length);
		Transform spawnPoint = spawnPoints[spawnIndex].transform;
		GameObject newZombie = Instantiate(zombie, spawnPoint, false);
		
		GameObject player = GameObject.Find("Player");
		newZombie.transform.LookAt(player.transform);

	}
}
