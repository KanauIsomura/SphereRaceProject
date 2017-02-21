using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Imageをフェードさせるやつ
public class ImageFade : MonoBehaviour {

    public float m_fFadeTime;   //フェードにかける時間

    private Image m_FadeImage;  //フェードさせるImage
    private float m_fAddAlpha;  //加算アルファ値


    /// <summary>
    /// スタート関数
    /// </summary>
	void Start () 
    {
        //Image取得
        m_FadeImage = GetComponent<Image>();

        //加算値計算
        m_fAddAlpha = -(1.0f / m_fFadeTime);
	}
	
    /// <summary>
    /// 更新処理
    /// </summary>
	void Update () 
    {
	    //色取得
        Color NowColor = m_FadeImage.color;

        //アルファ値加算
        NowColor.a += m_fAddAlpha * Time.deltaTime;

        //現在値で判定
        if (NowColor.a >= 1.0f || NowColor.a <= 0.0f)
            m_fAddAlpha = -m_fAddAlpha;

        //色設定
        m_FadeImage.color = NowColor;
	}
}
