using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiritManager : MonoBehaviour {
	public List<GameObject> SpiritList;		//スピリットオブジェクトのリスト

	// Use this for initialization
	void Start () {
		//自分の子にいるスピリットオブジェクトをリストに格納していく
		foreach(Transform Children in transform){
			SpiritList.Add(Children.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
