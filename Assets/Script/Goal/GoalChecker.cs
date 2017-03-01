//================================
/*
 * 制作者 佐々木瑞生
 * 
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
    private Canvas[] TimeCanvas;
    [SerializeField]
    private Camera[] NotMainCameras;
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
                for(int i = 0; i < TimeCanvas.Length; i++)
                TimeCanvas[i].enabled = true;
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
        if(isPlayerTag)
            return;
        CheckPointChecker playerCheckPoint = GameObject.Find("Player").GetComponent<CheckPointChecker>();
        if(playerCheckPoint.m_NowLapNum <= playerCheckPoint.m_RequiredLapNum && playerCheckPoint.m_CheckPointScore >= playerCheckPoint.m_MaxCheckPointNum-1) {
            playerCheckPoint.m_NowLapNum++;
            playerCheckPoint.m_CheckPointScore = -1;
        }
        //if(m_goalCamera.bGoal || !playerCheckPoint.m_CanPlayerGoal)
        //return;
        if(playerCheckPoint.m_NowLapNum >= playerCheckPoint.m_RequiredLapNum) {
            Time.timeScale = GoalTimeScale;
            m_goalCamera.bGoal = true;
            m_goalCamera.bFadeStart = true;
            m_raceTimer.PlayerGoal();
            GameObject.Find("Player").GetComponent<CheckPointChecker>().bGoal = true;
            if(DeleteCanvas.activeSelf || DoNotDeleteCanvas.activeSelf) {
                DeleteCanvas.SetActive(false);
                for(int i = 0; i < TimeCanvas.Length; i++)
                    TimeCanvas[i].enabled = false;
                for(int i = 0; i < NotMainCameras.Length; i++)
                    NotMainCameras[i].enabled = false;
            }
        }
	}
}
