using UnityEngine;

public class AlignRectToObject : MonoBehaviour {

	public GameObject followObject;
	public float height;
	
	// Update is called once per frame
	void Update () {
		Align();
	}

	private void OnValidate() {
		Align();
	}

	private void Align() {
		if (followObject) {
			Vector3 worldPosition = new Vector3(followObject.transform.position.x, height, followObject.transform.position.z);
			Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
			transform.position = screenPosition;
		}
	}
}
