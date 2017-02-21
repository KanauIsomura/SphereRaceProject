using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// タイトル管理したいクラス
/// </summary>
public class TitleController : MonoBehaviour {

    public string       m_NextScene;     //次シーン
    public SceneChanger m_SceneChanger;  //シーン変更用
    public GameObject   m_TitleObject;   //非表示にしておくオブジェクト
    public TitleStartPlayer  m_StartFlg;
    public Cloth        m_ClothObj;
    public float        m_fFadeTime;   //フェードにかける時間
    public RawImage     m_PushImage;   //プッシュイメージ
    public float        m_DrawTime;    //表示までの時間
    public float        m_BGMInTime;   //BGMのフェードインの時間

    private float       m_fAddAlpha;
    private float       m_fStartTime;
    private AudioSource m_TitleBGM;    //BGMフェード再生用

    /// <summary>
    /// スタート関数
    /// </summary>
	void Start () {
		//カーソルを非表示に
		Cursor.visible = false;

	    //非表示にする
        m_TitleObject.SetActive(false);
        m_ClothObj.enabled = false;

        //加算値設定
        m_fAddAlpha = 1.0f / m_fFadeTime;

        m_fStartTime = -1;

        //アルファ値0に
        Color color = m_PushImage.color;
        color.a = 0.0f;
        m_PushImage.color = color;

        //AudioSource取得
        m_TitleBGM = GetComponent<AudioSource>();
        m_TitleBGM.volume = 0;
	}
	

    /// <summary>
    /// 更新処理
    /// </summary>
	void Update () 
    {
        //開始
        if (m_StartFlg.StartFlg == true)
        {
            //旗オブジェクト表示
            m_TitleObject.SetActive(true);
            m_ClothObj.enabled = true;

            //開始時間設定
            if (m_fStartTime == -1)
            {
                m_fStartTime = Time.time;
                m_TitleBGM.Play();  //BGM再生
            }

            //音をフェードさせていく
            if ((Time.time - m_fStartTime) <= m_BGMInTime)
            {
                float Rate = (Time.time - m_fStartTime) / m_BGMInTime;
                m_TitleBGM.volume = Mathf.Lerp(0.0f, 1.0f, Rate);
            }

            //文字をフェードインさせていく
            if ((Time.time - m_fStartTime) >= m_DrawTime && m_PushImage.color.a <= 1.0f)
            {
                Color color = m_PushImage.color;
                color.a += m_fAddAlpha;
                m_PushImage.color = color;
            }

            //シーン変更判定
            if (MultiInput.Instance.GetPressButton(MultiInput.CONTROLLER_BUTTON.CIRCLE) == true)
            {
                if( m_SceneChanger.SetNextScene(m_NextScene) == true)
                    SoundManager.Instance.PlaySE("select");
            }
        }
	}
}
