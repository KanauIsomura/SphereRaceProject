using UnityEngine;
using System.Collections;

public class PlayerWeightMove : MonoBehaviour {

    //体重移動での移動値
    public float fWeightMaxSpeed = 5.0f;       //体重移動での最大速度
    public float fWeightAddSpeed = 1.5f;       //加速値
    public float fWeightDecSpeed = 1.0f;       //減速値
    public float fWeightReverseSpeed = 1.0f;   //反転速度


    private float fWeightSpeed;                //体重移動の速度
    private StartProduction StartFlg;   //スタート判定用

    //取得用変数
    public float WeightMaxSpeed { get { return fWeightMaxSpeed; } }
    public float WeightSpeed    { get { return fWeightSpeed; } }


	// Use this for initialization
    void Start()
    {
        StartFlg = GameObject.Find("StartProduction").GetComponent<StartProduction>();
	}
	
	// Update is called once per frame
    void Update()
    {
        //===============================
        // スタート判定
        //===============================
        if (StartFlg.isStart == false) return;


        //===============================
        // スティックの入力値保存
        //===============================
        //左スティック入力保存
        float fVertical = MultiInput.Instance.GetLeftStickAxis().y;      //上下

        //右スティック入力保存
        float fRightVertical = MultiInput.Instance.GetRightStickAxis().y;   //上下

        //===============================
        // 最大速度設定
        //===============================


        //===============================
        // 体重移動
        //===============================
        if (fRightVertical != 0 && fVertical != 0)
        {
            fWeightSpeed = Move(fWeightSpeed, fRightVertical);
        }

        //減速
        fWeightSpeed = Deceleration(fWeightSpeed);
    }



    /// <summary>
    /// 移動速度計算処理
    /// </summary>
    /// <param name="NowSpeed">   現在の速度 </param>
    /// <param name="StickValue"> スティックの入力値 </param>
    /// <returns>計算後の速度</returns>
    float Move(float NowSpeed, float StickValue)
    {
        //速度加算
        NowSpeed += StickValue * fWeightAddSpeed * Time.deltaTime;

        //逆入力の場合
        if (Mathf.Sign(NowSpeed) != Mathf.Sign(StickValue))
        {
            //速度加算
            NowSpeed += StickValue * fWeightReverseSpeed * Time.deltaTime;
        }

        //最大速度制限
        if (Mathf.Abs(NowSpeed) > fWeightMaxSpeed)
        {
            //減速処理
            NowSpeed = Deceleration(NowSpeed);
        }

        return NowSpeed;
    }


    /// <summary>
    /// 減速処理
    /// </summary>
    /// <param name="NowSpeed"> 現在の速度 </param>
    /// <returns>計算後の速度</returns>
    float Deceleration(float NowSpeed)
    {
        //符号保存
        float Sign = Mathf.Sign(NowSpeed);

        //減速
        NowSpeed = Mathf.Abs(NowSpeed) - fWeightDecSpeed * Time.deltaTime;

        //0以下にならないように制限
        if (NowSpeed < 0)
            NowSpeed = 0;

        //符号を戻す
        NowSpeed *= Sign;

        return NowSpeed;
    }

    /// <summary>
    /// 減衰設定
    /// </summary>
    /// <param name="AttenuationValue"></param>
    public void Attenuation(float AttenuationValue)
    {
        fWeightSpeed = fWeightSpeed * AttenuationValue;
    }
}
