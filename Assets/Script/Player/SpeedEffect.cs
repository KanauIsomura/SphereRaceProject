using UnityEngine;
using System.Collections;

public class SpeedEffect : MonoBehaviour {

    public float StartSpeed = 5.0f;     //表示を始める速度
    private PlayerMove PlayerSpeed;     //プレイヤー速度取得用
    private ParticleSystem Effect;      //表示切替用
    private bool bDraw = false;         //表示管理フラグ
    private StartProduction StartFlg;   //スタート判定用

	// Use this for initialization
    void Start()
    {
        PlayerSpeed = GameObject.Find("Player").GetComponent<PlayerMove>();
        StartFlg = GameObject.Find("StartProduction").GetComponent<StartProduction>();
        Effect = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //カウントダウンちゅうなら
        if (StartFlg.isStart == false) return;

        //表示判定
        if (StartSpeed <= Mathf.Abs(PlayerSpeed.PlayerSpeed))
        {
            if (bDraw != true)
            {
                bDraw = true;
                Effect.Play();
            }
        }
        else
        {
            if (bDraw != false)
            {
                bDraw = false;
                Effect.Stop();
            }
        }
	}
}
