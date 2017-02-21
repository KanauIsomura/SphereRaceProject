using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleAddRigid : MonoBehaviour
{
    Rigidbody rb;                  // 飛ばすオブジェクトの確保

    [Header("飛ばす力")]
    public Vector3 AddForce;       // 飛ばす力

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(AddForce);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
