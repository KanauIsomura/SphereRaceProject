using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseCanvas : MonoBehaviour {
	enum PauseState {
		StageSelect,
		Cancel,
		Restart,

		MaxPauseState
	};
	public MultiInput.CONTROLLER_BUTTON PauseButton;
	public MultiInput.CONTROLLER_BUTTON DecisionButton;
	public MultiInput.CONTROLLER_BUTTON LeftButton;
	public MultiInput.CONTROLLER_BUTTON RightButton;


	public Canvas m_thisCanvas;
	public Image[] m_FrameImage;
	public Image[] m_IconImage;
	private PauseState m_nowState;
	[SerializeField]private string thisSceneName;
	[SerializeField]private string StageSelectSceneName;
	[SerializeField]private float m_ScaleChangeVolume;
    [SerializeField]private int m_ScaleLimit,m_ScaleLimitMin,m_DefaultScale;
    [SerializeField]private StartProduction m_StartProduction;
    public bool isPausing;
	// Use this for initialization
	void Start () {
		m_nowState = PauseState.Cancel;
		m_thisCanvas.enabled = false;
		isPausing = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_StartProduction.isStart && MultiInput.Instance.GetTriggerButton(PauseButton)) {
			m_thisCanvas.enabled = !m_thisCanvas.enabled;
			isPausing = !isPausing;
			if(m_thisCanvas.enabled) {
				Time.timeScale = 0;
                SoundManager.Instance.ChangeVolumeRate(1.0f/3.0f, 1.0f / 3.0f);
                SoundManager.Instance.PlaySE("select");
			} else{
				Time.timeScale = 1;
                SoundManager.Instance.ChangeVolumeRate(3.0f, 3.0f);
                SoundManager.Instance.PlaySE("noselect");
            }
		}
		if(m_thisCanvas.enabled) {
			ChangeSelectIcon();
			ChengeIconScale();
		}
	}

	private void ChangeSelectIcon() {
		if(MultiInput.Instance.GetTriggerButton(LeftButton) || MultiInput.Instance.GetTriggerStickAxis(MultiInput.STICK_AXIS.LEFTSTICK_LEFT)) {
			m_IconImage[(int)m_nowState].rectTransform.sizeDelta = new Vector2(m_DefaultScale, m_DefaultScale);
			m_ScaleChangeVolume = Mathf.Abs(m_ScaleChangeVolume);
			m_nowState -= 1;
            SoundManager.Instance.PlaySE("pi03");
            if(m_nowState < 0) {
				m_nowState = PauseState.MaxPauseState - 1;
			}
		}
		if(MultiInput.Instance.GetTriggerButton(RightButton) || MultiInput.Instance.GetTriggerStickAxis(MultiInput.STICK_AXIS.LEFTSTICK_RIGHT)) {
			m_IconImage[(int)m_nowState].rectTransform.sizeDelta = new Vector2(m_DefaultScale, m_DefaultScale);
			m_ScaleChangeVolume = Mathf.Abs(m_ScaleChangeVolume);
			m_nowState += 1;
            SoundManager.Instance.PlaySE("pi03");
            if(m_nowState >= PauseState.MaxPauseState) {
				m_nowState = 0;
			}
		}
		switch(m_nowState) {
			case PauseState.StageSelect:
				m_FrameImage[(int)PauseState.StageSelect].enabled = true;
				m_FrameImage[(int)PauseState.Cancel].enabled = false;
				m_FrameImage[(int)PauseState.Restart].enabled = false;
				if(MultiInput.Instance.GetTriggerButton(DecisionButton)) {
					SceneManager.LoadScene(StageSelectSceneName);
					Time.timeScale = 1;
                    SoundManager.Instance.PlaySE("select");
					SoundManager.Instance.StopBGM();
                }
				break;
			case PauseState.Cancel:
				m_FrameImage[(int)PauseState.StageSelect].enabled = false;
				m_FrameImage[(int)PauseState.Cancel].enabled = true;
				m_FrameImage[(int)PauseState.Restart].enabled = false;
				if(MultiInput.Instance.GetTriggerButton(DecisionButton)) {
					m_thisCanvas.enabled = false;
					Time.timeScale = 1;
                    SoundManager.Instance.PlaySE("pi03");
                }
				break;
			case PauseState.Restart:
				m_FrameImage[(int)PauseState.StageSelect].enabled = false;
				m_FrameImage[(int)PauseState.Cancel].enabled = false;
				m_FrameImage[(int)PauseState.Restart].enabled = true;
				if(MultiInput.Instance.GetTriggerButton(DecisionButton)) {
					Time.timeScale = 1;
					SceneManager.LoadScene(thisSceneName);
                    SoundManager.Instance.PlaySE("select");
					SoundManager.Instance.StopBGM();
                }
				break;
			default:
				break;
		}
	}

	private void ChengeIconScale() {
		var ImageScale = m_IconImage[(int)m_nowState].rectTransform.sizeDelta;
		ImageScale.x += m_ScaleChangeVolume * Time.unscaledDeltaTime;
		ImageScale.y += m_ScaleChangeVolume * Time.unscaledDeltaTime;
		if(ImageScale.x > m_ScaleLimit || ImageScale.x < m_ScaleLimitMin) {
			m_ScaleChangeVolume *= -1;
		}
		m_IconImage[(int)m_nowState].rectTransform.sizeDelta = ImageScale;
	}
}
