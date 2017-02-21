using UnityEngine;
using System.Collections;

public class GhostCarWipe : MonoBehaviour {
	[SerializeField]
	WipeCamera WipeCameraScript;
	[SerializeField]
	string LockCameraName;		//見られるカメラ
	[SerializeField]
	float NotShowTime	= 1.0f; //この時間分写ってないと写ってないことに
	[SerializeField]
	float ShowDistance = 300.0f;		//この距離以内なら写っている

	float CameraInStartTime;        //カメラ内に入ったときに時間
	bool bShow;

	Vector3 MainCameraPosition;

	// Use this for initialization
	void Start () {
		bShow = false;
		MainCameraPosition = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//一定時間カメラに写っていない
		//if(Time.time - CameraInStartTime > NotShowTime) {
		//}
		////一定距離離れている
		//if(Vector3.Magnitude(transform.position - MainCameraPosition) > ShowDistance){
		//    if(!bShow) {
		//        bShow = true;
		//        WipeCameraScript.SetWipeFlag(true);
		//    }

		//}
		//一定時間カメラに収まっていて、一定距離近い場合
		if(Time.time - CameraInStartTime < NotShowTime &&
			Vector3.Magnitude(transform.position - MainCameraPosition) < ShowDistance) {
			if(bShow) {
				bShow = false;
				WipeCameraScript.SetWipeFlag(false);
			}
		}else{
			if(!bShow) {
				bShow = true;
				WipeCameraScript.SetWipeFlag(true);
			}
		}
	}

	void OnWillRenderObject() {
		if(Camera.current.name == LockCameraName) {
			CameraInStartTime = Time.time;
			MainCameraPosition = Camera.current.transform.position;
		}
	}
}
