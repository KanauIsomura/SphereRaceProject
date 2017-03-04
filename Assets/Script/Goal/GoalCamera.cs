//================================
/*
 取扱説明書
 watchObjには注目したいオブジェクトを入れてください。(プレイヤーだと思います。)
 制作者:佐々木瑞生
 */
//================================
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GoalCamera : MonoBehaviour {
	public float DistanceAway;
	public GameObject watchObj;
	public bool bGoal;
	public bool bCameraEnd;
	public float m_SizeMagnification = 1.0f;	// プレイヤーのサイズによる距離の倍率。
	public Vector3[] distanceDifference;	// プレイヤーとの距離のオフセット
	public Vector3 defaultPosition;

	private Transform objTrans;
	private Vector3 pos;
	private float theta;
	public float CameraChangeInterval;
	private float TimeCount;
	private int Mode;
	private CameraController cameraController;
	public bool bFadeStart;
	[SerializeField]
	private Image FadeImage;
	[SerializeField]
	private float FadeVolume;
    [SerializeField]
    private RaceRanking ranking;
    [SerializeField]
    private ReplayGhostManager replayGoastManager;
    [SerializeField]
    private string GoalBGMName;
    private bool isPlayGoalBGM;
    // Use this for initialization
    void Start () {
		theta = 0;
		bGoal = false;
		TimeCount = 0.0f;
		bCameraEnd = false;
		cameraController = GetComponent<CameraController>();
		var imageColor = FadeImage.color;
		imageColor.a = 0;
		FadeImage.color = imageColor;
		Mode = 0;
        isPlayGoalBGM = true;
    }
	
	// Update is called once per frame
	void Update () {
		// goalしたらカメラ位置を切り替える。
		if(!bGoal) return;
		if(bFadeStart) {
			GoalFadeOut();
		} else if(!bCameraEnd && !bFadeStart) {
			TimeCount -= Time.unscaledDeltaTime;
			GoalFadeIn();
			if(TimeCount < 0.0f) {
				if(Mode >= distanceDifference.Length) {
					bCameraEnd = true;
					return;
				}
				bFadeStart = true;
				Mode++;
			}
		}

		if(bCameraEnd) {
			GoalFadeIn();
            if(ranking.bRaceDataSave) {
                replayGoastManager.SendMessage("SeveGhost");
                ranking.bRaceDataSave = false;
            }
            if(isPlayGoalBGM) {
                isPlayGoalBGM = false;
                SoundManager.Instance.PlayBGM(GoalBGMName);
            }
			theta += Time.unscaledDeltaTime;
			var thisPos = transform.position;
			thisPos.x = watchObj.transform.position.x + DistanceAway * Mathf.Sin(theta);
			thisPos.y = watchObj.transform.position.y;
			thisPos.z = watchObj.transform.position.z + DistanceAway * Mathf.Cos(theta);
			transform.position = thisPos;
			transform.LookAt(watchObj.transform);   // オブジェクトのほうを見る
		}
	}
	/// <summary>
	/// カメラ位置の切り替え
	/// </summary>
	/// <param name="nMode">カメラ位置のパターン</param>
	private void ChangeCamera(int nMode = 0) {
        SoundManager.Instance.PlaySE("shutter");
        objTrans = watchObj.transform;
		pos = watchObj.transform.position;
		Vector3 cameraRote;
		Debug.Log(Mode);
		cameraRote.x = DistanceAway * distanceDifference[nMode].x;
		cameraRote.y = DistanceAway * distanceDifference[nMode].y;
		cameraRote.z = DistanceAway * distanceDifference[nMode].z;
		transform.position = pos+cameraRote;
		transform.LookAt(watchObj.transform);
		cameraController.enabled = false;
	}

	/// <summary>
	/// カメラ位置のリセット
	/// </summary>
	public void ResetCamera() {
		objTrans = watchObj.transform;
		pos = watchObj.transform.position;
		Vector3 diff = new Vector3(0, 6, 4);
		Time.timeScale = 1.0f;
		float x = objTrans.forward.x * diff.x;
		float y = objTrans.forward.y * diff.y;
		float z = objTrans.forward.z * diff.z;
		var cameraPos = new Vector3(x, y, z);

		transform.position = pos - cameraPos;
	}

	/// <summary>
	/// ゴールフェードアウト
	/// </summary>
	private void GoalFadeOut() {
		var imageColor = FadeImage.color;
		imageColor.a += (FadeVolume/255) * Time.unscaledDeltaTime;
		if(imageColor.a > 1.0f) {
			imageColor.a = 1.0f;
			FadeImage.color = imageColor;
			bFadeStart = false;
			if(Mode >= distanceDifference.Length)
				return;
			ChangeCamera(Mode);
			TimeCount = CameraChangeInterval;
			return;
		}
		FadeImage.color = imageColor;
	}

	/// <summary>
	/// ゴールフェードイン
	/// </summary>
	private void GoalFadeIn() {
		var imageColor = FadeImage.color;
		imageColor.a -= (FadeVolume/255) * Time.unscaledDeltaTime;
		if(imageColor.a < 0) {
			imageColor.a = 0;
		}
		FadeImage.color = imageColor;
	}
}
