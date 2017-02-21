using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RaceTimer : MonoBehaviour {
	public float m_ElapsedTime;    // レース時間
	private float m_fStartTime;
    [SerializeField]
    private bool bStart;
    [SerializeField]
    private bool bGoal;
	public Image[] m_NumberImage = new Image[6];
	public Image[] m_ColonImage = new Image[2];
	public Sprite[] m_NumberSprite = new Sprite[10];
	public Sprite m_ColonSprite;
	public Vector2 m_NumberSize = new Vector2(80,100);
	public RectTransform m_TimerRectTransForm;
	public RaceRanking ranking;
    [SerializeField]
    private string BGMName;
	// Use this for initialization
	void Start() {
		Init();
    }

	/// <summary>
	/// 初期化
	/// </summary>
	private void Init() {
		bStart = false;
		bGoal = false;
	}

	// Update is called once per frame
	void Update() {
		TimeCalculation();
	}

	/// <summary>
	/// レーススタート
	/// </summary>
	public void RaceStart() {
		bStart = true;
        SoundManager.Instance.PlayBGM(BGMName);
		m_fStartTime = Time.time;
	}

	/// <summary>
	/// レース終了
	/// </summary>
	public void PlayerGoal() {
		bGoal = true;
        SoundManager.Instance.StopBGM();
		ranking.RankingUpdate(m_ElapsedTime);
    }

	/// <summary>
	/// 経過時間計算
	/// </summary>
	private void TimeCalculation() {
		if(bStart && !bGoal) {
			m_ElapsedTime = Time.time - m_fStartTime;
			if(m_ElapsedTime > 5999.99f) {
				m_ElapsedTime = 5999.99f;
			}
			int Minutes = (int)m_ElapsedTime / 60;
			int Second = (int)(m_ElapsedTime - Minutes * 60);
			int DecimalPlaces = (int)((m_ElapsedTime - (Minutes * 60 + Second)) * 100);
			m_NumberImage[0].sprite = m_NumberSprite[Minutes / 10];
			m_NumberImage[1].sprite = m_NumberSprite[Minutes % 10];
			m_NumberImage[2].sprite = m_NumberSprite[Second / 10];
			m_NumberImage[3].sprite = m_NumberSprite[Second % 10];
			m_NumberImage[4].sprite = m_NumberSprite[DecimalPlaces / 10];
			m_NumberImage[5].sprite = m_NumberSprite[DecimalPlaces % 10];
		}
	}
}
