using UnityEngine;
using System.Collections;

/// <summary>
/// ステージセレクト時のプレイヤーの移動
/// </summary>
public class StageSelectPlayerMove : MonoBehaviour {

    public Transform[] m_StagePos;              //ステージの座標データ
    public int         m_nStartStage = 0;       //初期選択ステージ
    public float       m_fStickValue = 0.6f;    //スティック判定値   
    public float       m_fMoveTime   = 1.5f;    //移動にかける時間
    public float       m_fRotationSpeed = 2;    //決定後の回転速度

    private Vector3    m_StartRot;      //開始位置          
    private Vector3    m_EndRot;        //目標位置
    private float      m_fStartTime;    //移動開始時間  
    private bool       m_bSelect;       //選択判定
    private bool       m_bMove;         //移動判定
    private int        m_nNowSelect;    //現在選択しているステージ

    //取得用変数
    public int  NowSelect { get { return m_nNowSelect; } }
    public bool SelectFlg { get { return m_bSelect; } }
    public bool MoveFlg   { get { return m_bMove; } }

    //フェード判別用
    Fade Fade;


    /// <summary>
    /// 初期設定
    /// </summary>
	void Start()
    {
        m_bMove = false;                    //移動判定OFF
        m_bSelect = false;                  //選択判定OFF
        m_nNowSelect = m_nStartStage;       //初期選択位置設定

        //初期位置を設定
        Vector3 Rot = transform.eulerAngles;
        Rot.z = m_StagePos[m_nNowSelect].eulerAngles.z;
        transform.eulerAngles = Rot;

        Fade = GameObject.Find("FadeCanvas").GetComponent<Fade>();
		SoundManager.Instance.PlayBGM("loop_89");
	}

	// Update is called once per frame
	void Update () 
    {
        //スティックの入力値取得
        Vector2 LeftInput = MultiInput.Instance.GetLeftStickAxis();

        //選択済みでステージオブジェクトに到達後
        if (m_bSelect == true && m_bMove == false)
        {
            transform.Rotate(Vector3.right, m_fRotationSpeed * Time.deltaTime);
            return;
        }

        //フェード中は動かさない
        if (Fade.GetFading() == true && m_bSelect == false) return;


        //決定判定
        if ( LeftInput.y >= m_fStickValue && m_bSelect == false && m_bMove == false)
        {
            //移動準備
            m_StartRot = transform.eulerAngles;
            m_StartRot.x = SetAngle(m_StartRot.x);

            m_EndRot = transform.eulerAngles;
            m_EndRot.x = m_StagePos[m_nNowSelect].eulerAngles.x;
            m_EndRot.x = SetAngle(m_EndRot.x);

            m_fStartTime = Time.time;   //開始時間保存
            
            m_bMove = true;             //移動フラグON
            m_bSelect = true;           //選択済みフラグON
        }

        //左右選択
        if (Mathf.Abs(LeftInput.x) >= m_fStickValue && m_bMove == false)
        {
            if (LeftInput.x > 0)
            {//右
                m_nNowSelect++;
                if (m_nNowSelect > m_StagePos.Length - 1)
                {
                    m_nNowSelect = m_StagePos.Length - 1;
                    return;
                }

                //移動準備
                SetMove();
            }
            else
            {//左
                m_nNowSelect--;
                if (m_nNowSelect < 0)
                {
                    m_nNowSelect = 0;
                    return;
                }

                //移動準備
                SetMove();
            }
        }


        //移動処理
        MoveAngle();
	}


    /// <summary>
    /// 移動設定
    /// </summary>
    void SetMove()
    {
        //スタート角度設定
        m_StartRot = transform.eulerAngles;
        m_StartRot.z = SetAngle(m_StartRot.z);

        //終了角度設定
        m_EndRot = transform.eulerAngles;
        m_EndRot.z = m_StagePos[m_nNowSelect].eulerAngles.z;
        m_EndRot.z = SetAngle(m_EndRot.z);

        //開始時間保存
        m_fStartTime = Time.time;

        //移動フラグON
        m_bMove = true;            
    }


    /// <summary>
    /// 角度調整
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    float SetAngle(float angle)
    {
        if (angle > 180)
            angle -= 360;
        return angle;
    }

    /// <summary>
    /// 角度移動
    /// </summary>
    void MoveAngle()
    {
        //移動処理
        if (m_bMove == true)
        {
            //経過時間計算
            float NowTime = Time.time - m_fStartTime;

            //終了判定
            if (NowTime > m_fMoveTime)
            {
                transform.eulerAngles = m_EndRot;
                m_bMove = false;
                
                //決定済みではなかったらSEを鳴らす
                if (m_bSelect == false)
                    SoundManager.Instance.PlaySE("StageSelect");
            }

            //移動割合
            float rate = NowTime / m_fMoveTime;

            //移動
            transform.eulerAngles = Vector3.Lerp(m_StartRot, m_EndRot, rate);
        }
    }
}
