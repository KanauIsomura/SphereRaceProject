using UnityEngine;
using System.Collections;

public class TitleFallObjManager : MonoBehaviour {
    
    static TitleFallObjManager _TitleFallObjManager;

    [Header("落ち始める時間")]
    public float m_fFallStartTime;

    [Header("落ちるオブジェクトリスト")]
    public GameObject[] m_FallObjList;

    [Header("落ちるものが発生するタイミング")]
    public float m_fFallTime;

    [Header("どこのX地点に発生させるかの最大値、最低値")]
    public Vector2 MinMaxPosX;

    [Header("オブジェクトを1度に生成する最大数"),Range(1,10)]
    public int MaxObjNum;

    [Header("ムービー")]
    public TitleMovie movie;

    private float StartPosY = 13;

    private float m_fTime;  // 経過時間管理用
    

    // インスタンス取得
    public static TitleFallObjManager GetInstance()
    {
        return _TitleFallObjManager ?? (_TitleFallObjManager = new TitleFallObjManager());
    }

    // Use this for initialization
    void Start () {
        m_fTime = 0;
    }
	
	// Update is called once per frame
    //void Update () {
    //    if (movie.GetMovieFlg())    // 動画再生中なら更新しない
    //        return;

    //    DropTime(Random.Range(0, MaxObjNum));
    //}

    // 時間経過で発生させる
    void DropTime(int nObjNum)  // 引数:落とす数
    {
        m_fTime += Time.deltaTime;  // 時間を経過

        if (m_fFallStartTime > m_fTime) // 落ち始める時間が過ぎたら
            return;

        m_fFallStartTime = 0;   // 1度でも入ったら↑で止まらない

        if (m_fTime < m_fFallTime)  // 落ちる物が発生する時間が過ぎたら
            return;

            GameObject Clone = null;

        for (int nCnt = 0; nCnt < nObjNum; nCnt++)
        {
            int nRand = Random.Range(0, m_FallObjList.Length);
            Clone = (GameObject)Instantiate(m_FallObjList[nRand],
                    new Vector3(Random.Range(MinMaxPosX.x, MinMaxPosX.y), StartPosY , m_FallObjList[nRand].transform.position.z),
                    m_FallObjList[nRand].transform.rotation);   // 落ちるオブジェクトを生成
            Clone.transform.parent = transform;
        }


        m_fTime = 0;    // 経過時間をリセット
    }
}
