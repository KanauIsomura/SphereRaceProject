using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Concentration : MonoBehaviour {
	public float m_OriginSpeed = 10.0f;		//エフェクトが発生する速さ
	public PlayerMove PlayerMoveScript;	
	
	Image EffectImage;					//エフェクトの画像
	Color ImageColor;						//エフェクトの色

	// Use this for initialization
	void Start () {
		//初期の色を添付
		EffectImage = GetComponent<Image>();
		ImageColor = EffectImage.color;
	}
	
	// Update is called once per frame
	void Update() {
		//スピードが一定量を超えていたらエフェクトを出す
			EffectImage.color = new Color(
				ImageColor.r, ImageColor.g, ImageColor.b, PlayerMoveScript.PlayerSpeed / PlayerMoveScript.PlayerMaxSpeed);
	}
}
