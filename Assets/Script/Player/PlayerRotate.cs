using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤー回転
/// </summary>
public class PlayerRotate : MonoBehaviour {

    public float fAddAngle = 5.0f;  //加算回転角度
    private PlayerMove m_PlayerMove;    //速度取得用
    private StartProduction StartFlg;   //スタート判定用

    /// <summary>
    /// スタート関数
    /// </summary>
	void Start () {
        m_PlayerMove = gameObject.GetComponent<PlayerMove>(); 
        StartFlg = GameObject.Find("StartProduction").GetComponent<StartProduction>();
    
	}
	
    /// <summary>
    /// 更新関数
    /// </summary>
	void Update ()
    {
        //===============================
        // スタート判定
        //===============================
        if (StartFlg.isStart == false) return;

        //スティックの入力値取得
        float fRightHorizontal = MultiInput.Instance.GetRightStickAxis().x;
	    
        //回転
        //if (m_PlayerMove.VerticalSpeed != 0 || m_PlayerMove.SideSpeed != 0)
        //    transform.Rotate(transform.up, fRightHorizontal * fAddAngle * Time.deltaTime);
    }

}
