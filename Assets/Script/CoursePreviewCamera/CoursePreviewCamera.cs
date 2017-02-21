using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoursePreviewCamera : MonoBehaviour {
	public Vector3[]    m_StartPos;
	public Vector3[]    m_EndPos;
	public Vector3[]    m_StartVector;
	public Vector3[]    m_EndVector;
	public Vector3[]    m_StartRote;
	public Vector3[]    m_EndRote;
	public float[]      m_MoveTime;
	private int         m_NowMoveNumber;

	private Vector3    m_TurningStartPos;
	private Vector3    m_TurningEndPos;
	private Vector3    m_TurningControlPoint;
	private float      m_TurningMoveTime;

	public GameObject watchObj;
	public bool bStart;
	public bool bCameraEnd;
	public Vector3 defaultPosition;

	private Transform objTrans;
	private Vector3 pos;
	private float theta;
	private float TimeCount;
	private int Mode;
	public CameraController cameraController;
	public bool bFadeStart;
	[SerializeField]
	private Image FadeImage;
	[SerializeField]
	private float FadeVolume;
	public StartProduction startProduction;
	[SerializeField]
	private GameObject Camera;
	[SerializeField]
	private MultiInput.CONTROLLER_BUTTON skipButton;
    [SerializeField]
    private PauseCanvas pauseCanvas;
    [SerializeField]
    private GameObject DeleteCanvas;
    [SerializeField]
    private Canvas[] DoNotDeleteCanvas = new Canvas[2];
    [SerializeField]
    private Camera[] NotMainCameras = new Camera[2];
    // Use this for initialization
    void Start() {
		theta = 0;
		bStart = false;
		TimeCount = 0.0f;
		bCameraEnd = false;
		var imageColor = FadeImage.color;
		imageColor.a = 0;
		FadeImage.color = imageColor;
		Mode = 0;
		if(cameraController.enabled)
			cameraController.enabled = false;
		m_TurningStartPos = -watchObj.transform.right * 2;
		m_TurningEndPos = defaultPosition;
		m_TurningControlPoint = -watchObj.transform.forward * 2;
		m_TurningMoveTime = 3.0f;
        SoundManager.Instance.PlaySE("BeforeRace");
	}

	// Update is called once per frame
	void Update() {
		// goalしたらカメラ位置を切り替える。
		if(startProduction.isStart || startProduction.m_DoingCountDown) return;
        if(DeleteCanvas.activeSelf) {
            DeleteCanvas.SetActive(false);
            DoNotDeleteCanvas[0].enabled = false;
            DoNotDeleteCanvas[1].enabled = false;
            NotMainCameras[0].enabled = false;
            NotMainCameras[1].enabled = false;
        }

		if(bFadeStart) {
			GoalFadeOut();
		} else if(!bCameraEnd && !bFadeStart) {
            if(!pauseCanvas.isPausing) {
                if(MultiInput.Instance.GetPressButton(skipButton)) {
                    bCameraEnd = true;
                    bFadeStart = true;
                    return;
                }
            }

			if(Mode >= m_StartPos.Length) {
				bCameraEnd = true;
				return;
			}
			TimeCount += Time.deltaTime / m_MoveTime[Mode];
			GoalFadeIn();
			ChangeCamera(Mode);
			if(TimeCount >= 1.0f) {
				if(Mode >= m_StartPos.Length) {
					bCameraEnd = true;
					return;
				}
				bFadeStart = true;
				TimeCount = 0.0f;
				Mode++;
			}
		}

		// 全部終わったら
		if(bCameraEnd && !bFadeStart) {
			GoalFadeIn();
            SoundManager.Instance.FadeOutSE("BeforeRace", 0.3f);
            if(!DeleteCanvas.activeSelf) {
                DeleteCanvas.SetActive(true);
                DoNotDeleteCanvas[0].enabled = true;
                DoNotDeleteCanvas[1].enabled = true;
                NotMainCameras[0].enabled = true;
                NotMainCameras[1].enabled = true;
            }
            theta += Time.deltaTime / m_TurningMoveTime;
			BeziersCurvePoint(theta > 1.0f ? 1.0f : theta);
			Camera.transform.LookAt(watchObj.transform);   // オブジェクトのほうを見る
			var CameraRote = Camera.transform;
			CameraRote.eulerAngles = new Vector3(5, CameraRote.eulerAngles.y, CameraRote.eulerAngles.z);
			if(theta >= 1.3f) {
				startProduction.StartCountDown();
				cameraController.enabled = true;
			}
		}
	}
	/// <summary>
	/// カメラ位置の切り替え
	/// </summary>
	/// <param name="nMode">カメラ位置のパターン</param>
	private void ChangeCamera(int nMode = 0) {
		HermiteCurvePoints(nMode, TimeCount);
		//transform.LookAt(watchObj.transform);
	}

	/// <summary>
	/// エルミート曲線
	/// </summary>
	/// <param name="nMode">カメラモード</param>
	/// <param name="Time">経過時間</param>
	private void HermiteCurvePoints(int nMode, float Time) {
		//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
		//地味に長いエルミート計算群
		float h00, h01, h10, h11;
		float t;
		t = Time;
		h00 = ( 2 * t * t * t ) - ( 3 * t * t ) + 1;
		h01 = ( -2 * t * t * t ) + ( 3 * t * t );
		h10 = ( t * t * t ) - ( 2 * t * t ) + t;
		h11 = ( t * t * t ) - ( t * t );

		Vector3 Position = new Vector3(
			//X座標の計算
			h00 * m_StartPos[nMode].x + h01 * m_EndPos[nMode].x +
			h10 * m_StartVector[nMode].x + h11 * m_EndVector[nMode].x,
			//Y座標の計算
			h00 * m_StartPos[nMode].y + h01 * m_EndPos[nMode].y +
			h10 * m_StartVector[nMode].y + h11 * m_EndVector[nMode].y,
			//Z座標の計算
			h00 * m_StartPos[nMode].z + h01 * m_EndPos[nMode].z +
			h10 * m_StartVector[nMode].z + h11 * m_EndVector[nMode].z);

		Camera.transform.position = Position;  //求めた座標に移動する

		Camera.transform.eulerAngles = new Vector3(
			m_StartRote[nMode].x + ( m_EndRote[nMode].x - m_StartRote[nMode].x ) * t,
			m_StartRote[nMode].y + ( m_EndRote[nMode].y - m_StartRote[nMode].y ) * t,
			m_StartRote[nMode].z + ( m_EndRote[nMode].z - m_StartRote[nMode].z ) * t
			);
	}

	private void BeziersCurvePoint(float Time) {
		Camera.transform.localPosition = new Vector3(
			Mathf.Pow(Time, 2) * m_TurningEndPos.x + 2 * ( 1 - Time ) * Time * m_TurningControlPoint.x + Mathf.Pow(( 1 - Time ), 2) * m_TurningStartPos.x,
			Mathf.Pow(Time, 2) * m_TurningEndPos.y + 2 * ( 1 - Time ) * Time * m_TurningControlPoint.y + Mathf.Pow(( 1 - Time ), 2) * m_TurningStartPos.y,
			Mathf.Pow(Time, 2) * m_TurningEndPos.z + 2 * ( 1 - Time ) * Time * m_TurningControlPoint.z + Mathf.Pow(( 1 - Time ), 2) * m_TurningStartPos.z
			);
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
		imageColor.a += ( FadeVolume / 255 ) * Time.unscaledDeltaTime;
		if(imageColor.a > 1.0f) {
			imageColor.a = 1.0f;
			FadeImage.color = imageColor;
			bFadeStart = false;
			if(Mode >= m_StartPos.Length)
				return;
			ChangeCamera(Mode);
			TimeCount = 0.0f;
			return;
		}
		FadeImage.color = imageColor;
	}

	/// <summary>
	/// ゴールフェードイン
	/// </summary>
	private void GoalFadeIn() {
		var imageColor = FadeImage.color;
		imageColor.a -= ( FadeVolume / 255 ) * Time.unscaledDeltaTime;
		if(imageColor.a < 0) {
			imageColor.a = 0;
		}
		FadeImage.color = imageColor;
	}
}