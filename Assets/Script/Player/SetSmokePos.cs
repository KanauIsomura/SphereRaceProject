using UnityEngine;
using System.Collections;

//土煙の高さ調整
public class SetSmokePos : MonoBehaviour {

    CharacterController m_CharaCon;

	// Use this for initialization
	void Start () {
        m_CharaCon = GameObject.Find("Player").GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 Pos = transform.localPosition;
        Pos.y = -m_CharaCon.radius;
        transform.localPosition = Pos;
	}
}
