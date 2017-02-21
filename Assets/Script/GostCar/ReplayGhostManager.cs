using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ReplayGhostManager : MonoBehaviour {
	[SerializeField] string			FilePathName;		//読み込み再生を行うファイルネーム
	[SerializeField] GhostCar			PlayerObject;		//記録するプレイヤーオブジェクト
	[SerializeField] GhostCar			PlayerUFOObject;	//記録するUFOオブジェクト
	[SerializeField] GhostCar			GhostObject;		//再生するゴーストオブジェクト
	[SerializeField] GhostCar			GhostUFOObject;	//再生するゴーストUFO
	[SerializeField] StartProduction	StartScript;		//スタートフラグのscript

	bool bStart;				//スタートしたか
	
	static string PlayerDataName = "PlayerData";
	static string UFODataName = "UFOData";

	// Use this for initialization
	void Start () {
		bStart = false;
		LoadGhost();		//ゴーストデータの読み込み
	}
	
	// Update is called once per frame
	void Update () {
		//レースがスタートしていてスタートフラグが立っていないとき
		if(StartScript.isStart && !bStart){
			PlayerObject.SendMessage("StartRecord");			//ゴーストの記録開始
			PlayerUFOObject.SendMessage("StartRecord");		//UFOゴーストの記録開始
			GhostObject.SendMessage("StartReplay");			//ゴーストのリプレイの開始
			GhostUFOObject.SendMessage("StartReplay");		//UFOゴーストのリプレイの開始
			bStart = true;
		}

		if(Application.isEditor && Input.GetKeyDown(KeyCode.Z)){
			PlayerObject.SendMessage("StopRecord");
			PlayerUFOObject.SendMessage("StopRecord");
			SeveGhost();
		}
	}
	
	/// <summary>
	/// ゴーストデータの記録
	/// </summary>
	void SeveGhost(){
		FileInfo fi = new FileInfo(FilePathName);	//ファイルインフォ
		StreamWriter DataStream;					//ストリーム
		DataStream = fi.CreateText();

		DataStream.Write(PlayerDataName + ",");

		//Playerデータの書き込み
		foreach(GhostCar.ReplayData rd in PlayerObject.replayData){
			DataStream.Write(rd.fTime);DataStream.Write(',');
			DataStream.Write(rd.Position.x);DataStream.Write(',');
			DataStream.Write(rd.Position.y);DataStream.Write(',');
			DataStream.Write(rd.Position.z);DataStream.Write(',');
			DataStream.Write(rd.Rotate.x);DataStream.Write(',');
			DataStream.Write(rd.Rotate.y);DataStream.Write(',');
			DataStream.Write(rd.Rotate.z);DataStream.Write(',');
			DataStream.Write(rd.Rotate.w);DataStream.Write(',');
		}

		//UFODataの区切り
		DataStream.Write(UFODataName + ",");

		//UFOデータの書き込み
		foreach(GhostCar.ReplayData rd in PlayerUFOObject.replayData){
			DataStream.Write(rd.fTime);DataStream.Write(',');
			DataStream.Write(rd.Position.x);DataStream.Write(',');
			DataStream.Write(rd.Position.y);DataStream.Write(',');
			DataStream.Write(rd.Position.z);DataStream.Write(',');
			DataStream.Write(rd.Rotate.x);DataStream.Write(',');
			DataStream.Write(rd.Rotate.y);DataStream.Write(',');
			DataStream.Write(rd.Rotate.z);DataStream.Write(',');
			DataStream.Write(rd.Rotate.w);DataStream.Write(',');
		}

		DataStream.Flush();
		DataStream.Close();
	}
	
	/// <summary>
	/// ゴーストデータの読み込み
	/// </summary>
	void LoadGhost(){
		FileInfo fi = new FileInfo(FilePathName);
		StreamReader sw = new StreamReader(fi.OpenRead());

		//SplitのOptionの設定
		System.StringSplitOptions SplitOption = System.StringSplitOptions.RemoveEmptyEntries;
		
		//ファイルデータをEOFまでストリングに読み込む
		string FileData = sw.ReadToEnd();

		//,区切りでStringデータを保存する
		string[] StringData = FileData.Split(new char[] {','}, SplitOption);

		//データを保存するリスト
		List<GhostCar.ReplayData> PlayerDataList = new List<GhostCar.ReplayData>();
		List<GhostCar.ReplayData> UFODataList = new List<GhostCar.ReplayData>();
		
		//データのカウンター
		int nDataCount = 0;
		
		//Playerデータの読み込み
		for(;nDataCount < StringData.Length; ++nDataCount){
			if(StringData[nDataCount] == PlayerDataName){continue;}
			if(StringData[nDataCount] == UFODataName){++nDataCount; break;}
			GhostCar.ReplayData LoadData = new GhostCar.ReplayData();
			LoadData.fTime		= float.Parse(StringData[nDataCount]); nDataCount++;
			LoadData.Position.x	= float.Parse(StringData[nDataCount]); nDataCount++;
			LoadData.Position.y	= float.Parse(StringData[nDataCount]); nDataCount++;
			LoadData.Position.z	= float.Parse(StringData[nDataCount]); nDataCount++;
			LoadData.Rotate.x		= float.Parse(StringData[nDataCount]); nDataCount++;
			LoadData.Rotate.y		= float.Parse(StringData[nDataCount]); nDataCount++;
			LoadData.Rotate.z		= float.Parse(StringData[nDataCount]); nDataCount++;
			LoadData.Rotate.w		= float.Parse(StringData[nDataCount]);
			PlayerDataList.Add(LoadData);
		}
		//UFOデータの読み込み
		for(;nDataCount < StringData.Length; ++nDataCount){
			GhostCar.ReplayData LoadData = new GhostCar.ReplayData();
			LoadData.fTime		= float.Parse(StringData[nDataCount]); nDataCount++;
			LoadData.Position.x	= float.Parse(StringData[nDataCount]); nDataCount++;
			LoadData.Position.y	= float.Parse(StringData[nDataCount]); nDataCount++;
			LoadData.Position.z	= float.Parse(StringData[nDataCount]); nDataCount++;
			LoadData.Rotate.x		= float.Parse(StringData[nDataCount]); nDataCount++;
			LoadData.Rotate.y		= float.Parse(StringData[nDataCount]); nDataCount++;
			LoadData.Rotate.z		= float.Parse(StringData[nDataCount]); nDataCount++;
			LoadData.Rotate.w		= float.Parse(StringData[nDataCount]);
			UFODataList.Add(LoadData);
		}

		sw.Close();
		
		//データを受け渡す
		GhostObject.replayData		= PlayerDataList;
		GhostUFOObject.replayData	= UFODataList;
	}
}
