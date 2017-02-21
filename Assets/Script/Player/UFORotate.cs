using UnityEngine;
using System.Collections;

/// <summary>
/// UFO回転
/// </summary>
public class UFORotate : MonoBehaviour {

    public float fMaxAngle = 60.0f;     //最大角度
    public float fDecAngle = 60.0f;     //戻す角度
    public float fEasing = 2.0f;        //移動割合
    public float fStartDashTime = 2.0f; //スタートダッシュの時間
    bool bStartDash = true;
    float fStartTime;
    StartDash DashFlg;

    Vector3 vInitAngle;             //初期角度
    Vector3 vNowAngle;              //現在の角度

    private StartProduction StartFlg;   //スタート判定用

    // Use this for initialization
    void Start()
    {
        //取得
        StartFlg = GameObject.Find("StartProduction").GetComponent<StartProduction>();
        DashFlg = GameObject.Find("Player").GetComponent<StartDash>();
        //初期角度保存
        vInitAngle = transform.localEulerAngles;
	}
	
	// Update is called once per frame
    void Update()
    {
        //スティック入力保存
        float fVertical = MultiInput.Instance.GetRightStickAxis().y;    //上下
        float fHorizontal = MultiInput.Instance.GetRightStickAxis().x;  //左右

        //初期角度に戻す
        transform.localEulerAngles = vInitAngle;

        //角度加算
        
        //カウントダウンちゅうなら
        if (StartFlg.isStart == false)
        {
            if (DashFlg.NowDashCount != 0)
            {
                //現在の角度と最大角度の差分から移動量計算
                vNowAngle.x += (fMaxAngle - vNowAngle.x) * fEasing * Time.deltaTime;

                //最大角度判定
                if (vNowAngle.x > fMaxAngle) vNowAngle.x = fMaxAngle;
            }
            else
            {//入力が無かった場合
                if (vNowAngle.x > 0)
                {
                    vNowAngle.x -= fDecAngle * 0.5f * Time.deltaTime;
                    if (vNowAngle.x < 0)
                        vNowAngle.x = 0;
                }
            }

            //角度反映
            transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x + vNowAngle.x,
                                                transform.rotation.eulerAngles.y,
                                                transform.rotation.eulerAngles.z + vNowAngle.z * -1);

            //時間保存
            fStartTime = Time.time;

            return;
        }

        //スタートダッシュ中
        if (bStartDash == true)
        {
            if (fVertical != 0)
            {
                //現在の角度と最大角度の差分から移動量計算
                vNowAngle.x += (fMaxAngle - vNowAngle.x) * fEasing * Time.deltaTime;

                //最大角度判定
                if (vNowAngle.x > fMaxAngle) vNowAngle.x = fMaxAngle;
            }
            else
            {//入力が無かった場合
                if (vNowAngle.x > 0)
                {
                    vNowAngle.x -= fDecAngle * 0.5f * Time.deltaTime;
                    if (vNowAngle.x < 0)
                        vNowAngle.x = 0;
                }
            }

            //左右
            if (fHorizontal != 0)
            {
                //現在の角度と最大角度の差分から移動量計算
                vNowAngle.z += (((fMaxAngle / 2) * Mathf.Sign(fHorizontal)) - vNowAngle.z) * fEasing * Time.deltaTime;

                //最大角度判定
                if (vNowAngle.z > (fMaxAngle / 2)) vNowAngle.z = fMaxAngle;
                if (vNowAngle.z < -(fMaxAngle / 2)) vNowAngle.z = -fMaxAngle;
            }
            else
            {//入力が無かった場合

                if (vNowAngle.z > 0)
                {
                    vNowAngle.z -= fDecAngle * Time.deltaTime;
                    if (vNowAngle.z < 0)
                        vNowAngle.z = 0;
                }
                else if (vNowAngle.z < 0)
                {
                    vNowAngle.z += fDecAngle * Time.deltaTime;
                    if (vNowAngle.z > 0)
                        vNowAngle.z = 0;
                }
            }
            //角度反映
            transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x + vNowAngle.x,
                                                transform.rotation.eulerAngles.y,
                                                transform.rotation.eulerAngles.z + vNowAngle.z * -1);

            //終了判定
            if ((Time.time - fStartTime) >= fStartDashTime || DashFlg.StartDashFlg == false)
            {
                bStartDash = false;
            }

            return;
        }



        //前後
        if (fVertical != 0)
        {
            //現在の角度と最大角度の差分から移動量計算
            vNowAngle.x += ((fMaxAngle * Mathf.Sign(fVertical)) - vNowAngle.x) * fEasing * Time.deltaTime;
            
            //最大角度判定
            if (vNowAngle.x > fMaxAngle) vNowAngle.x = fMaxAngle;
            if (vNowAngle.x < -fMaxAngle) vNowAngle.x = -fMaxAngle; 
        }
        else
        {//入力が無かった場合

            if (vNowAngle.x > 0)
            {
                vNowAngle.x -= fDecAngle * Time.deltaTime;
                if (vNowAngle.x < 0)
                    vNowAngle.x = 0;
            }
            else if (vNowAngle.x < 0)
            {
                vNowAngle.x += fDecAngle * Time.deltaTime;
                if (vNowAngle.x > 0)
                    vNowAngle.x = 0;
            }
        }

        //左右
        if (fHorizontal != 0)
        {
            //現在の角度と最大角度の差分から移動量計算
            vNowAngle.z += ((fMaxAngle * Mathf.Sign(fHorizontal)) - vNowAngle.z) * fEasing * Time.deltaTime;

            //最大角度判定
            if (vNowAngle.z > fMaxAngle) vNowAngle.z = fMaxAngle;
            if (vNowAngle.z < -fMaxAngle) vNowAngle.z = -fMaxAngle;
        }
        else
        {//入力が無かった場合

            if (vNowAngle.z > 0)
            {
                vNowAngle.z -= fDecAngle * Time.deltaTime;
                if (vNowAngle.z < 0)
                    vNowAngle.z = 0;
            }
            else if (vNowAngle.z < 0)
            {
                vNowAngle.z += fDecAngle * Time.deltaTime;
                if (vNowAngle.z > 0)
                    vNowAngle.z = 0;
            }
        }

        //角度反映
        transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x + vNowAngle.x, 
                                            transform.rotation.eulerAngles.y ,
                                            transform.rotation.eulerAngles.z + vNowAngle.z * -1);
	}
}
