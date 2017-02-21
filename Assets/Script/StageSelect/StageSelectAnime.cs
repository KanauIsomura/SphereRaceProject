using UnityEngine;
using System.Collections;

public class StageSelectAnime : MonoBehaviour {

    Animator anim;
    Vector3 OldPos;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        OldPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (OldPos == transform.position)
        {
            anim.SetBool("isRun", false);
        }
        else
        {
            anim.SetBool("isRun", true);
        }

        OldPos = transform.position;
	}
}
