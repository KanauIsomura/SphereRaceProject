using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StageSelectImageLoop : MonoBehaviour {
    [SerializeField]
    private float m_MoveSpeed;
    [SerializeField]
    private int m_Direction;
    [SerializeField]
    private RawImage m_TextImage;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        var UVRect = m_TextImage.uvRect;
        UVRect.x += m_MoveSpeed * m_Direction * Time.deltaTime;
        m_TextImage.uvRect = UVRect;
    }
}
