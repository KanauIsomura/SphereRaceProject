using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 土煙設定
/// </summary>
public class DashEffect : MonoBehaviour {
   
    public Terrain          m_StageData;             //ステージのTerrain
    public string[]         m_TexName;               //テクスチャファイル名
    public ParticleSystem[] m_EffectData;            //パーティクルデータ
    public float            m_Rate = 0.4f;           //表示を始める割合
    public float            m_StartSpeed = 5.0f;     //表示を始める速度  
    public float            m_GroundDistance = 3.0f; //地面までの距離       


    //パーティクル管理用map
    Dictionary<string,ParticleSystem>     m_MapData = new Dictionary<string,ParticleSystem>();
    SplatPrototype[]    m_TexData;       //Terrainで使用しているテクスチャ格納用
    float               m_fHeightRatio;  //高さの割合
    float               m_fWidthRatio;   //幅の割合
    CharacterController m_CharaCon;      //接地判定用
    PlayerMove          m_PlayerSpeed;   //速度取得用        
    StartProduction     m_StartFlg;      //スタート判定用
	
    // Use this for initialization
	void Start () {    
        //マップに格納
        for(int nCount = 0; nCount < m_TexName.Length; nCount ++) {
            m_MapData.Add(m_TexName[nCount],m_EffectData[nCount]);
        }

        //Terrainに使用しているテクスチャデータを取得
        m_TexData = m_StageData.terrainData.splatPrototypes;

        //縦横の割合計算
        m_fHeightRatio = m_StageData.terrainData.alphamapTextures[0].height / m_StageData.terrainData.size.x;
        m_fWidthRatio  = m_StageData.terrainData.alphamapTextures[0].width  / m_StageData.terrainData.size.z;

        //キャラクターコントローラー取得
        m_CharaCon = gameObject.GetComponent<CharacterController>();
        m_PlayerSpeed = gameObject.GetComponent<PlayerMove>(); 
        
        //スタート判定取得
        GameObject StartProduction = GameObject.Find("StartProduction");
        if (StartProduction == null)
        {
            Debug.Log("StartProductionが見つかりません。");
            m_StartFlg = null;
        }
        else
            m_StartFlg = StartProduction.GetComponent<StartProduction>();

	}


	
    /// <summary>
    /// 更新処理
    /// </summary>
	void Update () {

        //スタート判定
        if (m_StartFlg == null || m_StartFlg.isStart == false) return;


        //プレイヤーの速度が開始以下で空中にいたら止める
        if (Mathf.Abs(m_PlayerSpeed.PlayerSpeed) < m_StartSpeed || 
            Physics.Raycast(transform.position, -transform.up, m_GroundDistance + m_CharaCon.radius) == false)
        {
            AllStopEffect();
            return;
        }


        //ピクセル判別用にテクスチャ取得
        Texture2D[] AlphaMapTex = m_StageData.terrainData.alphamapTextures;  


        //アルファテクスチャごとにピクセルを取得する
        for (int nATexCount = 0; nATexCount < AlphaMapTex.Length; nATexCount++)
        {
            //ピクセルデータ取得
            Color PixelData = AlphaMapTex[nATexCount].GetPixel((int)(transform.position.x * m_fWidthRatio), (int)(transform.position.z * m_fHeightRatio));
            //Debug.Log(PixelData);
            //Debug.Log("幅" + AlphaMapTex[nATexCount].width);
            //Debug.Log("高さ" + AlphaMapTex[nATexCount].height);
            //Debug.Log("左下" + AlphaMapTex[nATexCount].GetPixel(0,0));
            //Debug.Log("右下" + AlphaMapTex[nATexCount].GetPixel(AlphaMapTex[nATexCount].width, 0));
            //Debug.Log("左上" + AlphaMapTex[nATexCount].GetPixel(0, AlphaMapTex[nATexCount].height));
            //Debug.Log("右上" + AlphaMapTex[nATexCount].GetPixel(AlphaMapTex[nATexCount].width, AlphaMapTex[nATexCount].height));

            //表示切替
            SetEffect(0 + nATexCount * 4, PixelData.r); //1
            SetEffect(1 + nATexCount * 4, PixelData.g); //2
            SetEffect(2 + nATexCount * 4, PixelData.b); //3
            SetEffect(3 + nATexCount * 4, PixelData.a); //4
        }
	}




    /// <summary>
    /// パーティクルの表示/非表示切替
    /// </summary>
    /// <param name="Count"> 使用テクスチャカウント </param>
    /// <param name="Pixel"> テクスチャの仕様割合 </param>
    void SetEffect(int Count, float Pixel)
    {
        //要素数より大きい数が来たら処理しない
        if (m_TexData.Length <= Count) return;

        //テクスチャのファイル名を取得
        string TexName = m_TexData[Count].texture.name;

        //表示切替
        if (Pixel >= m_Rate)
        {//表示
            if (m_MapData[TexName].isPlaying == false)
                m_MapData[TexName].Play();
        }
        else
        {//非表示
            if (m_MapData[TexName].isPlaying == true)
                m_MapData[TexName].Stop();
        }
    }



    /// <summary>
    /// マップ内のパーティクルを全て止める
    /// </summary>
    void AllStopEffect()
    {
        //パーティクルを全て止める
        for (int Count = 0; Count < m_TexData.Length; Count++)
        {
            string TexName = m_TexData[Count].texture.name;

            if (m_MapData[TexName].isPlaying == true)
                m_MapData[TexName].Stop();
        }
    }
}
