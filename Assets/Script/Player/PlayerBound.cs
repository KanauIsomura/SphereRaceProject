using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーバウンドさせたいクラス
/// </summary>
public class PlayerBound : MonoBehaviour {

    public float        Coefficient = 0.5f;        //バウンドの係数
    public float        MinPower = 3.0f;           //バウンドの最低威力
    public float        SpiritPower = 5.0f;        //自分より大きい物とぶつかった時の最低威力
    PlayerMove          PlayerDirection;
    KatamariStatus      KatamariSize;              //塊のサイズ
    

    /// <summary>
    /// スタート関数
    /// </summary>
	void Start () 
    {
        PlayerDirection = GetComponent<PlayerMove>();
        KatamariSize = GetComponent<KatamariStatus>();
	}
	

    /// <summary>
    /// 衝突判定  ※デバックログ禁止!!!　　←こいつ動いている時しか入らねえ
    /// </summary>
    /// <param name="hit"></param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //塊にくっ付くオブジェクトは処理を行わない
        if (hit.gameObject.tag == "Player" ||
            hit.gameObject.tag == "Spirit" &&
            KatamariSize.KatamariSize >= hit.gameObject.GetComponent<SpiritStatus>().m_Size &&
            PlayerDirection.BoundSpeed != 0.0f)
        {
            return;
        }

        //角度計算用のベクトル設定
        Vector3 vtemp = hit.normal;
        vtemp.y = 0.0f; //Y成分削除


        //最低バウンド威力設定
        float fminPower;
        if (hit.gameObject.tag == "Spirit")
            fminPower = SpiritPower;
        else
            fminPower = MinPower;

        //角度の計算
        float fWallAngle = 180 - (90 + Vector3.Angle(vtemp, hit.normal));

        //登れない角度なら判定しない
        if (PlayerDirection.SlopeLimit > fWallAngle && hit.gameObject.tag != "Spirit")
            return;

        //飛ばす方向設定
        Vector3 vFrom = hit.normal;
        vFrom.y = 0.0f;   //Y成分削除

        //バウンドの威力設定
        float BoundPower;
        if (PlayerDirection.PlayerSpeed * Coefficient > fminPower)
            BoundPower = PlayerDirection.PlayerSpeed * Coefficient;
        else
            BoundPower = fminPower;


        //抵抗値計算
        Vector3 ResistanceValue;
        ResistanceValue = PlayerDirection.PlayerDirection + hit.normal.normalized;
        //PlayerDirection.PlayerDirection;
        //Debug.Log("hit" + hit.normal);
        //Debug.Log("Player" + PlayerDirection.PlayerDirection);
        //Debug.Log("ResistanceValue" + ResistanceValue);

        //バウンドさせる
        //PlayerDirection.BoundSet(vFrom, BoundPower, Mathf.Abs(ResistanceValue.z), Mathf.Abs(ResistanceValue.x));
    }


    //プレイヤーが止まってる時用の衝突処理
    void OnCollisionEnter(Collision collision)
    {
        //塊にくっ付くオブジェクトは処理を行わない＆動いていたら処理を行わない
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Spirit" &&
            PlayerDirection.PlayerDirection != Vector3.zero &&
            KatamariSize.KatamariSize >= collision.gameObject.GetComponent<SpiritStatus>().m_Size)
        {
            return;
        }


        //バウンドさせる
        Vector3 vFrom;

        //方向設定
        vFrom = collision.gameObject.transform.forward;
        //バウンドの威力設定
        float BoundPower = SpiritPower;

        Vector3 ResistanceValue;
        ResistanceValue = PlayerDirection.PlayerDirection + collision.gameObject.transform.forward;
       
        //バウンドさせる
       // PlayerDirection.BoundSet(vFrom, BoundPower, ResistanceValue.x, ResistanceValue.z);
    }
}
