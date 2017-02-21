using UnityEngine;
using System.Collections;

public class BoosterEffect : MonoBehaviour {

    public ParticleSystem[] BoosterParticle;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// エフェクト表示
    /// </summary>
    public void StartEffect()
    {
        for (int i = 0; i < BoosterParticle.Length; i++)
        {
            SoundManager.Instance.PlaySE("burst");
            BoosterParticle[i].Play();
        }
    }
}
