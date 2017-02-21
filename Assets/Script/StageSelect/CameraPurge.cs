using UnityEngine;
using System.Collections;

public class CameraPurge : MonoBehaviour {

    public StageSelectPlayerMove SelectFlg;
	
	// Update is called once per frame
	void Update () {
        if (SelectFlg.SelectFlg == true)
            transform.parent = null;
	}
}
