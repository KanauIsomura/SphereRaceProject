using UnityEngine;
using System.Collections;

public class CharacterControlGravity : MonoBehaviour {
	private CharacterController		m_CharaCon;	//キャラコンへの参照
	private Vector3				m_Gravity;	//かかっている重力

	// Use this for initialization
	void Start () {
		m_CharaCon = GetComponent<CharacterController>();
		if(m_CharaCon == null){
			Destroy(this);
			return;
		}

		m_Gravity = Vector3.zero;
	}
	
	void FixedUpdate() {
		if(m_CharaCon.isGrounded){
			m_Gravity = Vector3.zero;
		}

		//プレイヤーに重力をかける
		m_Gravity += Physics.gravity * Time.fixedDeltaTime;
		m_CharaCon.Move(m_Gravity * Time.fixedDeltaTime);
	}
}
