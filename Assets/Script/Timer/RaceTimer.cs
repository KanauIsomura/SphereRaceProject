//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//
//  RaceTimer.cs
//
//  作成者:佐々木瑞生
//==================================================
//  概要
//  タイマー管理全般
//
//==================================================
//
//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
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
	public RectTransform m_TimerRectTransForm;
	public RaceRanking ranking;
    [SerializeField]
    private string BGMName;
    [SerializeField]
    private Timer raceTimer;
    [SerializeField]
    private Timer m_TimerPrefab;           // タイマープレハブ
	[SerializeField]
    private Timer[] m_TimerObject;         // タイマーオブジェクト
    private CheckPointChecker playerCheckPoint; // プレイヤー周回情報
	private Vector3 m_LapTimerPosOffset = new Vector3(360,173,0);	// タイマーの位置のオフセット
	private float m_TimerPosInterval = -60.0f;                       // タイマーの位置の間隔
	static public float m_LapTime;									// ラップタイム
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
        playerCheckPoint = GameObject.Find("Player").GetComponent<CheckPointChecker>();
		m_LapTime = 0;
		m_TimerObject = new Timer[playerCheckPoint.m_RequiredLapNum];
        for(int i = 0; i < m_TimerObject.Length; i++) {
			Vector3 pos = m_LapTimerPosOffset;
			pos.y = pos.y + i * m_TimerPosInterval;
			m_TimerObject[i] = (Timer)Instantiate(m_TimerPrefab);
			m_TimerObject[i].transform.parent = transform;
			m_TimerObject[i].transform.localPosition = pos;
			m_TimerObject[i].SendTimer(0);
			m_TimerObject[i].ChangeScale(new Vector3(1.5f, 1.5f, 1));
			m_TimerObject[i].ChangeColor(Color.red);
        }
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
            raceTimer.SendTimer(m_ElapsedTime);
			m_LapTime += Time.deltaTime;
			// 必要周回数が0だとバグるのでエラー処理
			if(playerCheckPoint.m_RequiredLapNum > 1)
				m_TimerObject[playerCheckPoint.m_NowLapNum].SendTimer(m_LapTime);
		}
	}
}
