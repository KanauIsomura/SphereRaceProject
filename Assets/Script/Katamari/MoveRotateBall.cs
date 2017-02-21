using UnityEngine;
using System.Collections;

/// <summary>
/// 前回座標と現在座標差で回転を加える
/// </summary>
public class MoveRotateBall : MonoBehaviour {
	public float							RotatePower = 100.0f;

	Vector3									OldPosiiton;	//ひとつ前のフレーム座標
	[SerializeField] CharacterController	Sphere;			//自身の円のコライダー

	// Use this for initialization
	void Start () {
		OldPosiiton = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		BallRoll();
	}
	
	/// <summary>
	/// 玉を回転させる
	/// </summary>
    void BallRoll(){
		//前回座標と現在座標が違うなら回転を加える
		if(OldPosiiton != transform.position){
			//軸を作る
			Vector3 VecX, VecY, VecZ;
			Vector3 PositionDifference;     //座標差で回転する量を決定

			VecY = Vector3.up;
			VecZ = PositionDifference = OldPosiiton - transform.position;       //移動量

			//Z軸の正規化
			VecZ.Normalize();

			//X軸を求める
			VecX = Vector3.Cross(VecY, VecZ);
			VecX.Normalize();

			//移動量から角度を出す
			float fDegree = PositionDifference.magnitude / (2 * Mathf.PI * Sphere.radius) * 360;

			//前回座標が現在座標の方向に向いた状態のX軸で回転を加える
			PositionDifference.y = 0.0f;    //Y座標は考慮しない
			transform.RotateAround(transform.position, VecX, -fDegree);
		}
		
		//過去座標として座標を保存
		OldPosiiton = transform.position;
    }
}
