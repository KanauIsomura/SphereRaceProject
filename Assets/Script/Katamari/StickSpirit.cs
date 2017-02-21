using UnityEngine;
using System.Collections;

/// <summary>
/// ぶつかったゲームオブジェクトをくっつける処理
/// </summary>
public class StickSpirit : MonoBehaviour {
	public string				m_StickTagName;		//くっつけたいオブジェクトのタグネーム
	public GameObject			m_StickSpiritList;		//くっつけにいくオブジェクト
	public KatamariStatus		m_KatamariStatus;		//塊のステータス
	public CameraController	m_CameraController;	//カメラコントローラースクリプト
	public GameObject			m_GlitterEffect;

	float m_OldKatamariSize;    //一つ前の塊の大きさ

	// Use this for initialization
	void Start () {
		m_OldKatamariSize = GetComponent<KatamariStatus>().KatamariSize;
	}
	
	// Update is called once per frame
	void Update () {
			transform.localRotation =
				new Quaternion(0.0f, transform.localRotation.y, 0.0f, transform.localRotation.w);
	}

	/// <summary>
	/// 当たった瞬間の処理
	/// </summary>
	/// <param name="OtherCollider">当たった対象のコライダー</param>
	//void OnCollisionEnter(Collision OtherCollider){
	void OnControllerColliderHit(ControllerColliderHit OtherCollider){
		//設定したタグとぶつかった場合くっつける
		if(OtherCollider.gameObject.CompareTag(m_StickTagName)) {
			KatamariStatus		MyKatamariStatus	= GetComponent<KatamariStatus>();						//塊のステータス
			CharacterController	MayCharaCon		= GetComponent<CharacterController>();					//自分のキャラコン

			GameObject			OtherObject		= OtherCollider.gameObject;
			SpiritStatus			OtherSpritStatus	= OtherCollider.gameObject.GetComponent<SpiritStatus>();	//スピリットのステータス

			//塊よりくっつけるものが大きい場合
			if(MyKatamariStatus.KatamariSize < OtherSpritStatus.m_Size)
				return;

			//Rigidbodyを親だけにしたいのでくっ付けるオブジェクトのRigidbodyを消す
			Destroy(OtherObject.GetComponent<Rigidbody>());

			//相手の親を自分にする
			OtherObject.transform.parent = m_StickSpiritList.transform;

			//くっつけたオブジェクトのタグとレイヤーを変更する
			OtherObject.layer	= LayerMask.NameToLayer("Katamari");
			OtherObject.tag	= "Player";

			//塊の最適化をスタートさせる
			OtherObject.GetComponent<SpiritOptimaize>().OptimaizeTime = 0.0f;
			OtherObject.GetComponent<SpiritOptimaize>().StartOptimaize(MayCharaCon.radius);

			//くっつけるオブジェクトが塊の凹凸に影響を与えるか
			//if(!OtherSpritStatus.m_Convex) OtherCollider.collider.enabled = false;	//与えない場合当たり判定を消す

			//塊が大きくなった分当たり判定を大きく
			MyKatamariStatus.KatamariSize	+= OtherSpritStatus.m_Size * 0.1f;
			MayCharaCon.radius				= (int)MyKatamariStatus.KatamariSize / 1 * 0.1f + 0.1f;

			//塊のサイズが1の倍数になったらエフェクトをだす
			if(m_OldKatamariSize % 1 + (MyKatamariStatus.KatamariSize - m_OldKatamariSize) % 1 >= 1.0f) {
				m_GlitterEffect.SendMessage("StartGlitter");
			}

			//古い大きさとして保存しておく
			m_OldKatamariSize = MyKatamariStatus.KatamariSize;

			//カメラの距離を変える
			m_CameraController.ChangeOffset((int)MyKatamariStatus.KatamariSize);

			SoundManager.Instance.PlaySE("cling");
		}
	}
}
