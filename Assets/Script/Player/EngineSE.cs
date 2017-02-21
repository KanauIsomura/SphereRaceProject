using UnityEngine;
using System.Collections;

/// <summary>
/// エンジン音鳴らす
/// </summary>
public class EngineSE : MonoBehaviour {

    public float m_StartMinVolume = 0.1f;    //最小音量
    public float m_StartMaxVolume = 0.3f;    //最大音量
    public float m_MinVolume = 0.2f;    //最小音量
    public float m_MaxVolume = 0.4f;    //最大音量

    public float m_MinPitch = 0.5f;     //最小ピッチ
    public float m_MaxPitch = 1.5f;     //最大ピッチ

    //public float m_FadeTime = 2.0f;     //フェードにかける時間

    private PlayerMove  m_PlayerSpeed;   //プレイヤーの速度取得用
    private bool        m_bPlay;         //再生判別用
    private AudioSource m_AudioData;
    private StartProduction m_StartFlg;    //スタート判定用

    private PauseCanvas m_PauseFlg;      //ポーズ判定
    private bool        m_OldFlg;        //前回のフラグ
    private float m_SaveVolume;      //音量保存
    private float m_NowMinVolume;    //現在の最小音量
    private float m_NowMaxVolume;    //現在の最大音量

    /// <summary>
    /// スタート関数
    /// </summary>
    void Start ()
    {
        m_StartFlg = GameObject.Find("StartProduction").GetComponent<StartProduction>();
        m_PauseFlg = GameObject.Find("PauseCanvas").GetComponent<PauseCanvas>();
        m_AudioData = GetComponent<AudioSource>();
        m_PlayerSpeed = GetComponent<PlayerMove>();
        m_bPlay = true;
        m_OldFlg = false;

        //エンジン音再生
        m_AudioData.volume = m_MinVolume;
        m_AudioData.pitch = m_MinPitch;
        m_AudioData.Play();

        //最大速度設定
        m_NowMinVolume = m_StartMinVolume;
        m_NowMaxVolume = m_StartMaxVolume;
	}
	
    /// <summary>
    /// 更新処理
    /// </summary>
	void Update ()
    {
        //ポーズ中は音量を下げる
        if (m_OldFlg == false && m_PauseFlg.isPausing == true)
        {//ポーズ時開始時
            m_SaveVolume = m_AudioData.volume;
            m_AudioData.volume = 0.0f;

            //フラグ保存
            m_OldFlg = m_PauseFlg.isPausing;
            return;
        }

        if (m_OldFlg == true && m_PauseFlg.isPausing == false)
        {//ポーズ時終了時
            m_AudioData.volume = m_SaveVolume;

            //フラグ保存
            m_OldFlg = m_PauseFlg.isPausing;
        }



        //ポーズ中なら処理を行わない
        if (m_PauseFlg.isPausing == true) return;

        //最大音量設定
        if (m_StartFlg.isStart == false)
        {//開始前
            m_NowMinVolume = m_StartMinVolume;
            m_NowMaxVolume = m_StartMaxVolume;
        }
        else
        {//開始後
            m_NowMinVolume = m_MinVolume;
            m_NowMaxVolume = m_MaxVolume;
        }


        //スティック入力値を取得
        float fVertical = MultiInput.Instance.GetLeftStickAxis().y;    //上下
        float fHorizontal = MultiInput.Instance.GetLeftStickAxis().x;  //左右

        //スティックの入力があるなら音を出す
        if (fVertical != 0 || fHorizontal != 0)
        {//再生
            //音量をあげていく
            m_AudioData.volume += m_NowMaxVolume / 10.0f * Time.deltaTime;
            if (m_AudioData.volume > m_NowMaxVolume)
                m_AudioData.volume = m_NowMaxVolume;

            //ピッチをあげていく
            m_AudioData.pitch += m_MaxPitch / 10.0f * Time.deltaTime;
            if (m_AudioData.pitch > m_MaxPitch)
                m_AudioData.pitch = m_MaxPitch;
        }
        else
        {//停止
            //音量をさげていく
            m_AudioData.volume -= m_NowMaxVolume / 10.0f * Time.deltaTime;
            if (m_AudioData.volume < m_NowMinVolume)
                m_AudioData.volume = m_NowMinVolume;

            //ピッチをさげていく
            m_AudioData.pitch -= m_MinPitch / 2.5f * Time.deltaTime;
            if (m_AudioData.pitch < m_MinPitch)
                m_AudioData.pitch = m_MinPitch;
        }
	}

    void SetSE(bool bFlg)
    {
        //変更判別
        if (m_bPlay == bFlg) return;

        //フラグ更新
        m_bPlay = bFlg;

        //再生or停止
        if (m_bPlay == true)
        {
            m_AudioData.Play();
        }
        else
        {
            if (m_AudioData.volume == 0.0f)
                m_AudioData.Stop();
        }
    }
}
