//================================
/*
 取扱説明書
 GoalCameraにはMainCameraを
 RaceTimerにはTimeキャンバスを入れてください。
 */
//================================

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoalChecker : MonoBehaviour {
	public GoalCamera m_goalCamera;
	public RaceTimer m_raceTimer;
	public Image m_GoalImage;
	public float m_FadeWaitTime;		// フェードに移行するまでの待ち時間。
	public Fade m_fade;					// フェードカンバス
	private bool isFade;
	public string StateSceneName;
	public float GoalTimeScale = 0.2f;
    [SerializeField]
    private RaceRanking ranking;
    [SerializeField]
    private MultiInput.CONTROLLER_BUTTON EndButton;
    [SerializeField]
    private float ChangeRankingWaitTime;
    [SerializeField]
    private float ChangeSceneWaitTime;
    [SerializeField]
    private GameObject DoNotDeleteCanvas;
    [SerializeField]
    private GameObject DeleteCanvas;
    [SerializeField]
    private Canvas[] TimeCanvas = new Canvas[2];
    [SerializeField]
    private Camera[] NotMainCameras = new Camera[2];
    // Use this for initialization
    void Start () {
		m_GoalImage.enabled = false;
		isFade = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!m_goalCamera.bCameraEnd) return;
		Time.timeScale = 1;
		m_FadeWaitTime -= Time.deltaTime;
        if(m_FadeWaitTime > 0.0f)
            m_GoalImage.enabled = true;
        if(m_FadeWaitTime < 0.0f && !isFade) {
            if(m_GoalImage.enabled) {
                m_GoalImage.enabled = false;
                TimeCanvas[0].enabled = true;
                TimeCanvas[1].enabled = true;
            }
            if(m_FadeWaitTime < -ChangeRankingWaitTime) {
                ranking.ChangeRankingPosition();
            }else {
                ranking.ShowRanking();
            }
            if(!isFade && (MultiInput.Instance.GetTriggerButton(EndButton) || m_FadeWaitTime < -ChangeSceneWaitTime)) {
                isFade = true;
                m_fade.StartFade(Fade.eFADEMODE.FadeOut);
            }
		}else if(isFade && !m_fade.bFading) {
			SoundManager.Instance.StopBGM();
			SceneManager.LoadScene(StateSceneName);
		}
	}

	void OnTriggerEnter(Collider obj) {
		bool isPlayerTag = (obj.tag != "Player");
		// プレイヤーで、まだゴールしてなくて、ゴール条件を満たしていたらゴールできる。
		if( isPlayerTag || m_goalCamera.bGoal || !GameObject.Find("Player").GetComponent<CheckPointChecker>().m_CanPlayerGoal)
			return;
		Time.timeScale = GoalTimeScale;
		m_goalCamera.bGoal = true;
		m_goalCamera.bFadeStart = true;
		m_raceTimer.PlayerGoal();
		GameObject.Find("Player").GetComponent<CheckPointChecker>().bGoal = true;
        if(DeleteCanvas.activeSelf || DoNotDeleteCanvas.activeSelf) {
            DeleteCanvas.SetActive(false);
            TimeCanvas[0].enabled = false;
            TimeCanvas[1].enabled = false;
            NotMainCameras[0].enabled = false;
            NotMainCameras[1].enabled = false;
        }
	}
}
