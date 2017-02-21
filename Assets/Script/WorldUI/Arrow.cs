using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Arrow : MonoBehaviour {
    [SerializeField]
    private Image m_arrowImage;
    [SerializeField]
    private Color m_FirstColor,m_SecondColor;
    [SerializeField]
    private float m_ChangeTime;
    private bool m_ColorDirection;
    private Color m_DifferenceColor;
    private float m_Time;

    // Use this for initialization
    void Start () {
        m_DifferenceColor = m_SecondColor - m_FirstColor;
        m_ColorDirection = true;
        m_arrowImage.color = m_FirstColor;
        m_Time = 0;
    }
	
	// Update is called once per frame
	void Update () {
        Color imageColor = m_arrowImage.color;
        m_Time += Time.deltaTime / m_ChangeTime;
        if(m_Time > 1.0f) {
            m_Time = 0;
            m_ColorDirection = !m_ColorDirection;
        }
        if(m_ColorDirection) {
            imageColor = m_FirstColor + ( m_DifferenceColor * m_Time );
        }else {
            imageColor = m_SecondColor - ( m_DifferenceColor * m_Time );
        }
        m_arrowImage.color = imageColor;
    }
}
