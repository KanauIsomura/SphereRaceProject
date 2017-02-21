using UnityEngine;
using System.Collections;

public class PlayerHeight : MonoBehaviour {

	RaycastHit hit;
	public float DistanceAway;  // 離れる距離
	private float posY = -1000;
	[SerializeField]
	private CharacterController katamariCollider;
	void Update() {
		Vector3 RayStartPos = transform.localPosition;
		RayStartPos.y += 1800;
		Ray ray = new Ray(RayStartPos, Vector3.down);
		var obj = Physics.RaycastAll(ray, 2000);
		//if(Physics.Raycast(RayStartPos, Vector3.down, out hit, 1000)) {
		//posY = obj[i].transform.localPosition.y;
		posY = katamariCollider.radius;
		transform.localPosition = new Vector3(transform.localPosition.x, posY + DistanceAway, transform.localPosition.z);
		posY = -1000;
	}
}