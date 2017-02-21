using UnityEngine;
using System.Collections;

public class WipeCamera : MonoBehaviour {
	[SerializeField]
	Vector2 WipePosition;		//ワイプの表示場所
	[SerializeField]
	Vector2 OutWipePosition;    //ワイプをしまっておく座標
	[SerializeField]
	float WipeTime = 3.0f;		//ワイプが表示される時間

	StartProduction StartScript;	//スタートスクリプトへの参照

	Camera Wipe;
	float WipeStartTime;
	bool bWipe;
	Rect WipeRect;

	// Use this for initialization
	void Start () {
		StartScript = GameObject.Find("StartProduction").GetComponent<StartProduction>();
		Wipe = GetComponent<Camera>();
		WipeRect = Wipe.rect;
		WipeStartTime = 999999;
		bWipe = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!StartScript.isStart){
			return;
		}
		Vector2 NowWipePos;
		if(bWipe) {
			NowWipePos = Vector2.Lerp(OutWipePosition, WipePosition, 
				Mathf.Clamp((Time.time - WipeStartTime) / WipeTime, 0.0f, 1.0f));
		} else {
			NowWipePos = Vector2.Lerp(WipePosition, OutWipePosition, 
				Mathf.Clamp((Time.time - WipeStartTime) / WipeTime, 0.0f, 1.0f));
		}
		WipeRect.position = NowWipePos;
		Wipe.rect = WipeRect;
	}

	/// <summary>
	/// ワイプのイン・アウト関数
	/// </summary>
	/// <param name="bWipeFlag">trueでワイプがIn、Falseでワイプアウト</param>
	public void SetWipeFlag(bool bWipeFlag) {
		if(bWipeFlag) {
			if(!bWipe) {//ワイプイン
				bWipe = true;
				WipeStartTime = Time.time;
				SoundManager.Instance.PlaySE("puu71_b");
			}
		}else {
			if(bWipe) {//ワイプアウト
				bWipe = false;
				WipeStartTime = Time.time;
				SoundManager.Instance.PlaySE("puu72_a");
			}

		}
	}
}
