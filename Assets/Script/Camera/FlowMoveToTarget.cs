using UnityEngine;
using System.Collections;

public class FlowMoveToTarget : MonoBehaviour {
	public Transform	m_TargetTransform;			//移動先のTransform
	public float		m_MoveFlowTime	= 1.0f;	//ターゲットまでの移動にかかる時間
	public float		m_RotateFlowTim	= 1.0f;	//ターゲットと同じ角度になるまでの時間
	public Vector3	m_OffSetPosition	= new Vector3(0.0f, 1.5f, 0.0f);	//ロックポジションのオフセット座標

	// Use this for initialization
	void Start() {
		transform.position = m_TargetTransform.position;
		transform.rotation = m_TargetTransform.rotation;
	}

	// Update is called once per frame
	/*void Update() {
	}*/

	/// <summary>
	/// カメラ追跡用の更新
	/// </summary>
	void LateUpdate() {
		//設定したTargetまで移動する処理
		transform.position =
			Vector3.Lerp(transform.position, m_TargetTransform.position + m_OffSetPosition, Mathf.Clamp(Time.deltaTime / m_MoveFlowTime, 0.0f, 1.0f));
		transform.rotation = 
			Quaternion.Lerp(transform.rotation, m_TargetTransform.rotation, Mathf.Clamp(Time.deltaTime / m_RotateFlowTim, 0.0f, 1.0f));
		
		//transform.position = m_TargetTransform.position + m_OffSetPosition;
		
	}
}
