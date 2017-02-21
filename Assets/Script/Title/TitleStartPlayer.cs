using UnityEngine;
using System.Collections;

public class TitleStartPlayer : MonoBehaviour {

    public ParticleSystem m_StartSmoke;
    public ParticleSystem m_Smoke;
    public  float    m_nMoveInterval = 5;
    public Animator  m_UFOAnim;

    private bool     m_bMove;
    private float    m_nEndTime;
    private bool     m_bStart;
    private Animator m_Anim;
    private int      m_nWait;
    private int      m_nStartMove;
    private int      m_nPlayerMove;
    private bool m_bPlay;
    AudioSource m_Audio;

    //取得用変数
    public bool StartFlg { get { return m_bStart; } }

    /// <summary>
    /// スタート
    /// </summary>
	void Start () 
    {
        //アニメーター取得
        m_Anim = GetComponent<Animator>();

        //ハッシュ取得
        m_nWait = Animator.StringToHash("Base Layer.Wait");
        m_nStartMove = Animator.StringToHash("Base Layer.TitleCarMove");
        //m_nPlayerMove = Animator.StringToHash("Base Layer.");

        m_bStart = false;
        m_bMove = false;
        m_bPlay = false;

        m_StartSmoke.Play();
        m_Smoke.Stop();

        m_Audio = GetComponent<AudioSource>();
	}
	
	/// <summary>
	/// 更新処理
	/// </summary>
	void Update () 
    {
        //アニメーション情報取得
        AnimatorStateInfo anim = m_Anim.GetCurrentAnimatorStateInfo(0);

        if (anim.fullPathHash == m_nStartMove && anim.normalizedTime > 0.3f && m_bPlay == false)
        {
            m_bPlay = true;
            m_Audio.Play();
        }

        //終了判定
        if (anim.fullPathHash == m_nWait && m_bStart == false)
        {
            m_nEndTime = Time.time;
            m_bStart = true;
            m_StartSmoke.Stop();
            m_Audio.Stop();
            m_Smoke.Play();
        }

        //終了していないなら更新をしない
        if (m_bStart == false) return;

        //更新
        if (anim.fullPathHash == m_nWait)
        {
            if (Time.time - m_nEndTime > m_nMoveInterval && m_bMove == false)
            {
                m_bMove = true;
                m_Audio.Play();
                m_Anim.SetTrigger("PlayerMove");
                m_UFOAnim.SetTrigger("Start");
            }
        }
        else
        {
            m_bMove = false;
            m_nEndTime = Time.time;
        }
    }
}

