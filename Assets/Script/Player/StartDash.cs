using UnityEngine;
using System.Collections;

/// <summary>
/// スタートダッシュ
/// </summary>
public class StartDash : MonoBehaviour {

    public float fStartDashCount = 10.0f;    //何回入力すればスタートダッシュするか
    public float fAddDashCount = 2.0f;       //加算カウント
    public float fDecDashCount = 1.0f;       //減算カウント
    public float fNextFrameCount = 0.3f;     //次の入力までの猶予
    public float fStickValue     = 0.5f;     //判定するスティックの値(0～1)
    public float fStartDashSpeed = 15.0f;    //スタートダッシュの速度


    float fStartFrame = 0;                //入力開始時のフレーム保存用
    float fNowDashCount = 0;              //現在のスタートダッシュカウント
    public float NowDashCount {get{return fNowDashCount;}}
    bool bFirst = true;                   //初回判定用
    bool bInput = false;                  //前の入力があったか
    float fOldLeftValue = 0;              //前回の左の入力値
    float fOldRightValue = 0;             //前回の右の入力値
    PlayerMove PlayerSpeed; 
    bool bStartDashFlg = false;           //スタートダッシュ判定
    public bool StartDashFlg { get { return bStartDashFlg; } }

    private StartProduction StartFlg;               //スタート判定用
    private Transform Katamari;                     //塊回転用
    private bool StartParticle = false;             //パーティクル開始判定     
    public ParticleSystem SmokeParticle;            //煙パーティクル             

	// Use this for initialization
	void Start () {
        //取得
        StartFlg = GameObject.Find("StartProduction").GetComponent<StartProduction>();
        PlayerSpeed = GameObject.Find("Player").GetComponent<PlayerMove>();
        Katamari = GameObject.Find("katamari").gameObject.transform;
        SmokeParticle.Stop();

	}
	
	// Update is called once per frame
	void Update () {
        
        //カウントダウンが終了したら
        if (StartFlg.isStart == true)
        {
            //スタートダッシュカウント
            if (fNowDashCount >= fStartDashCount)
            {
                PlayerSpeed.VerticalSpeed = fStartDashSpeed;

                GameObject.Find("BoosterEffects").GetComponent<BoosterEffect>().StartEffect();

                //成功判定
                bStartDashFlg = true;
            }
            else
            {
                PlayerSpeed.VerticalSpeed = 0.0f;
            }



            //スクリプトを切る
            enabled = false;

            //パーティクル消す
            SmokeParticle.Stop();


            return;
        }


        ////////////////////////////////////
        //スタートダッシュ準備
        ////////////////////////////////////

        //スティック入力取得
        float fLeftInput = MultiInput.Instance.GetLeftStickAxis().y;     //左上下
        float fRightInput = MultiInput.Instance.GetRightStickAxis().y;   //右上下


        //カウント減算
        fNowDashCount -= fDecDashCount * Time.deltaTime;
        if (fNowDashCount <= 0.0f)
        {
            if (StartParticle == true)
            {
                StartParticle = false;
                SmokeParticle.Stop();
            }
            fNowDashCount = 0.0f;
        }


        //初回時のみ前入力設定処理
        if (bFirst == true)
        {
            fOldLeftValue = -Mathf.Sign(fLeftInput);
            fOldRightValue = -Mathf.Sign(fRightInput);
        }

        //１回目判定
        if (bInput == false &&                                    //一回目判定
            Mathf.Abs(fLeftInput) >= fStickValue &&               //一定入力値以上
            Mathf.Abs(fRightInput) >= fStickValue &&              //一定入力値以上
            Mathf.Sign(fLeftInput) != Mathf.Sign(fRightInput) &&  //上下入力判定
            fOldLeftValue != Mathf.Sign(fLeftInput) &&            //前回の入力と逆入力
            fOldRightValue != Mathf.Sign(fRightInput))            //前回の入力と逆入力
        {
            bFirst = false;                             //初回判定OFF
            bInput = true;                              //一回目入力判定ON
            fOldLeftValue = Mathf.Sign(fLeftInput);     //入力保存
            fOldRightValue = Mathf.Sign(fRightInput);   //入力保存
            fStartFrame = Time.time;                    //入力開始時間保存
            return;
        }

        //2回目
        if (bInput == true &&
            Mathf.Abs(fLeftInput) >= fStickValue &&    //一定入力値以上
            Mathf.Abs(fRightInput) >= fStickValue &&   //一定入力値以上
            fOldLeftValue != Mathf.Sign(fLeftInput) && //前回の入力と逆入力
            fOldRightValue != Mathf.Sign(fRightInput)) //前回の入力と逆入力
        {
            //入力成功
            bInput = false;                                     //入力判定OFF
            fNowDashCount += fAddDashCount; //* Time.deltaTime; //カウント加算
            if (fNowDashCount > fStartDashCount + fAddDashCount)
                fNowDashCount = fStartDashCount + fAddDashCount;

            fOldLeftValue = Mathf.Sign(fLeftInput);             //入力保存
            fOldRightValue = Mathf.Sign(fRightInput);           //入力保存
            if (StartParticle == false)
            {
                StartParticle = true;
                SmokeParticle.Play();
            }
        }
        else
        {
            //入力受付時間が過ぎていないか
            if ((Time.time - fStartFrame) >= fNextFrameCount)
            {//入力失敗
                bInput = false;
            }
        }

        ////////////////////////////////////
        // 速度を送る
        ////////////////////////////////////
        PlayerSpeed.VerticalSpeed = fNowDashCount * (PlayerSpeed.PlayerMaxSpeed / fStartDashCount);
        //Debug.Log(fNowDashCount);

        ////////////////////////////////////
        // 塊を回転させる
        ////////////////////////////////////
        //Debug.Log(fNowDashCount * 100 * Time.deltaTime);
        Katamari.Rotate(new Vector3(fNowDashCount * 100 * Time.deltaTime, 0, 0));
	}
}
