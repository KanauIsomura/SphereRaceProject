//====================================================
//つくったひと:佐々木瑞生
//
//☆チェックポイントを置くときのルール☆
//①チェックポイントは3個以上置くこと
//②ゴールの番号が一番大きい数字になること
//③原則置く順番は通る順番とすること
//④スタート地点は0番とし、どうあがいてもそこは通るようにすること
//-----以下追加するかも-----
// NEOからの変更点(未完成)
//⑥チェックポイント0番はゴールより前かつプレイヤーの初期位置より後ろにすること
//⑤チェックポイントにコライダーは不要です、たぶん。そのかわりちゃんとポジションを設定すること(予定)
//====================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CheckPointCheckerNEO : MonoBehaviour {
    public bool m_CanPlayerGoal; // ゴールできる条件を満たしているか
    private int m_MaxCheckPointNum;  // チェックポイントの最大数
    private int m_OldCheckPointNumber;    // 前回のチェックポイントナンバー
	[SerializeField]
    private int m_CheckPointScore;
    private GameObject[] m_TagObjects;
	[SerializeField]
	private List<GameObject> m_ObjList;
    private bool m_isRevers;
    [SerializeField]
    private Image ReverseImage;
    private int m_OldScore;
    public bool bGoal;
    private bool m_CourseRound;
    [SerializeField]
    private float m_ColorChangeVolume;
	[SerializeField]
	private Transform m_PlayerTransform;
    // Use this for initialization
    void Start () {
		m_MaxCheckPointNum = CheckTagObjectNumber("CheckPoint");
		m_CheckPointScore = 0;
		m_OldScore = 0;
		bGoal = false;
		m_CourseRound = false;
		m_ObjList.Clear();
		for(int i = m_TagObjects.Length - 1; i >= 0; i--) {
			m_ObjList.Add(m_TagObjects[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckNowScore_new();
		if(m_isRevers) {
			FlashImageColor();
		}
	}

	public void CheckNowScore() {
		// var container = 
		int TargetPointNum;
		int CheckPointNum = m_CheckPointScore;

		if(m_CheckPointScore < 0)
			CheckPointNum = m_TagObjects.Length - Mathf.Abs(m_CheckPointScore);
		if(CheckPointNum == m_TagObjects.Length - 1) {
			TargetPointNum = 0;
		} else {
			TargetPointNum = CheckPointNum + 1;
		}

		m_OldScore = m_CheckPointScore;

		Vector3 AP = Mathematics.VectorCalculation(m_ObjList[CheckPointNum].transform.position, m_PlayerTransform.position);
		Vector3 AB = Mathematics.VectorCalculation(m_ObjList[CheckPointNum].transform.position,m_ObjList[TargetPointNum].transform.position);
		float DotAPAB = Vector3.Dot(AP,AB);
		float DotABAB = Vector3.Dot(AB,AB);

		Debug.Log("m_CheckPointScore" + m_CheckPointScore);
		Debug.Log("CheckPointNum" + CheckPointNum);
		//Debug.Log("DotAPAB" + DotAPAB);
		//Debug.Log("DotABAB" + DotABAB);

		if(DotAPAB <= 0) {
			m_CheckPointScore--;
		} else if(DotAPAB > DotABAB) {
			m_CheckPointScore++;
		}

		if(m_OldScore < m_CheckPointScore || bGoal) {
			if(m_isRevers) {
				m_isRevers = false;
				ReverseImage.GetComponent<Animator>().SetTrigger("Out");
			}
		} else if(m_OldScore < m_CheckPointScore) {
			if(!m_isRevers) {
				m_isRevers = true;
				ReverseImage.color = new Color(ReverseImage.color.r, 0.0f, ReverseImage.color.b);
				ReverseImage.GetComponent<Animator>().SetTrigger("In");
			}
		}
	}

	public void CheckNowScore_new() {
		Vector3 AP;
		Vector3 AB;
		float DotAPAB;
		float DotABAB;
		int CheckPointNum = 0;

		for(int i = 0; i < m_TagObjects.Length - 1; i++) {
			AP = Mathematics.VectorCalculation(m_ObjList[i].transform.position, m_PlayerTransform.position);
			AB = Mathematics.VectorCalculation(m_ObjList[i].transform.position, m_ObjList[i != m_TagObjects.Length - 1 ? i + 1 : 0].transform.position);
			DotAPAB = Vector3.Dot(AP, AB);
			DotABAB = Vector3.Dot(AB, AB);
			if(0 < DotAPAB && DotAPAB < DotABAB) {
				CheckPointNum = i;
				break;
			}
		}
		Debug.Log("CheckPointNum" + CheckPointNum);
	}

	public void CheckNowScore_neo() {
		// var container = 
		int TargetPointNum;
		int CheckPointNum = m_CheckPointScore;
		if(m_CheckPointScore < 0)
			CheckPointNum = m_TagObjects.Length - Mathf.Abs(m_CheckPointScore);
		if(CheckPointNum == m_TagObjects.Length - 1) {
			TargetPointNum = 0;
		} else {
			TargetPointNum = CheckPointNum + 1;
		}

		m_OldScore = m_CheckPointScore;

		Vector3 AP = Mathematics.VectorCalculation(m_ObjList[CheckPointNum].transform.position, m_PlayerTransform.position);
		Vector3 AB = Mathematics.VectorCalculation(m_ObjList[CheckPointNum].transform.position,m_ObjList[TargetPointNum].transform.position);
		Vector3 NormalAB = Vector3.Normalize(AB);
		float DotAPNorAB = Vector3.Dot(AP, NormalAB);

		Debug.Log("m_CheckPointScore" + m_CheckPointScore);
		Debug.Log("CheckPointNum" + CheckPointNum);
		//Debug.Log("DotAPNorAB" + DotAPNorAB);

		if(DotAPNorAB <= 0) {
			m_CheckPointScore--;
		} else if(Mathematics.VectorSize(AB) > DotAPNorAB) {
			m_CheckPointScore++;
		}

		if(m_OldScore < m_CheckPointScore || bGoal) {
			if(m_isRevers) {
				m_isRevers = false;
				ReverseImage.GetComponent<Animator>().SetTrigger("Out");
			}
		} else if(m_OldScore < m_CheckPointScore) {
			if(!m_isRevers) {
				m_isRevers = true;
				ReverseImage.color = new Color(ReverseImage.color.r, 0.0f, ReverseImage.color.b);
				ReverseImage.GetComponent<Animator>().SetTrigger("In");
			}
		}
	}

	//シーン上のBlockタグが付いたオブジェクトを数える
	int CheckTagObjectNumber(string tagname) {
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
