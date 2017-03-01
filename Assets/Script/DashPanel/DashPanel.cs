using UnityEngine;
using System.Collections;

public class DashPanel : MonoBehaviour {

    public float fDashSpeed;    //加速速度

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerMove PlayerSpeed = col.GetComponent<PlayerMove>();
            PlayerSpeed.PlayerSpeed = PlayerSpeed.PlayerSpeed + fDashSpeed;
        }
    }
}
