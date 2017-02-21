using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleFadeImage : MonoBehaviour {

    // フェード遷移列挙
    public enum FadeState
    {
        None = 0,
        FADE_IN,    
        FADE_OUT,
        MAX_FADE
    };

    [Header("現在のフェード")]
    public FadeState m_Fade;              // 現在のフェード(初期値はインスペクターで決めて)

    [Header("フェードの速さ")]
    public float m_fFadeSpeed = 0.1f; 

    [Header("フェードをループさせるか否か")]
    public bool m_bLoop = false;   

    [Header("フェードの最大値")]
    private float m_fMaxFade = 1.0f;

    [Header("フェードの最小値")]
    private float m_fMinFade = 0.0f;
    
    private Image m_FadeImage;            // フェードする画像
    private int m_nAlpha;                 // 初期のアルファ

	// Use this for initialization
	void Start () {
        m_FadeImage = transform.GetComponent<Image>();    // Imageを取得
	}
	
	// Update is called once per frame
	void Update () {
        FadeUpdate();
    }

    // フェード更新処理
    void FadeUpdate()
    {
        switch (m_Fade)   // 現在のフェード
        {
            case FadeState.None:    // フェード無し
                break;

            case FadeState.FADE_OUT: // フェードアウト
                if (m_FadeImage.color.a > m_fMinFade)
                    m_FadeImage.color -= new Color(0, 0, 0, m_fFadeSpeed);
                else
                {
                    m_FadeImage.color = new Color(1.0f, 1.0f, 1.0f, m_fMinFade); // アルファを最低値に指定
                    m_Fade = FadeState.FADE_IN;   // フェードインに移行
                }
                break;

            case FadeState.FADE_IN:    // フェードイン
                if (m_FadeImage.color.a < m_fMaxFade)
                {
                    m_FadeImage.color += new Color(0, 0, 0, m_fFadeSpeed);
                }
                else if (m_bLoop)
                {
                    m_Fade = FadeState.FADE_OUT;   // フェードアウトに移行
                }
                else if (!m_bLoop)
                {
                    m_FadeImage.color = new Color(1.0f, 1.0f, 1.0f, m_fMaxFade);
                    m_Fade = FadeState.None;
                }
                break;
        }
    }

    // フェードをセット
    public void SetFade(FadeState state)
    {
        m_Fade = state;
    }
}
