//================================================
//概要:スタート時のカウントダウン
//制作者:佐々木瑞生
//
//================================================
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartProduction : MonoBehaviour {
    public float m_NowNumber;       // 現在カウント
    public bool m_DoingCountDown;  // カウントダウンの最中か否か
    public RaceTimer raceTimer;     // レース用タイマー
    public Image m_StartImage;
    public Sprite[] m_Sprites = new Sprite[11];
    public float ImageAlphaChangeVolume = 100;
    public float NumberAlphaChangeVolume = 100;
    public Vector2 m_NumberSize = new Vector2(180,319);
    public Vector2 m_StartImageSize = new Vector2(300,100);
    public bool isStart;
    public Color[] NumberColor = new Color[11];
    [SerializeField]
    private Fade fade;
    private bool isChangeNumber;
    private int oldNumber;
    private Vector2 m_TextSize;     // 現在テキストサイズ
    [SerializeField]
    private float m_ScaleChangeVolume;
    [SerializeField]
    private float m_fillAmountChangeVolume;
    [SerializeField]
    private float m_WaitTime;
    // Use this for initialization
    void Start() {
        m_DoingCountDown = false;
        isStart = false;
    }

    public void StartCountDown(float StartNumber = 3) {
        m_NowNumber = StartNumber - 0.001f;
        oldNumber = (int)StartNumber;
        isStart = false;
        m_TextSize = m_StartImage.rectTransform.sizeDelta = m_NumberSize;
        m_StartImage.sprite = m_Sprites[(int)m_NowNumber + 1];
        Color color = m_StartImage.color;
        color = NumberColor[( (int)m_NowNumber + 1 )];
        m_StartImage.color = color;
        m_DoingCountDown = true;
        SoundManager.Instance.PlaySE("StartSignal");
    }

    // Update is called once per frame
    void Update() {
        if(fade.bFading) {
            return;
        }
        if(m_DoingCountDown) {
            if(!m_StartImage.enabled)
                m_StartImage.enabled = true;
            m_NowNumber -= Time.deltaTime;
            m_TextSize.x -= m_ScaleChangeVolume * Time.deltaTime;
            m_TextSize.y -= m_ScaleChangeVolume * Time.deltaTime * m_NumberSize.x / m_NumberSize.y;
            m_StartImage.rectTransform.sizeDelta = m_TextSize;
            Color color = m_StartImage.color;
            color.a += NumberAlphaChangeVolume / 255.0f * Time.deltaTime;
            if(oldNumber > (int)m_NowNumber) {
                m_StartImage.sprite = m_Sprites[( (int)m_NowNumber + 1 )];
                color = NumberColor[( (int)m_NowNumber + 1 )];
                oldNumber = (int)m_NowNumber;
                m_TextSize = m_NumberSize;
            }
            m_StartImage.color = color;
            if(m_NowNumber < 0.0f) {
                m_StartImage.sprite = m_Sprites[10];
                m_StartImage.color = NumberColor[10];
                m_StartImage.rectTransform.sizeDelta = m_StartImageSize;
                m_StartImage.fillAmount = 0;
                m_DoingCountDown = false;
                isStart = true;
                raceTimer.RaceStart();
            }
        } else if(m_NowNumber < 0.0f) {
            m_StartImage.fillAmount += m_fillAmountChangeVolume * Time.deltaTime;
            if(m_StartImage.fillAmount >= 1.0f) {
                if(m_WaitTime < 0.0f) {
                    Color color = m_StartImage.color;
                    color.a -= ImageAlphaChangeVolume / 255.0f * Time.deltaTime;
                    m_StartImage.color = color;
                    if(m_StartImage.color.a < 0.0f)
                        m_StartImage.enabled = false;
                }else {
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }
}
