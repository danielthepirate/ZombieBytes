using UnityEngine;

public class AlignWithObject : MonoBehaviour {

	public GameObject followObject;
	
	// Update is called once per frame
	void Update () {
		if (followObject) {
			Vector3 follow = new Vector3(followObject.transform.position.x, 0f, followObject.transform.position.z);
			transform.position = follow;
		}
	}
}
