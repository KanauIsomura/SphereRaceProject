using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedMaterBar : MonoBehaviour {
	enum MaterPosition_X {
		Right,
		Left,
		Center,
	};
	enum MaterPosition_Y {
		Up,
		Bottom,
		Center,
	};

	[SerializeField]
	private Image m_barImage;
	[SerializeField]
	private Image m_CircleImage;
	[Header("針のイメージ")]
	[SerializeField]
	private Image m_NeedleImage;
	[SerializeField]
	private GameObject m_MaterSummaryObj;
	public float m_SpeedMagnification;
	private float m_NowSpeed;
	private float m_MaxSpeed;
	public PlayerMove playerMove;
	private float m_OldTime;
	public float publicNowSpeed
	{
		set
		{
			m_NowSpeed = value;
		}
		get
		{
			return m_NowSpeed;
		}
	}
	private float Deviation;
	public float IntervalTime;
	[SerializeField]
	private MaterPosition_X MaterPosX;
	[SerializeField]
	private MaterPosition_Y MaterPosY;
	// Use this for initialization
	void Start () {
		m_OldTime = Time.time;
		if(!m_MaterSummaryObj.activeInHierarchy) {
			m_MaterSummaryObj.SetActive(true);
		}
		var pos = m_MaterSummaryObj.GetComponent<RectTransform>().position;
		var scale = m_MaterSummaryObj.GetComponent<RectTransform>().localScale;
		switch(MaterPosX) {
			case MaterPosition_X.Left:
				pos.x = 0.0f;
				scale.x = -1;
				break;
			case MaterPosition_X.Right:
				pos.x = Screen.width;
				scale.x = 1;
				break;
			case MaterPosition_X.Center:
				pos.x = Screen.width*0.5f;
				scale.x = 1;
				break;
		}
		switch(MaterPosY) {
			case MaterPosition_Y.Up:
				pos.y = Screen.height;
				scale.y = -1;
				break;		   
			case MaterPosition_Y.Bottom:
				pos.y = 0.0f;
				scale.y = 1;
				break;		   
			case MaterPosition_Y.Center:
				pos.y = Screen.height*0.5f;
				scale.y = 1;
				break;
		}
		m_MaterSummaryObj.GetComponent<RectTransform>().position = pos;
		m_MaterSummaryObj.GetComponent<RectTransform>().localScale = scale;
	}
	
	// Update is called once per frame
	void Update () {
		m_MaxSpeed = playerMove.PlayerMaxSpeed;
		m_NowSpeed = playerMove.PlayerSpeed;
		m_NowSpeed = Mathf.Abs(m_NowSpeed);
		m_CircleImage.transform.Rotate(new Vector3(0,0, m_NowSpeed * m_SpeedMagnification * Time.deltaTime));
		if(Time.time - m_OldTime > IntervalTime + Random.Range(-0.05f,0.05f)) {
			Deviation = Random.Range(0.0f, 0.08f);
			m_OldTime = Time.time;
		}
		m_barImage.fillAmount = 1.0f - ((m_NowSpeed / m_MaxSpeed > 1.0f ? 1.0f: m_NowSpeed / m_MaxSpeed) - Deviation);
		var rotation = m_NeedleImage.rectTransform.localEulerAngles;
		rotation.z = 90 - ((m_NowSpeed / m_MaxSpeed > 1.0f ? 1.0f - Deviation : Mathf.Clamp((m_NowSpeed / m_MaxSpeed) - Deviation,0.0f,1.0f))) * 90;
		m_NeedleImage.rectTransform.localEulerAngles = rotation;
	}
}
