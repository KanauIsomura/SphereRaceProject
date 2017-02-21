using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade : MonoBehaviour {
	public const float ScreenWidth = 1024.0f;
	public const float ScreenHeight = 576.0f;
	public const float HEIGHT_CORRECTION = (1.0f / ScreenWidth) * ScreenHeight;
	public enum eFADEMODE {
		FadeIn,
		FadeOut
	};

	public float fSizeLimit;

	public eFADEMODE FadeMode = 0;          // フェードモード
	public float FadeVolume;        // アルファ値変化量
	public float PublicScaleChangeVolume = 0.01f;// 大きさ変化量
	public bool bFading;
	public float fAlpha;

	private float fScaleChangeVolume;// 大きさ変化量
    
    public Image image;
	public RectTransform m_RectTransForm;
    private Vector2 vecSize;
    
	private bool FadePause = false; // ポーズの影響を受けるか否か(falseで受けない)
	private float DifferenceTime;	// 前フレームとの差
    [SerializeField]
    private float m_FadeAccelerator;
    // Use this for initialization
    void Start () {
		fScaleChangeVolume = PublicScaleChangeVolume;// 大きさ変化量
	}

    // 使い始めるとき
    public void StartFade(eFADEMODE fadeMode = eFADEMODE.FadeIn, bool pause = false) {
        FadeMode = fadeMode;
        bFading = true;
		FadePause = pause;

		if(!image.enabled) {
			image.enabled = true;
		}

		if(FadeMode == eFADEMODE.FadeIn) {
            fAlpha = 255.0f;
			m_RectTransForm.sizeDelta = new Vector2(ScreenWidth, ScreenHeight);
		} else {
            fAlpha = 0.0f;
			m_RectTransForm.sizeDelta = new Vector2(fSizeLimit, fSizeLimit * HEIGHT_CORRECTION);
		}
        var color = image.color;
        color.a = fAlpha;
        image.color = color;
    }

    // Update is called once per frame
    void Update() {
        if(!bFading) return;
        vecSize = m_RectTransForm.sizeDelta;
        if(FadePause) {
            DifferenceTime = Time.deltaTime;
        } else {
            DifferenceTime = Time.unscaledDeltaTime;
        }
        switch(FadeMode) {
            case eFADEMODE.FadeIn:
                vecSize.x += fScaleChangeVolume * m_FadeAccelerator * 0.5f * DifferenceTime;
                vecSize.y += fScaleChangeVolume * m_FadeAccelerator * 0.5f * HEIGHT_CORRECTION * DifferenceTime;
                if(vecSize.x > fSizeLimit || vecSize.y > fSizeLimit * HEIGHT_CORRECTION) {
                    vecSize = new Vector2(fSizeLimit, fSizeLimit * HEIGHT_CORRECTION);
                    bFading = false;
                }
                if(vecSize.x > ScreenWidth) {
                    fAlpha -= FadeVolume * DifferenceTime;
                }
                if(fAlpha < 0.0f) {
                    fAlpha = 0.0f;
                }
                m_FadeAccelerator += 1.5f * DifferenceTime;
                break;
            case eFADEMODE.FadeOut:
                vecSize.x -= fScaleChangeVolume * DifferenceTime;
                vecSize.y -= fScaleChangeVolume * HEIGHT_CORRECTION * DifferenceTime;
                if(vecSize.x < ScreenWidth || vecSize.y < ScreenHeight) {
                    vecSize = new Vector2(ScreenWidth, ScreenHeight);
                    bFading = false;
                }
                fAlpha += FadeVolume * DifferenceTime;
                if(fAlpha > 255.0f) {
                    fAlpha = 255.0f;
                }

                break;
            default:
                break;
        }
        m_RectTransForm.sizeDelta = vecSize;
        var color = image.color;
        //color.a = fAlpha / 255.0f;
        color.a = 255.0f;
        image.color = color;
    }

    public bool GetFading() {
        return bFading;
    }
}
