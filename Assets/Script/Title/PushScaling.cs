using UnityEngine;
using System.Collections;


/// <summary>
/// プッシュ画像拡縮
/// </summary>
public class PushScaling : MonoBehaviour {

    Vector3 m_InitScale;      //初期角度

    // Use this for initialization
    void Start()
    {
        m_InitScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Scale = transform.eulerAngles;
        Scale.x = m_InitScale.x + 0.1f * Mathf.Sin(Time.time);
        Scale.y = m_InitScale.y + 0.1f * Mathf.Sin(Time.time);
        transform.localScale = Scale;
    }
}
