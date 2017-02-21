using UnityEngine;
using System.Collections;

public class TitleCollision : MonoBehaviour {

    Rigidbody rb;                  // 飛ばすオブジェクトの確保
    public Vector3 Add;
    
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionEnter(Collision col)
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(Add);
    }
}
