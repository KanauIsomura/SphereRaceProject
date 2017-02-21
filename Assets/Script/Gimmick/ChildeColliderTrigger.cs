using UnityEngine;
using System.Collections;

public class ChildeColliderTrigger : MonoBehaviour {
    
    //メッセージを送る対象
    GameObject parent;

	// Use this for initialization
	void Start () {
        parent = gameObject.transform.parent.gameObject;
	}
	
    //プレイヤー発見時
	void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            parent.SendMessage("Locate", collider);
        }
    }

    //プレイヤーを見失ったら
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            parent.SendMessage("LoseLocate", collider);
        }
    }
}
