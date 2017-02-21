using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーの移動クラス
/// </summary>
public class PlayerMove : MonoBehaviour {

    //プレイヤーの移動値
    public float m_fMaxSpeed = 15.0f;     //塊の最大速度
    public float m_fAddSpeed = 5.0f;      //加速値
    public float m_fDecSpeed = 3.5f;      //減速値
    public float m_fReverseSpeed = 1.0f;  //反転速度
    public float m_fDashDecSpeed = 5.0f;  //減速値

    private float m_fNowMaxSpeed;         //現在の最大速度
    private float m_fVerticalSpeed;       //縦の速度
    private float m_fSideSpeed;           //横の速度
    private float m_fNowSpeed;            //現在の速度
    private float m_fDashSpeed;           //ダッシュ速度

    private float m_fBoundSpeed;          //バウンド時の速度
    private Vector3 m_vBoundVec;          //バウンド方向


    //角度制限用変数
    public float m_SlopeMin = 45.0f;        //登れる最低の角度
    public float m_SlopeMax = 60.0f;        //登れる最大の角度
    public float m_fAddMaxSpeed = 10.0f;    //足し込む速度
    private float m_fSlopLimit;             //現在の登れる坂
    private KatamariStatus KatamariSize;
    private float m_fInitSize;              //初期サイズ

    //Get・Set用変数
    public float PlayerMaxSpeed { get { return m_fNowMaxSpeed; } }
    public float PlayerSpeed    { get { return m_fNowSpeed; } }
    public float VerticalSpeed  { get { return m_fVerticalSpeed; } set { m_fVerticalSpeed = value; } }
    public float SideSpeed      { get { return m_fSideSpeed; }     set { m_fSideSpeed = value; } }
    public float DashSpeed      { get { return m_fDashSpeed; }     set { m_fDashSpeed = value; } }
    public float SlopeLimit     { get { return m_fSlopLimit;} }
    public float BoundSpeed     { get { return m_fBoundSpeed;} }



    private CharacterController CharaCon;
    private StartProduction StartFlg;    //スタート判定用
    private PlayerWeightMove WeightMove; //体重移動分取得用
    private Vector3 vFrom;               //進行方向
    public Vector3 PlayerDirection { get { return vFrom; } }

    /// <summary>
    /// スタート関数
    /// </summary>
    void Start () {
        //取得
		//StartFlg = GameObject.Find("StartProduction").GetComponent<StartProduction>();
        CharaCon = GetComponent<CharacterController>();
        WeightMove = GetComponent<PlayerWeightMove>();
        KatamariSize = GetComponent<KatamariStatus>();
        m_fInitSize = KatamariSize.m_Size;
        m_fNowMaxSpeed = m_fMaxSpeed;
        m_fSlopLimit = m_SlopeMin;  //初期値設定
    }
	
    /// <summary>
    /// アップデート関数
    /// </summary>
	void Update ()
    {
        //===============================
        // 速度保存
        //===============================
        if (Mathf.Abs(m_fSideSpeed) > Mathf.Abs(m_fVerticalSpeed) + Mathf.Abs(WeightMove.WeightSpeed) + Mathf.Abs(m_fDashSpeed))
            m_fNowSpeed = m_fSideSpeed;
        else
            m_fNowSpeed = m_fVerticalSpeed + WeightMove.WeightSpeed + m_fDashSpeed;


        //===============================
        // スタート判定
        //===============================
        if (StartFlg.isStart == false) return;


        //===============================
        // スティックの入力値保存
        //===============================
        //左スティック入力保存
        float fVertical = MultiInput.Instance.GetLeftStickAxis().y;      //上下
        float fHorizontal = MultiInput.Instance.GetLeftStickAxis().x;  //左右

        
        //===============================
        // 最大速度設定
        //===============================
        m_fNowMaxSpeed = (KatamariSize.m_Size - m_fInitSize) * m_fAddMaxSpeed + m_fMaxSpeed + WeightMove.WeightMaxSpeed;


        //===============================
        // 移動速度計算
        //===============================
        m_fVerticalSpeed = Move(m_fVerticalSpeed, fVertical);
        m_fSideSpeed = Move(m_fSideSpeed, fHorizontal);


        //===============================
        // 減速
        //===============================
        m_fVerticalSpeed = Deceleration(m_fVerticalSpeed, m_fDecSpeed);
        m_fSideSpeed = Deceleration(m_fSideSpeed, m_fDecSpeed);
        m_fDashSpeed = Deceleration(m_fDashSpeed, m_fDashDecSpeed);


        //===============================
        // 登れる角度設定
        //===============================
        m_fSlopLimit = Mathf.Ceil(m_fNowSpeed * (m_SlopeMax / m_fMaxSpeed));
        if (m_fSlopLimit < m_SlopeMin)
            m_fSlopLimit = m_SlopeMin;
        if (m_fSlopLimit > m_SlopeMax)
            m_fSlopLimit = m_SlopeMax;


        //===============================
        // バウンド分移動させる
        //===============================
        if (m_fBoundSpeed != 0)
        {
            //減速させる
            m_fBoundSpeed = Deceleration(m_fBoundSpeed, m_fDecSpeed);
        }


        //===============================
        // 移動させる
        //===============================
        //移動量設定
        Vector3 vMove = Vector3.zero;
        float fWeightSpeed = WeightMove.WeightSpeed;

        //斜め移動判定
        if (m_fVerticalSpeed != 0 && m_fSideSpeed != 0)
        {//斜め移動時
            float Speed = Mathf.Sqrt(2);
            vMove += transform.right * (m_fSideSpeed / Speed);
            vMove += transform.forward * ((m_fVerticalSpeed + fWeightSpeed + m_fDashSpeed) / Speed);
        }
        else
        {//斜め移動じゃない場合
            vMove += transform.right * m_fSideSpeed;
            vMove += transform.forward * (m_fVerticalSpeed + fWeightSpeed + m_fDashSpeed);
        }


        //===============================
        // 移動
        //===============================
        Vector3 NewPos = transform.position + vMove * Time.deltaTime;
        //vFrom = Vector3.Normalize(NewPos - transform.position);
        vFrom = Vector3.Normalize(vMove);

        //移動実行
        CharaCon.Move((m_vBoundVec * m_fBoundSpeed) * Time.deltaTime);
        CharaCon.Move(vMove * Time.deltaTime);
    }


    /// <summary>
    /// バウンド用設定
    /// </summary>
    /// <param name="Vec"></param>
    /// <param name="Speed"></param>
    public void BoundSet(Vector3 Vec, float Speed, float fSideResistanceValue, float fVerticalResistanceValue)
    {
        //変数設定
        m_fBoundSpeed = Mathf.Abs(Speed);
        m_vBoundVec = Vec;


        //Debug.Log("Side前" + m_fSideSpeed);
        //Debug.Log("Vertical前" + m_fVerticalSpeed);

        //抵抗値で速度減少
        m_fSideSpeed = m_fSideSpeed * fSideResistanceValue;
        m_fVerticalSpeed = m_fVerticalSpeed * fVerticalResistanceValue;
        WeightMove.Attenuation(fVerticalResistanceValue);

        //Debug.Log("Side" + m_fSideSpeed);
        //Debug.Log("Vertical" + m_fVerticalSpeed);
        //Debug.Log("WeightSpeed" + WeightMove.WeightSpeed);

    }


    /// <summary>
    /// 移動速度計算処理
    /// </summary>
    /// <param name="NowSpeed">   現在の速度 </param>
    /// <param name="StickValue"> スティックの入力値 </param>
    /// <returns>計算後の速度</returns>
    float Move(float NowSpeed,float StickValue)
    {
        //速度加算
        NowSpeed += StickValue * m_fAddSpeed * Time.deltaTime;

        //逆入力の場合
        if (Mathf.Sign(NowSpeed) != Mathf.Sign(StickValue))
        {
            //速度加算
            NowSpeed += StickValue * m_fReverseSpeed * Time.deltaTime;
        }

        //最大速度制限
        if (Mathf.Abs(NowSpeed) > m_fNowMaxSpeed - WeightMove.WeightMaxSpeed)
        {
            //減速処理
            NowSpeed = Deceleration(NowSpeed,m_fDecSpeed);
        }

        return NowSpeed;
    }


    /// <summary>
    /// 減速処理
    /// </summary>
    /// <param name="NowSpeed"> 現在の速度 </param>
    /// <param name="DecSpeed"> 減速速度</param>
    /// <returns>計算後の速度</returns>
    float Deceleration(float NowSpeed, float DecSpeed)
    {
        //符号保存
        float Sign = Mathf.Sign(NowSpeed);

        //減速
        NowSpeed = Mathf.Abs(NowSpeed) - DecSpeed * Time.deltaTime;

        //0以下にならないように制限
        if (NowSpeed < 0)
            NowSpeed = 0;

        //符号を戻す
        NowSpeed *= Sign;

        return NowSpeed;
    }
}
