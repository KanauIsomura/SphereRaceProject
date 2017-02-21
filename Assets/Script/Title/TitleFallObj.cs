using UnityEngine;
using System.Collections;

public class TitleFallObj : MonoBehaviour {
   
    [Header("生存時間")]
    public float fLifeTime = 5.0f;

    [Header("加える力")]
    public Vector3 AddPow;

    [Header("X方向にランダムに加える力の範囲")]
    public Vector2 XPowRange;

    private Rigidbody rb;   // Rigidbody制御用変数

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();     // オブジェクトのRigidbody取得
        float fRandDirect = Random.Range(-1,1);     // X方向にかける力の方向をランダムで制御
        float fRandPow = Random.Range(XPowRange.x, XPowRange.y);    // X方向にかける力をランダムで制御
        rb.AddForce(new Vector3(fRandPow * fRandDirect, AddPow.y, AddPow.z)); // Rigidbodyに力を加える
	}
	
	// Update is called once per frame
	void Update () {
        fLifeTime -= Time.deltaTime;    // 時間を経過

        if (fLifeTime < 0)  // 生存時間終了
        {
            Destroy(gameObject);    // 削除
        }
    }
}
