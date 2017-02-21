using UnityEngine;
using System.Collections;

public class CorseMap : MonoBehaviour {
	[SerializeField]
	GameObject[] MapIconPrefab;
	[SerializeField]
	Transform[] IconPosition;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < IconPosition.Length; ++i) {
			GameObject NewIcon = Instantiate(MapIconPrefab[i]);
			NewIcon.GetComponent<CharIcon>().IconTransform = IconPosition[i];
			NewIcon.transform.position = new Vector3(0.0f, 345.0f, 0.0f);
			NewIcon.transform.parent = transform;
		}
	}
	
	// Update is called once per frame
	//void Update () {
	//}
}
