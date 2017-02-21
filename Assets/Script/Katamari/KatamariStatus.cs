using UnityEngine;
using System.Collections;

public class KatamariStatus : MonoBehaviour {
	public float m_Size = 1.0f;	//現在の塊のサイズ

	// Use this for initialization
	/*void Start () {
		m_Size = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}*/

	/// <summary>
	/// ゲッターセッター
	/// </summary>
	public float KatamariSize{
		get{ return m_Size;}
		set{ m_Size = value;}
	}
}
