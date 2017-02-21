using UnityEngine;
using System.Collections;

/// <summary>
/// 指定した個数のくっついたオブジェクトを取り外すオブジェクト
/// </summary>
public class PurgeSpirit : MonoBehaviour {
	public GameObject m_SpiritObjectList;
	public GameObject m_SpiritManager;
	public int		m_PurgeNum = 3;
	public float		m_ForcePower = 50.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(MultiInput.Instance.GetTriggerButton(MultiInput.CONTROLLER_BUTTON.CIRCLE)){
			Purge();
		}
	}

	void Purge(){
		int PurgeCount = 0;

		//くっつけたオブジェクトリストを探索
		foreach(Transform Childlen in m_SpiritObjectList.transform) {
			//外すオブジェクトにRigidbodyを作ってAddForceをかける
			Rigidbody ChidlenRigid = Childlen.gameObject.AddComponent<Rigidbody>();
			Childlen.LookAt(new Vector3(transform.position.x, transform.position.y - 3.0f, transform.position.z));//プレイヤーの方向に向ける
			ChidlenRigid.AddForce(Childlen.forward * m_ForcePower, ForceMode.Acceleration);

			//オブジェクトのタグとレイヤーを変更
			//Childlen.gameObject.layer = LayerMask.NameToLayer("Spirit");
			//Childlen.gameObject.tag = "Spirit";
			Childlen.gameObject.GetComponent<Collider>().enabled = false;

			//親を変更
			Childlen.parent = m_SpiritManager.transform;
			++PurgeCount;

			//外したオブジェクトが設定した個数に達したら
			//if(m_PurgeNum == PurgeCount++)
			//	break;
		}
	}
}
