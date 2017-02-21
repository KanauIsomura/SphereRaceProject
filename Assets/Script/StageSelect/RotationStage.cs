using UnityEngine;
using System.Collections;

/// <summary>
/// ステージ回転
/// </summary>
public class RotationStage : MonoBehaviour {

    public float m_fRotSpeed = 3.0f;   //回転速度

    /// <summary>
    /// 更新処理
    /// </summary>
	void Update () {
        transform.Rotate(Vector3.up, m_fRotSpeed * Time.deltaTime);
	}
}
