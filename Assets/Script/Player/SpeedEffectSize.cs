using UnityEngine;
using System.Collections;

/// <summary>
/// 風パーティクルサイズ設定
/// </summary>
public class SpeedEffectSize : MonoBehaviour {

    //塊のサイズ1に対しての加算値
    public float m_fAddLifetime = 0.005f;
    public float m_fAddSpeed    = 0.125f;
    public float m_fAddSize     = 0.23f;
    public float m_fAddRadius   = 0.12f;       

    float m_fOldSize;               //前回サイズ保存用
    KatamariStatus m_KatamariSize;  //現在の塊サイズ取得用
    ParticleSystem Effect;

    /// <summary>
    /// 初期化
    /// </summary>
	void Start () {
        m_KatamariSize = GameObject.Find("Player").GetComponent<KatamariStatus>();
        m_fOldSize = m_KatamariSize.m_Size;
        Effect = GetComponent<ParticleSystem>();
	}
	
    
    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        //サイズに変更がないなら更新を行わない
        if (m_fOldSize == m_KatamariSize.m_Size) return;

        //変更分のサイズ
        float size = m_KatamariSize.m_Size - m_fOldSize;

        //サイズ変更
        Effect.startLifetime += size * m_fAddLifetime;
        Effect.startSpeed    += size * m_fAddSpeed;
        Effect.startSize     += size * m_fAddSize;

        ParticleSystem.ShapeModule temp = Effect.shape;
        temp.radius += size * m_fAddRadius;

        //サイズ保存
        m_fOldSize = m_KatamariSize.m_Size;
	
	}
}
