using UnityEngine;
using System.Collections;

//ゴーストとぶつかったら飛ばすよう
public class GhostPush : MonoBehaviour {

    public float fPower = 4.0f; //飛ばす威力
    SpiritStatus m_Size;

    void Start()
    {
        m_Size = GetComponent<SpiritStatus>();
    }

    ////衝突判定
    //void OnCollisionEnter(Collision collision)
    //{
    //    //プレイヤー以外の物とサイズが大きい物は処理を行わない
    //    if (collision.gameObject.tag != "Player" || 
    //        m_Size.m_Size <= collision.gameObject.GetComponent<KatamariStatus>().m_Size) return;

    //    //ゴーストの向いている方向に飛ばす
    //    collision.gameObject.GetComponent<PlayerMove>().BoundSet(transform.forward, fPower);

    //}
}
