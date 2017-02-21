using UnityEngine;
using System.Collections;


/// <summary>
/// ステージオブジェクト更新管理
/// </summary>
public class StageScaleController : MonoBehaviour {

    public ScaleStage[] m_StageObj;         //ステージオブジェクト
    public StageSelectPlayerMove m_Player;  //現在選らんでいる物取得用

    //フェード判別用
    Fade Fade;

    void Start()
    {
        Fade = GameObject.Find("FadeCanvas").GetComponent<Fade>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
	void Update () {


        for (int nCount = 0; nCount < m_StageObj.Length; nCount++)
        {
            if (m_Player.NowSelect == nCount)
                m_StageObj[nCount].SetUse(true);
            else
                m_StageObj[nCount].SetUse(false);
        }

        //フェード中は動かさない
        if (Fade.GetFading() == false)
        {
            if (MultiInput.Instance.GetTriggerButton(MultiInput.CONTROLLER_BUTTON.SELECT))
                GameObject.Find("SceneChanger").GetComponent<SceneChanger>().SetNextScene("Title");
        }
	}
}
