using UnityEngine;
using System.Collections;

public class ScaleStage : MonoBehaviour {

    public Vector3 m_MaxScale;    //最大角度
    public Vector3 m_MinScale;    //最小角度
    public float   m_ScaleTime;   //拡縮にかける時間   


    bool    m_bUse;               //使用判定
    Vector3 m_InitScale;          //初期角度
    Vector3 m_StartScale;         //開始角度
    Vector3 m_EndScale;           //終了角度
    float   m_fStartTime;         //開始時間

    /// <summary>
    /// 初期化
    /// </summary>
	void Start ()
    {
        m_bUse = false;
        m_InitScale = transform.localScale; 
	}
	
    /// <summary>
    /// 更新処理
    /// </summary>
	void Update () 
    {
        //使用中のみ更新
        if (m_bUse == true)
        {
            //経過時間計算
            float NowTime = Time.time - m_fStartTime;

            //終了判定
            if (NowTime > m_ScaleTime)
            {
                //終了サイズに合わせる
                transform.localScale = m_EndScale;


                //現在のサイズから次の設定
                if (transform.localScale == m_MaxScale)
                {//今が最大サイズなら
                    m_fStartTime = Time.time;     //開始時間保存
                    m_StartScale = m_MaxScale;    //開始サイズ設定
                    m_EndScale = m_MinScale;      //終了サイズ設定
                    return;
                }

                if (transform.localScale == m_MinScale)
                {//今が最小サイズなら
                    m_fStartTime = Time.time;     //開始時間保存
                    m_StartScale = m_MinScale;    //開始サイズ設定
                    m_EndScale = m_MaxScale;      //終了サイズ設定
                    return;
                }
            }

            //移動割合
            float rate = NowTime / m_ScaleTime;

            //移動
            transform.localScale = Vector3.Lerp(m_StartScale, m_EndScale, rate);
        }
	}


    /// <summary>
    /// 使用判定設定
    /// </summary>
    /// <param name="bFlg"></param>
    public void SetUse(bool bFlg)
    {
        //同じなら受け付けない
        if (m_bUse == bFlg) return;

        //フラグ更新
        m_bUse = bFlg;

        //設定
        if (m_bUse == true)
        {
            m_fStartTime = Time.time;               //開始時間保存
            m_StartScale = transform.localScale;    //開始サイズ設定
            m_EndScale   = m_MaxScale;              //終了サイズ設定
        }
        else
        {
            //初期サイズに戻す
            transform.localScale = m_InitScale;
        }
    }
}
