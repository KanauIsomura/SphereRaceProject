//====================================================
//制作者:佐々木瑞生
//
//☆チェックポイントを置くときのルール☆
//①チェックポイントは3個以上置くこと
//②ゴールの番号が一番大きい数字になること
//③原則置く順番は通る順番とすること
//④スタート地点は0番とし、どうあがいてもそこは通るようにすること
//-----以下追加するかも-----
//====================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
public class CheckPointChecker : MonoBehaviour {
    public bool m_CanPlayerGoal; // ゴールできる条件を満たしているか
    public int m_MaxCheckPointNum;  // チェックポイントの最大数
    private int m_OldCheckPointNumber;    // 前回のチェックポイントナンバー
    [SerializeField]
    public int m_CheckPointScore;          // 現在チェックポイントスコア
    private GameObject[] m_TagObjects;
	private bool m_isRevers;
	private bool m_isReversCheck;
	private float m_ReverseTimer;
	[SerializeField]
	private Image ReverseImage;
	private int m_OldSocre;                 // 前フレームとの比較用
	public bool bGoal;
	private bool m_CourseRound;
	[SerializeField]
	private float m_ColorChangeVolume;
	private Vector3 m_OldPlayerPos;
	private Vector3 m_PlayerMoveVector;
	[SerializeField]
	private GameObject[] m_ObjList;
	[SerializeField]
	private float m_ReverseRange = 85.0f;
	[SerializeField]
	private float m_ReverseTime = 1.5f;
    [Header("ゴールより手前なら-1、奥なら0を入れてください")]
    [SerializeField]
    private int m_FirstPlayerScore = -1;
    [Header("周回数")]
    public int m_RequiredLapNum;    // 必要周回数
    [System.NonSerialized]
    public int m_NowLapNum;         // 現在周回数

	// Use this for initialization
	void Start () {
		// チェックポイントの数を調べます。
		m_MaxCheckPointNum = CheckTagObjectNumber("CheckPoint");
        m_CheckPointScore = m_FirstPlayerScore;
		m_OldSocre = m_FirstPlayerScore;
		m_ReverseTimer = 0;
		m_isReversCheck = false;
		bGoal = false;
		m_CourseRound = false;
        m_NowLapNum = 0;
        m_ObjList = new GameObject[m_TagObjects.Length];
		for(int i = m_TagObjects.Length - 1; i >= 0; i--) {
			int number = m_TagObjects[i].GetComponent<CheckPoint>().m_thisPointNumber;
			m_ObjList[number] = m_TagObjects[i];
		}
	}

	void Update() {
		ReverseChecker();
		if(m_isReversCheck) {
			m_ReverseTimer += Time.deltaTime;
		}else {
			m_ReverseTimer = 0;
			if(m_isRevers) {
				m_isRevers = false;
				ReverseImage.GetComponent<Animator>().SetTrigger("Out");
			}
		}
		if(m_ReverseTimer > m_ReverseTime) {
			if(!m_isRevers) {
				m_isRevers = true;
				ReverseImage.GetComponent<Animator>().SetTrigger("In");
			}
		}
		if(m_isRevers) {
			FlashImageColor();
		}
	}

	void LateUpdate() {
		m_PlayerMoveVector = Mathematics.VectorCalculation(m_OldPlayerPos,transform.position);
		m_OldPlayerPos = transform.position;
	}

    /// <summary>
    /// チェックポイントに入った時の制御
    /// </summary>
    /// <param name="CheckPointNumber">入ったチェックポイントの番号</param>
	public void IntoCheckPoint(int CheckPointNumber) {
		if(CheckPointNumber == 0) {
			m_CheckPointScore = 0;
			if(m_OldSocre == m_MaxCheckPointNum - 1) {
				m_CourseRound = true;
			}
		}

		if(m_OldCheckPointNumber < CheckPointNumber) {
			if((m_OldCheckPointNumber == 0 && CheckPointNumber == m_MaxCheckPointNum-1) || m_CheckPointScore < 0) {
				m_CheckPointScore = CheckPointNumber - m_MaxCheckPointNum;
			} else {
				m_CheckPointScore = CheckPointNumber;
			}
		}
		if(m_OldCheckPointNumber > CheckPointNumber) {
			if(m_CheckPointScore < 0) {
				m_CheckPointScore = CheckPointNumber - m_MaxCheckPointNum;
			} else {
				m_CheckPointScore = CheckPointNumber;
			}
		}

		if(m_CheckPointScore >= m_TagObjects.Length - 1 || m_CourseRound) {
			m_CanPlayerGoal = true;
		} else {
			m_CanPlayerGoal = false;
		}


		m_OldCheckPointNumber = CheckPointNumber;
		m_OldSocre = m_CheckPointScore;
	}

	/// <summary>
	/// 逆走チェッカー
	/// </summary>
	private void ReverseChecker() {
		int TargetPointNum;
		int CheckPointNum = m_CheckPointScore;

		if(m_CheckPointScore < 0)
			CheckPointNum = m_TagObjects.Length - Mathf.Abs(m_CheckPointScore);
		if(CheckPointNum == m_TagObjects.Length - 1) {
			TargetPointNum = 0;
		} else {
			TargetPointNum = CheckPointNum + 1;
		}

		if(Mathematics.VectorSize(m_PlayerMoveVector) == 0)
			return;

		Vector3 AB = Mathematics.VectorCalculation(m_ObjList[CheckPointNum].transform.position, m_ObjList[TargetPointNum].transform.position);

		float forwardRange = Mathematics.VectorRange(AB, transform.forward);
		float moveRange = Mathematics.VectorRange(AB, m_PlayerMoveVector);

		if(forwardRange > m_ReverseRange && moveRange > m_ReverseRange) {
			if(!m_isReversCheck) {
				m_isReversCheck = true;
			}
		} else {
			if(m_isReversCheck) {
				m_isReversCheck = false;
			}
		}
	}

	//シーン上のBlockタグが付いたオブジェクトを数える
	private int CheckTagObjectNumber(string tagname) {
        m_TagObjects = GameObject.FindGameObjectsWithTag(tagname);
        return m_TagObjects.Length;
    }

	private void FlashImageColor() {
		var ImageColor = ReverseImage.color;
		ImageColor.g += m_ColorChangeVolume / 255.0f * Time.deltaTime;
		if(ImageColor.g > 1.0f || ImageColor.g < 0.0f) {
			m_ColorChangeVolume *= -1;
            ImageColor.g = Mathf.Clamp(ImageColor.g, 0.0f, 1.0f);
		}
		ReverseImage.color = ImageColor;
	}
}
