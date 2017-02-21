using UnityEngine;
using System.Collections;

public class TitleAnimeTiming : MonoBehaviour {

    //private Animator[] anim = new Animator[2];
    [SerializeField, Header("塊")]
    Animator KatamariAnim;
    [SerializeField, Header("魂")]
    Animator DamashiiAnim;

    [SerializeField, Header("アニメを再生したい時間")]
    float[] AnimeTime;

    [SerializeField, Header("Push Enter")]
    TitleFadeImage image;

    [SerializeField, Header("↑がFade開始する時間")]
    float fFadeTime;

    private float timeElapsed;      // 経過時間保存用変数
    private int nAnimCnt;           // 再生したいアニメカウンタ

    // Use this for initialization
    void Start () {
        timeElapsed = 0;
        nAnimCnt = 0;
    }

    // Update is called once per frame
    void Update() {

        timeElapsed += Time.deltaTime;  // 時間を経過

        if (AnimeTime.Length <= nAnimCnt)  // アニメーションカウンタ最大なら
        {
            if (timeElapsed > fFadeTime)  //タイトルのアニメーションが落ちてきたら
            {
                image.SetFade(TitleFadeImage.FadeState.FADE_IN);            // PushEnterのフェード開始
                transform.GetComponent<TitleAnimeTiming>().enabled = false; // このスクリプトのアクティブをOFF(今後余計な処理に入らない為)
            }
            return;
        }


        if (timeElapsed > AnimeTime[nAnimCnt])  // 指定時間経過したら
        {
            switch (nAnimCnt)
            {
                case 0:
                    KatamariAnim.SetTrigger("IsDown");    // 塊のアニメーションを再生
                    break;
                case 1:
                    DamashiiAnim.SetTrigger("IsDown");    // 魂のアニメーションを再生
                    break;
            }
            nAnimCnt++;         // 再生したいアニメカウンタを次へ
            timeElapsed = 0;    // 経過時間リセット
        }
	}
}
