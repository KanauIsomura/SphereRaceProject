using UnityEngine;
using System.Collections;

public class MobAction : MonoBehaviour {
    enum Action
    {
        WAIT,
        ESCPE,
        HUNT,
        RETURN,
        STOP,
        AVTION_MAX
    };

    public GameObject Target;

    [SerializeField]
    private Vector3 StartPos;
    [SerializeField]
    private Action Gimmick_State;
    [SerializeField]
    private float StopTime;

    public float speed = 1.0f;
    public int max = 1;

    private float fStopStartTime;
    private SpiritStatus Size;
    
    
	// Use this for initialization
	void Start () {
        StartPos = transform.position;
        Gimmick_State = Action.WAIT;
        Size = GetComponent<SpiritStatus>();
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 Move;
        Vector3 newRotation;
        switch (Gimmick_State)
        {
            case Action.STOP:
                if ((Time.time - fStopStartTime) >= StopTime)
                    Gimmick_State = Action.WAIT;

                break;
            case Action.WAIT:
                if (Mathf.Abs(StartPos.x - transform.position.x) > 1 || Mathf.Abs(StartPos.z - transform.position.z) > 1)
                {
                    StartPos.y = transform.position.y;
                    transform.LookAt(StartPos);
                    Move = transform.forward * speed;
                    transform.position += Move * Time.deltaTime;
                }
                break;

            case Action.RETURN:
                if (Mathf.Abs(StartPos.x - transform.position.x) > 1 || Mathf.Abs(StartPos.z - transform.position.z) > 1)
                {
                    StartPos.y = transform.position.y;
                    transform.LookAt(StartPos);
                    Move = transform.forward * speed;
                    transform.position += Move * Time.deltaTime;
                }
                else
                {
                    fStopStartTime = Time.time;
                    Gimmick_State = Action.STOP;
                }
                break;
            case Action.ESCPE:
                newRotation.x = transform.rotation.x;
                newRotation.z = transform.rotation.z;
                transform.LookAt(Target.transform.position);
                transform.rotation = new Quaternion(newRotation.x, transform.rotation.y * -1, newRotation.z, transform.rotation.w);
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), 5 * Time.deltaTime);

                Move = transform.forward * speed;
                transform.position += Move * Time.deltaTime;
                break;
            case Action.HUNT:
                newRotation.x = transform.rotation.x;
                newRotation.z = transform.rotation.z;
                transform.LookAt(Target.transform.position);
                transform.rotation = new Quaternion(newRotation.x, transform.rotation.y, newRotation.z,transform.rotation.w);
                //newRotation.y = transform.rotation.y;
                //transform.LookAt(newRotation);
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), 5 * Time.deltaTime);

                Move = transform.forward * speed;
                transform.position += Move * Time.deltaTime;
                break;
        }
	}

    //衝突判定
    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            //逃げている時なら
            if (Gimmick_State == Action.ESCPE)
                GetComponent<MobAction>().enabled = false;
            else
                Gimmick_State = Action.RETURN;
                
        }
    }
    
    //プレイヤー発見時
    void Locate(Collider collider)
    {
        // ターゲットとのサイズ判定処理の追加が必要！
        if (collider.GetComponent<KatamariStatus>().m_Size < Size.m_Size)
        {
            if (Gimmick_State == Action.RETURN || Gimmick_State == Action.STOP) return;

            Gimmick_State = Action.HUNT;
        }
        else
        {
            Gimmick_State = Action.ESCPE;
        }
    }

    //プレイヤーを見失ったら
    void LoseLocate(Collider collider)
    {
        if (Gimmick_State == Action.RETURN || Gimmick_State == Action.STOP) return;
            Gimmick_State = Action.WAIT;
    }
}
