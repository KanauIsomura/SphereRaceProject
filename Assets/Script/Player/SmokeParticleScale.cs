using UnityEngine;
using System.Collections;


/// <summary>
/// 土煙パーティクルサイズ変更
/// </summary>
public class SmokeParticleScale : MonoBehaviour {

    public float m_fAddScale = 0.1f;       //塊のサイズ1に対しての加算値

    float          m_fOldSize;      //前回サイズ保存用
    KatamariStatus m_KatamariSize;  //現在の塊サイズ取得用

	/// <summary>
	/// 初期化
	/// </summary>
	void Start () {
        //塊ステータス取得
        m_KatamariSize = GameObject.Find("Player").GetComponent<KatamariStatus>();
        m_fOldSize = m_KatamariSize.m_Size;
	}
	
    /// <summary>
    /// 更新
    /// </summary>
	void Update () {

        //サイズに変更がないなら更新を行わない
        if (m_fOldSize == m_KatamariSize.m_Size) return;

        //サイズ変更
        Vector3 NewScale = transform.localScale;

        NewScale.x += (m_KatamariSize.m_Size - m_fOldSize) * m_fAddScale;
        NewScale.y = NewScale.x;
        NewScale.z = NewScale.x;

        transform.localScale = NewScale;

        //サイズ保存
        m_fOldSize = m_KatamariSize.m_Size;
	}
}
