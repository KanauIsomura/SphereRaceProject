//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//
//  Timer.cs
//
//  作成者:佐々木瑞生
//==================================================
//  概要
//  タイマー表示全般
//
//==================================================
//  作成日:2017/03/01
//  更新日:
//    2017/03/01:作成開始
//
//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public Image[] m_NumberImage = new Image[6];    // 数字イメージ
    public Image[] m_ColonImage = new Image[2];     // コロンイメージ
    public Sprite[] m_NumberSprite = new Sprite[10];// 数字スプライト
    public Sprite m_ColonSprite;                    // コロンスプライト
    //public Vector2 m_NumberSize = new Vector2(80,100);// 大きさ(使うかは不明)

    /// <summary>
    /// タイマー表示
    /// </summary>
    /// <param name="Time">表示時間</param>
    public void SendTimer(float Time) {
        if(Time > 5999.99f) {
            Time = 5999.99f;
        }
        int Minutes = (int)Time / 60;
        int Second = (int)(Time - Minutes * 60);
        int DecimalPlaces = (int)((Time - (Minutes * 60 + Second)) * 100);
        m_NumberImage[0].sprite = m_NumberSprite[Minutes / 10];
        m_NumberImage[1].sprite = m_NumberSprite[Minutes % 10];
        m_NumberImage[2].sprite = m_NumberSprite[Second / 10];
        m_NumberImage[3].sprite = m_NumberSprite[Second % 10];
        m_NumberImage[4].sprite = m_NumberSprite[DecimalPlaces / 10];
        m_NumberImage[5].sprite = m_NumberSprite[DecimalPlaces % 10];

    }

    /// <summary>
    /// 色変更
    /// </summary>
    /// <param name="TimerColor">変更後のカラー</param>
    public void ChangeColor(Color TimerColor) {
        for(int i = 0; i < 6; i++) {
            m_NumberImage[i].color = TimerColor;
        }
    }

    /// <summary>
    /// 拡大率変更
    /// </summary>
    /// <param name="TimerScale">変更後の拡大率</param>
    public void ChangeScale(Vector3 TimerScale) {
        transform.localScale = TimerScale;
    }

    /// <summary>
    /// 位置変更
    /// </summary>
    /// <param name="TimerPosition">変更後の位置</param>
    public void ChangePosition(Vector3 TimerPosition) {
        transform.position = TimerPosition;
    }
}
