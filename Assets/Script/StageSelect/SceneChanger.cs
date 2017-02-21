using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    string NextScene = null;   //次シーン
    Fade Fade;

	// Use this for initialization
	void Start () 
    {
        Fade = GameObject.Find("FadeCanvas").GetComponent<Fade>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (NextScene != null && Fade.GetFading() == false ){
			SoundManager.Instance.StopBGM();
            SceneManager.LoadScene(NextScene);
		}
	}

    public bool SetNextScene(string next)
    {
        if (NextScene != null) return false;

        //シーン名設定
        NextScene = next;


        //フェード開始
        if (Fade.GetFading() == false)
            Fade.StartFade(Fade.eFADEMODE.FadeOut);

        return true;
    }
}
