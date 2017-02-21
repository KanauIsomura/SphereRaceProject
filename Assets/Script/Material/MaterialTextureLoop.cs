using UnityEngine;
using System.Collections;

public class MaterialTextureLoop : MonoBehaviour {
	[SerializeField] Vector2 LoopSpeed = Vector2.zero;	//テクスチャループの速さ
	Material TextureLoopMaterial;

	// Use this for initialization
	void Start () {
		TextureLoopMaterial = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		TextureLoopMaterial.mainTextureOffset += LoopSpeed;
	}
}
