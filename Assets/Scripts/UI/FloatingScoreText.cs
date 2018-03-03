using UnityEngine;
using TMPro;

public class FloatingScoreText : MonoBehaviour {

	[SerializeField] float offset;
	[SerializeField] TextMeshPro textMesh;

	[Header("debug")]
	public int debugScore;
	public Transform debugSpawn;

	private void OnValidate() {
		SetScore(debugScore);
		if (debugSpawn) {
			SetWorldPosition(debugSpawn);
		}
	}

	private void Start() {
		GameObject bucket = GameObject.Find("BucketFloatingScoreText");
		transform.parent = bucket.transform;
	}

	public void SetWorldPosition(Transform spawn) {
		Vector3 worldPosition = new Vector3(spawn.position.x, spawn.position.y + offset, spawn.position.z);
		transform.position = worldPosition;
	}

	public void SetScore(int score) {
		textMesh.SetText("+" + score);
	}
}
