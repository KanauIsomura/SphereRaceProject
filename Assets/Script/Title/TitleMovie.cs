using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleMovie : MonoBehaviour {

    [Header("ムービー開始までの時間")]
    public float fMovieStartTime;

    [Header("再生したいムービー")]
    public MovieTexture movie;

    [Header("利用したいフェード")]
    public Fade fade;

    private Image texture;
    private float fTime;    // 経過時間管理
    private bool bMovie;    // ムービー再生中かどうか
    private bool bFade;
    private bool bResetFlg; 

	// Use this for initialization
	void Start () {
        fTime = fMovieStartTime;    // 開始時間を指定
        bMovie = false;             // 
        bFade = false;
        bResetFlg = false;
        texture = GetComponent<Image>();

        texture.material.mainTexture = movie;    // ムービーを入れる
        //movie.loop = false;
        texture.enabled = false;
    }


    /*
    // Update is called once per frame
    void Update ()
    {
        if (bMovie) // 既にムービー再生されてたら
        {
            if (!movie.isPlaying)// 再生終わったら
            {
                bResetFlg = true;
            }

            // 入力されたら
            if (MultiInput.Instance.GetPressButton(MultiInput.CONTROLLER_BUTTON.CIRCLE))
                bResetFlg = true;

            if (bResetFlg)
                ResetMovie();
        }
        else
        {
            fTime -= Time.deltaTime;    // 時間を経過

            if (fTime < 0)  // 未入力時間が経過したら
            {
                if (!bFade)
                {
                    Debug.Log("フェード開始");
                    bFade = true;
                    fade.StartFade(Fade.eFADEMODE.FadeOut);
                }

                if (!fade.bFading)
                {
                    fade.StartFade();
                    MovieStart();
                }
            }
        }
	}

    // ムービー再生
    private void MovieStart()
    {
        fTime = fMovieStartTime;
        texture.enabled = true;
        movie.Play();  // ここに再生処理

        //　音楽再生
        var audioSource = GetComponent<AudioSource>();
        audioSource.clip = movie.audioClip;
        audioSource.loop = false;
        audioSource.Play();
        Debug.Log("再生");

        bMovie = true;
        bFade = false;
    }

    // ムービーが終わったらまたは、入力があったら呼ぶ
    public void ResetMovie()
    {
        if (bMovie) // もしムービー再生されていたら
        {
            if (!bFade)
            {
                movie.Stop();   // ムービーを停止

                // 音を止める
                var audioSource = GetComponent<AudioSource>();
                audioSource.clip = movie.audioClip;
                audioSource.loop = false;
                audioSource.Stop();

                // フェード開始
                fade.StartFade(Fade.eFADEMODE.FadeOut);
                bFade = true;
            }

            if (!fade.bFading)
            {
                fade.StartFade();           // フェード
                texture.enabled = false;    // 動画を非表示
                bMovie = false;             
                bFade = false;
                bResetFlg = false;
                Debug.Log("停止");
            }
        }
        fTime = fMovieStartTime;
    }
    */
    public bool GetMovieFlg()
    {
        return texture.enabled;
    }

   
}
