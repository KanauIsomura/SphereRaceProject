using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GhostCar : MonoBehaviour {
	public struct ReplayData{
		public float		fTime;
		public Vector3	Position;
		public Quaternion	Rotate;
	};

	[SerializeField]
	int MaxRecordPerSecond = 0;
	
	float	Duration;
	float	fRecordStartTime;
	float	fReplayStartTime;
	int		nReordCount;
	int		nReplayCount;
	bool		bRecording;
	bool		bReplay;
	List<ReplayData> ReplayDataList = new List<ReplayData>();

	/// <summary>
	/// リプレイデータ
	/// </summary>
	public List<ReplayData> replayData{
		get{	return ReplayDataList;}
		set{	ReplayDataList = value;}
	}

	/// <summary>
	/// 現在のリプレイデータを作成
	/// </summary>
	/// <returns>作成したリプレイデータ</returns>
	ReplayData GetData(){
		ReplayData NowData = new ReplayData();
		NowData.fTime		= Time.time - fRecordStartTime;
		NowData.Position	= transform.position;
		NowData.Rotate	= transform.rotation;
		return NowData;
	}

	// Use this for initialization
	void Awake() {
		Duration = (MaxRecordPerSecond == 0) ? 0 : 1f / MaxRecordPerSecond;
		fRecordStartTime	= 0.0f;
		fReplayStartTime	= 0.0f;
		nReordCount		= 0;
		nReplayCount		= 0;
		bRecording		= false;
		bReplay			= false;
		ReplayDataList.Clear();
	}

	/// <summary>
	/// データの記録をスタート
	/// </summary>
	void StartRecord(){
		StopCoroutine("Replay");
		StopCoroutine("Recording");
		bRecording		= false;
		bReplay			= false;
		fRecordStartTime	= Time.time;
		nReordCount		= 0;
		bRecording		= true;
		ReplayDataList.Clear();
		StartCoroutine("Recording");
	}
	
	/// <summary>
	/// データの記録を停止
	/// </summary>
	void StopRecord(){
		StopCoroutine("Recording");
		bRecording	= false;
	}

	/// <summary>
	/// データの再生をスタート
	/// </summary>
	void StartReplay(){
		StopCoroutine("Recording");
		StopCoroutine("Replay");
		bReplay			= false;
		bRecording		= false;
		fReplayStartTime	= Time.time;
		nReplayCount		= 0;
		bReplay			= true;
		StartCoroutine("Replay");
	}

	/// <summary>
	/// データの再生を停止
	/// </summary>
	void StopReplay(){
		StopCoroutine("Replay");
		bReplay	= false;
	}

	/// <summary>
	/// データ書き込みのコルーチン
	/// </summary>
	/// <returns></returns>
	IEnumerator Recording(){
		while(bRecording){
			ReplayRecording();

			yield return new WaitForSeconds(Duration);
		}
	}
	
	/// <summary>
	/// データ再生のコルーチン
	/// </summary>
	/// <returns></returns>
	IEnumerator Replay(){
		while(bReplay){
			ReplayPlaying(Time.time - fReplayStartTime);

			if(nReplayCount + 1 < nReordCount - 1)
				++nReplayCount;

			yield return new WaitForSeconds(0);
		}
	}

	/// <summary>
	/// リプレイ情報のセット
	/// </summary>
	/// <param name="BeforeData">変更前のデータ</param>
	/// <param name="AfterData">変更後のデータ</param>
	/// <param name="DeltaTime">変更割合</param>
	void SetReplay(ReplayData BeforeData, ReplayData AfterData, float DeltaTime) {
		Vector3 LerpPosition = Vector3.Lerp(BeforeData.Position, AfterData.Position, DeltaTime);
		Quaternion LerpQuaternion = Quaternion.Lerp(BeforeData.Rotate, AfterData.Rotate, DeltaTime);
		if(float.IsNaN(LerpPosition.x) || float.IsNaN(LerpPosition.y) || float.IsNaN(LerpPosition.z))
			return;
		if(float.IsNaN(LerpQuaternion.x) || float.IsNaN(LerpQuaternion.y) || float.IsNaN(LerpQuaternion.z))
			return;

		transform.position		= LerpPosition;
		transform.rotation		= LerpQuaternion ;
	}

	/// <summary>
	/// リプレイの再生
	/// </summary>
	/// <param name="fTime">リプレイ経過時間</param>
	void ReplayPlaying(float fTime){
		int DataIndex = 0;
		for(int i = 0; i < ReplayDataList.Count; ++i){
			if(ReplayDataList[i].fTime > fTime) {
				DataIndex = i - 1;
				break;
			}
		}

		if(DataIndex < 1)
			return;

		ReplayData BeforeData	= ReplayDataList[DataIndex - 1];
		ReplayData AfterData	= ReplayDataList[DataIndex];

		float DeltaTime = AfterData.fTime - fTime / (AfterData.fTime - BeforeData.fTime);
		SetReplay(BeforeData, AfterData, DeltaTime);
	}

	/// <summary>
	/// リプレイデータの記録
	/// </summary>
	void ReplayRecording(){
		ReplayDataList.Add(GetData());
	}
}
