using UnityEngine;
using System.Collections;

public class SteamEffect : MonoBehaviour {

    public float StartSpeed = 5.0f;     //表示を始める速度
    private PlayerMove PlayerSpeed;     //プレイヤー速度取得用
    private ParticleSystem Effect;      //表示切替用
    private bool bDraw = true;          //表示管理フラグ
    private StartProduction StartFlg;   //スタート判定用
    RaycastHit hit;
    public float fGroundDistance = 1.5f;    //地面までの距離
    CharacterController CharaCon;

	// Use this for initialization
	void Start () {
        PlayerSpeed = GameObject.Find("Player").GetComponent<PlayerMove>();
        StartFlg = GameObject.Find("StartProduction").GetComponent<StartProduction>();
        Effect = GetComponent<ParticleSystem>();
        CharaCon = GameObject.Find("Player").GetComponent<CharacterController>();
        SetEffect(false);
	}
	
	// Update is called once per frame
	void Update () {

        //カウントダウンちゅうなら
        if (StartFlg.isStart == false) return;

        //スピード判定
        if (StartSpeed <= Mathf.Abs(PlayerSpeed.PlayerSpeed))
        {
            SetEffect(true);
        }
        else
        {
            SetEffect(false);
        }


        //接地判定
        if (HitCheckGround() == false)
            SetEffect(false);
	}

    void SetEffect(bool bFlg)
    {
        if (bDraw == bFlg) return;

        bDraw = bFlg;

        if (bDraw == true)
        {
            Effect.Play();
        }
        else
        {
            Effect.Stop();
        }

    }

    /// <summary>
    /// 接地判定
    /// </summary>
    /// <returns></returns>
    bool HitCheckGround()
    {
        Vector3 pos = transform.parent.position;
        //pos.y -= 2;
        Physics.SphereCast(pos, CharaCon.radius, -transform.parent.up, out hit);
        if (CharaCon.radius >= hit.distance)
        {
            return true;
        }
        else
        {//空中
            return false;
        }
    }
}
