using UnityEngine;
using System.Collections;

public class UFOEffect : MonoBehaviour {

    private StartProduction StartFlg;   //スタート判定用
    public ParticleSystem[] Particle;
    bool bFlg = true;
    StartDash StartDashCount;

	// Use this for initialization
    void Start()
    {
        //取得
        StartFlg = GameObject.Find("StartProduction").GetComponent<StartProduction>();
        SetParticle(false);
        StartDashCount = GameObject.Find("Player").GetComponent<StartDash>();
	}
	
	// Update is called once per frame
	void Update () {

        //カウントダウン中
        if (StartFlg.isStart == false)
        {
            if (StartDashCount.NowDashCount > 0)
            {
                SetParticle(true);
            }
            else
            {
                SetParticle(false);
            }
            return;
        }

        float fVertical = MultiInput.Instance.GetLeftStickAxis().y;      //上下


        if (fVertical > 0)
        {
            SetParticle(true);
        }
        else
        {
            SetParticle(false);
        }
	}

    void SetParticle(bool bflg)
    {
        if (bFlg == bflg) return;

        int i;
        bFlg = bflg;

        if (bFlg)
        {
            for (i = 0; i < Particle.Length; i++)
                Particle[i].Play();
        }
        else
        {
            for (i = 0; i < Particle.Length; i++)
                Particle[i].Stop();
        }
    }
}
