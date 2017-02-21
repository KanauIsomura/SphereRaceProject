using UnityEngine;
using System.Collections;

public class SppedEffectObject : MonoBehaviour {

    private PlayerMove PlayerSpeed;     //プレイヤー速度取得用

	// Use this for initialization
    void Start()
    {
        PlayerSpeed = GameObject.Find("Player").GetComponent<PlayerMove>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.LookRotation(PlayerSpeed.PlayerDirection.normalized);
	}
}
