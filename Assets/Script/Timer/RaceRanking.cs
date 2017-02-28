//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//
//  RaceTimer.cs
//
//  作成者:佐々木瑞生
//==================================================
//  概要
//  ランキング管理
//
//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public class RaceRanking : MonoBehaviour {
	[SerializeField]
	private bool bRankingReflect = true;                                // ランキングを反映させるか否か(デバッグ用)
	public List<float> RankingTime = new List<float>();                 // ランキングタイム(リスト)
	public Image[] m_NumberImage = new Image[18];                       // 数字用イメージ
	public Sprite[] m_NumberSprite = new Sprite[10];                    // 数字スプライト
    public bool bRaceDataSave;                                          // データを更新するか否か(新規タイムが3位以内でtrue)
    [SerializeField]
    private RectTransform[] RankingRectTransform = new RectTransform[3];// ランキングタイムの位置
    [SerializeField]
    private RectTransform m_NewRankingRectTransform;                    // 新規タイムの位置
    private int[] RankingPosition = new int[4]{ 180, 60, -50, -155 };   // ランキングのY座標のオフセット
    private float[] RankingScale = new float[4] {3.5f,3.2f,3.0f,2.8f }; // ランキングのスケールのオフセット
    private Vector2[] RankingTargetVector = new Vector2[4];             // ランキング移動後の位置格納用
    private float[] RankingTargetScale = new float[4];                  // ランキング移動後の拡大率格納用
    private int m_NewRanking;                                           // 新規タイムのランキング(0～3)
    private float m_ElapsedTime = 0;                                    // 経過時間
    [SerializeField]
    private string RankingDataFileName;                                 // ランキングデータ格納用ファイルネーム
    private bool PlayGoodSE;                                            // ランキング更新時SEネーム
    private bool PlayBadSE;                                             // ランキング非更新時SEネーム

    private const float MAX_TIME = 5999.99f;                            // タイマーの最大値
    // Use this for initialization
    void Start () {
		LoadRanking();
		for(int i = 0; i < 3; i++) {
			int Minutes = (int)RankingTime[i] / 60;
			int Second = (int)(RankingTime[i] - Minutes * 60);
			int DecimalPlaces = (int)Mathf.RoundToInt(((RankingTime[i] - (Minutes * 60 + Second)) * 100));
			m_NumberImage[0+6*i].sprite = m_NumberSprite[Minutes / 10];
			m_NumberImage[1+6*i].sprite = m_NumberSprite[Minutes % 10];
			m_NumberImage[2+6*i].sprite = m_NumberSprite[Second / 10];
			m_NumberImage[3+6*i].sprite = m_NumberSprite[Second % 10];
			m_NumberImage[4+6*i].sprite = m_NumberSprite[DecimalPlaces / 10];
			m_NumberImage[5+6*i].sprite = m_NumberSprite[DecimalPlaces % 10];
		}
        m_ElapsedTime = 0;
        PlayBadSE = false;
        PlayGoodSE = false;
    }

    /// <summary>
    /// ランキング更新
    /// </summary>
    /// <param name="NewTime">更新タイム</param>
	public void RankingUpdate(float NewTime) {
		if(bRankingReflect) {
			int i;
			for(i = 0; i < 3; ++i) {
				if(NewTime < RankingTime[i]) {
					RankingTime.Insert(i, NewTime);
                    if(i == 0) {
                        bRaceDataSave = true;
                    }
			        RankingTime.Remove(3);
					break;
				}
			}
            m_NewRanking = i;
            if(m_NewRanking < 3) {
                PlayGoodSE = true;
            }else {
                PlayBadSE = true;
            }

            FileInfo rankingFile = new FileInfo("Data/" + RankingDataFileName + ".bin");
			StreamWriter streamWriter;
			streamWriter = rankingFile.CreateText();
			for(i = 0; i < 3; i++) {
				streamWriter.WriteLine(RankingTime[i]);
			}
			streamWriter.Flush();
			streamWriter.Close();
		}
	}

    /// <summary>
    /// ランキングデータの読み込み
    /// ファイルがなければ生成
    /// </summary>
	void LoadRanking() {
        string FileName = "Data/"+RankingDataFileName+".bin";
        if(!File.Exists(FileName)) {
            FileStream fileStream = File.Create("Data/"+ RankingDataFileName + ".bin");
            fileStream.Close();
            FileInfo saveFile = new FileInfo("Data/" + RankingDataFileName + ".bin");
            StreamWriter streamWriter;
            streamWriter = saveFile.CreateText();
            for(int i = 0; i < 3; i++) {
                streamWriter.WriteLine(MAX_TIME);
                RankingTime[i] = MAX_TIME;
            }
            streamWriter.Flush();
            streamWriter.Close();
        } else {
            FileInfo rankingFile = new FileInfo("Data/" + RankingDataFileName + ".bin");
            StreamReader streamReader = new StreamReader(rankingFile.OpenRead());
            for(int i = 0; i < 3; i++) {
                RankingTime[i] = float.Parse(streamReader.ReadLine());
            }
            streamReader.Close();
        }
	}

    /// <summary>
    /// ランキングの描画
    /// </summary>
	public void ShowRanking() {
        int i;
        m_NewRankingRectTransform.anchoredPosition = new Vector2(0, RankingPosition[0]);
        m_NewRankingRectTransform.localScale = new Vector2(RankingScale[0], RankingScale[0]);
        for(i = 0; i < 3; i++) {
            RankingTargetVector[i] = RankingRectTransform[i].anchoredPosition = new Vector2(0, RankingPosition[i + 1]);
            RankingRectTransform[i].localScale = new Vector2(RankingScale[i + 1], RankingScale[i + 1]);
            RankingTargetScale[i] = RankingRectTransform[i].localScale.x;
        }
        
        RankingTargetVector[3] = new Vector2(0, RankingPosition[0]);
        RankingTargetScale[3] = RankingScale[0];
    }

    /// <summary>
    /// ランキングの位置の変更
    /// </summary>
    public void ChangeRankingPosition() {
        int RankingCorrection = 0;
        float Scale;
        if(PlayGoodSE) {
            SoundManager.Instance.PlaySE("Good");
            PlayGoodSE = false;
        }
        if(PlayBadSE) {
            SoundManager.Instance.PlaySE("Bad");
            PlayBadSE = false;
        }
        m_ElapsedTime += Time.deltaTime / 2.0f;
        m_NewRankingRectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp( RankingTargetVector[3].y,RankingPosition[m_NewRanking], m_ElapsedTime));
        Scale = Mathf.Lerp(RankingTargetScale[3], RankingScale[m_NewRanking], m_ElapsedTime);
        m_NewRankingRectTransform.localScale = new Vector2(Scale, Scale);
        for(int i = 0; i < 3; i++) {
            if(i >= m_NewRanking) {
                RankingCorrection = 1;
            } else {
                RankingCorrection = 0;
            }
            RankingRectTransform[i].anchoredPosition = new Vector2(0, Mathf.Lerp(RankingTargetVector[i].y, RankingPosition[i + RankingCorrection],m_ElapsedTime));
            Scale = Mathf.Lerp(RankingTargetScale[i], RankingScale[i + RankingCorrection], m_ElapsedTime);
            RankingRectTransform[i].localScale = new Vector2(Scale, Scale);
        }
    }
}
