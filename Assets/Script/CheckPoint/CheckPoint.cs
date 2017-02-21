using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {
    [SerializeField]
    public int m_thisPointNumber;

    void OnTriggerEnter(Collider obj) {
        if(obj.tag != "Player") {
            return;
        }

        GameObject.Find("Player").GetComponent<CheckPointChecker>().IntoCheckPoint(m_thisPointNumber);
    }
}
