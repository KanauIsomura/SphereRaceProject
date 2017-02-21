using UnityEngine;
using System.Collections;

public class TitleDestroyObjTime : MonoBehaviour {

    [SerializeField]
    float DestroyObjTime;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        DestroyObjTime -= Time.deltaTime;


        if (0 > DestroyObjTime)
        {
            Destroy(gameObject);
        }
    }
}
