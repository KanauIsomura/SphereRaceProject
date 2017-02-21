using UnityEngine;
using System.Collections;

public class StageSelectTextLoop : MonoBehaviour {
    [SerializeField]
    private float m_TargetPoint;
    [SerializeField]
    private float m_MoveSpeed;
    [SerializeField]
    private float m_FirstPoint;
    [SerializeField]
    private int m_Direction;
    [SerializeField]
    private RectTransform m_MoveObjPosition;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        var position = m_MoveObjPosition.anchoredPosition;
        position.x += m_MoveSpeed * m_Direction * Time.deltaTime;
        Debug.Log(position.x);
        if(m_Direction < 0) {
            if(position.x < m_TargetPoint) {
                position.x = m_FirstPoint;
            }
        } else {
            if(position.x > m_TargetPoint) {
                position.x = m_FirstPoint;
            }
        }
        m_MoveObjPosition.anchoredPosition = position;
    }
}

