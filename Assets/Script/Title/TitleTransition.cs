using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleTransition : MonoBehaviour
{
    [Header("このシーンに移動")]
    public string SceneName;

    [Header("利用したいフェード")]
    public Fade fade;

    [Header("ムービー")]
    public TitleMovie movie;

    private bool bFade; // 

    // Use this for initialization
    void Start () {
        bFade = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (MultiInput.Instance.GetPressButton(MultiInput.CONTROLLER_BUTTON.CIRCLE) )//&& !movie.GetMovieFlg())    // ここでキー指定
        {
            if (!bFade)
            {
                Debug.Log("フェード開始");
                bFade = true;
                fade.StartFade(Fade.eFADEMODE.FadeOut);
            }
        }

        if (!fade.bFading && bFade)
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
