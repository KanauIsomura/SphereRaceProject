using UnityEngine;
using System.Collections;

/// <summary>
/// 土煙パーティクル表示切替
/// </summary>
public class SmokeParticle : MonoBehaviour {
    
    public float m_fStartSpeed = 10.0f;     // 再生を始める速度
    PlayerMove m_PlayerMove;        // 速度取得用
    ParticleSystem m_Particle;      // 再生管理用
    bool m_bStart;

	// Use this for initialization
	void Start () {
        m_PlayerMove = GameObject.Find("Player").gameObject.GetComponent<PlayerMove>();
        m_Particle = GetComponent<ParticleSystem>();
        m_bStart = false;
        m_Particle.Stop();
	}
	
	// Update is called once per frame
	void Update () {
	    //速度判定
        //if (Mathf.Abs(m_PlayerMove.SideSpeed) >= m_fStartSpeed || Mathf.Abs(m_PlayerMove.VerticalSpeed) >= m_fStartSpeed)
        //    SetStartFlg(true);
        //else
        //    SetStartFlg(false);
	}

    void SetStartFlg(bool bFlg)
    {
        //現在のStartFlgと同じなら変更をしない
        if (m_bStart == bFlg) return;

        //変更する
        m_bStart = bFlg;

        //パーティクルを切り替える
        if(m_bStart == true)
            m_Particle.Play();
        else
            m_Particle.Stop();
    }
}
