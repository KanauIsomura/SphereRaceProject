using UnityEngine;
using System.Collections;

/// <summary>
/// タイトルロゴを揺らす
/// </summary>
public class TitleLogoRotation : MonoBehaviour {

    public float  m_fRotPower = 10; //回転力
    Vector3       m_InitAngle;      //初期角度

	// Use this for initialization
	void Start () {
        m_InitAngle = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 Angle = transform.eulerAngles;
        Angle.z = m_InitAngle.z + m_fRotPower * Mathf.Sin(Time.time);
        transform.eulerAngles = Angle;
	}
}
