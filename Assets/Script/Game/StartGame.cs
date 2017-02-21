using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	public Fade m_FeadScript;

	// Use this for initialization
	void Start () {
		m_FeadScript.StartFade(Fade.eFADEMODE.FadeIn);
	}
	
	// Update is called once per frame
	/*void Update () {
	
	}*/
}
