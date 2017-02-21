using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public float CameraFlowTime = 1.0f;	//移動する時間
	public float OffSetValuY = 0.5f;    //Y座標の塊が大きくなったときに移動する量の大きさ
	Vector3 CameraOffset;				//初期カメラの距離
	Vector3 TargetPoint;				//移動したいカメラ座標

	// Use this for initialization
	void Start () {
		CameraOffset = transform.localPosition;	//初期座標を入れる
		TargetPoint = transform.localPosition;
	}
	
	// Update is called once per frame
	/*void Update () {
		transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPoint, Mathf.Clamp(Time.deltaTime / CameraFlowTime, 0.0f, 1.0f));
	}*/

	/// <summary>
	/// カメラの更新
	/// </summary>
	void LateUpdate() {
		transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPoint, Mathf.Clamp(Time.deltaTime / CameraFlowTime, 0.0f, 1.0f));
	}

	/// <summary>
	/// 塊の大きさに合わせてカメラの距離を変える処理
	/// </summary>
	/// <param name="KatamariSize"></param>
	public void ChangeOffset(int KatamariSize){
		TargetPoint = new Vector3(CameraOffset.x, CameraOffset.y + (KatamariSize - 1) * OffSetValuY, CameraOffset.z - (KatamariSize - 1));
	}
}
