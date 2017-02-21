using UnityEngine;
using System.Collections;

public class GlitterEffect : MonoBehaviour {
	public ParticleSystem[] GlitterParticle;    //キラキラエフェクトの配列

	// Use this for initialization
	/*void Start () {
	}
	
	// Update is called once per frame
	*/void Update () {
		if(Input.GetKeyDown(KeyCode.Z)){
			StartGlitter();
		}
	}

	void StartGlitter() {
		for(int nCount = 0; nCount < GlitterParticle.Length; ++nCount) {
			GlitterParticle[nCount].gameObject.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
			GlitterParticle[nCount].Play();
		}
	}
}
