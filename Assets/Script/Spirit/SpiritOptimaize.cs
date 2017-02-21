using UnityEngine;
using System.Collections;

public class SpiritOptimaize : MonoBehaviour {
	[SerializeField]
	float m_OpitimaizeTime = 1.0f;	//スピリットの位置が最適化されるまでの時間

	bool		m_OpitimaizeStart;		//最適化スタート
	float	m_StartTime;			//開始時間
	Vector3	m_EndPoint;			//移動先地点
	Vector3	m_StartPoint;			//開始地点

	public float OptimaizeTime{
		set{m_OpitimaizeTime = value;}
	}

	// Use this for initialization
	void Start () {
		m_OpitimaizeStart = false;
	}
	
	// Update is called once per frame
	void Update() {
		//if(m_OpitimaizeStart){
		//    //時間で目標地点まで動かす
		//    transform.localPosition = m_EndPoint; //Vector3.Lerp(m_StartPoint, m_EndPoint, Mathf.Clamp((Time.time - m_StartTime) / m_OpitimaizeTime, 0.0f, 1.0f));

		//    //最終位置まで移動したら処理を止める
		//    if(transform.localPosition == m_EndPoint){
		//        //gameObject.GetComponent<Collider>().enabled = false;
		//        m_OpitimaizeStart = false;
		//    }
		//}
	}

	/// <summary>
	/// 最適化スタート関数
	/// </summary>
	/// <param name="fRadius">くっつけたい半径</param>
	public void StartOptimaize(float fRadius) {
		Vector3 VecZ = transform.localPosition;	//ローカル座標を入れる
		VecZ.Normalize();						//Z方向べクトルを1に

		//最適化のスタート地点と終わりの地点を設定
		m_StartPoint	= VecZ * transform.localPosition.magnitude;
		m_EndPoint	= VecZ * fRadius;

		//最適化をスタートさせる
		m_OpitimaizeStart = true;

		m_StartTime = Time.time;

		transform.localPosition = m_EndPoint;
	}
}
