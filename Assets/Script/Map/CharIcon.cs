using UnityEngine;
using System.Collections;

public class CharIcon : MonoBehaviour {
	Transform m_IconTransform;
	
	public Transform IconTransform{
		get { return m_IconTransform; }
		set { m_IconTransform = value; }
	}

	// Update is called once per frame
	void Update () {
		//対象キャラクターと同じ位置に
		transform.position = new Vector3(
			m_IconTransform.position.x, transform.position.y, m_IconTransform.position.z);
	}
}
