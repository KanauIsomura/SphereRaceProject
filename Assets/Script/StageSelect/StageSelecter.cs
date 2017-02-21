using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageSelecter : MonoBehaviour {
	public string SceneName;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision obj) {
        if (obj.gameObject.tag != "Player" )
            return;

        //くっつくSEを鳴らす
        SoundManager.Instance.PlaySE("cling");

        //塊の子に設定
        transform.parent = GameObject.Find("katamari").transform;
        GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("SceneChanger").GetComponent<SceneChanger>().SetNextScene(SceneName);
	}
}
