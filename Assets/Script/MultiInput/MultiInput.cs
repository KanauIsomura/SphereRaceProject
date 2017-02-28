//=========================================
/*
  取り扱い説明書
☆ボタンを取得するときはMultiInput.Instance.GetPressButton()に
  MultiInput.CONTROLLER_BUTTON列挙型を引数に入れて呼んでください。
☆スティックを取得するときはMultiInput.Instance.GetLeftStickAxis()
  もしくはMultiInput.Instance.GetRightStickAxis()を呼んでください。
☆スティックはそれぞれ-1.0～1.0のVector2で、その他ボタンはboolで返ってきます。
  スティックは右上が(1.0,1.0)です。

☆パブリックの変数にはキーを入れてください。
☆パッドによってボタン配置が違うかもしれないです。
  (エレコムのJC-U3312Sを基準としています)
  その時はお手数ですがStartでpadDictionalyに入れているキーコードを
  変えてください(ボタン1がKeyCode.Joystick1Button0です)。

☆十字キーと右スティックが反応しない時はProjectSettingsのInputのSizeを22にして以下の4つを追加してください。
Name:HorizontalRight
DescriptiveNameからAltPositiveButtonまで空白
Gravity:0
Dead:0.19
Sensitivity:1
Type:JoystickAxis
Axis:3rd axis

Name:VerticalRight
DescriptiveNameからAltPositiveButtonまで空白
Gravity:0
Dead:0.19
Sensitivity:1
Type:JoystickAxis
Axis:4th axis

Name:GamePadHorizontal
DescriptiveNameからAltPositiveButtonまで空白
Gravity:3
Dead:0.001
Sensitivity:3
Type:JoystickAxis
Axis:5th axis

Name:GamePadVertical
DescriptiveNameからAltPositiveButtonまで空白
Gravity:3
Dead:0.001
Sensitivity:3
Type:JoystickAxis
Axis:6th axis

☆追加機能
実行中に歯車アイコンのタブを開いて「KeyDataのアタッチメント」を行うとキーデータの上書きができます。
現在keyData.binの中身が不十分だとバグります、ごめんなさい。解決策模索中....

PS.頭悪いプログラムでごめんなさい☆
*/
//=========================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class MultiInput : SingletonMonoBehaviour<MultiInput> {
    public enum CONTROLLER_BUTTON {
        CLOSS_UP,        // 十字キー上
        CLOSS_DOWN,      // 十字キー下
        CLOSS_LEFT,      // 十字キー左
        CLOSS_RIGHT,     // 十字キー右
        CIRCLE,          // ○ボタン(Aボタン)
        CANCEL,          // ×ボタン(Bボタン)
        TRIANGLE,        // △ボタン(Xボタン)
        SQUARE,          // □ボタン(Yボタン)
        RIGHT_1,         // R1ボタン
        RIGHT_2,         // R2ボタン
        RIGHT_3,         // R3ボタン
        LEFT_1,          // L1ボタン
        LEFT_2,          // L2ボタン
        LEFT_3,          // L3ボタン  
        RIGHTSTICK_UP,   // 右スティック上
        RIGHTSTICK_DOWN, // 右スティック下
        RIGHTSTICK_LEFT, // 右スティック左
        RIGHTSTICK_RIGHT,// 右スティック右 
        LEFTSTICK_UP,    // 左スティック上
        LEFTSTICK_DOWN,  // 左スティック下
        LEFTSTICK_LEFT,  // 左スティック左
        LEFTSTICK_RIGHT, // 左スティック右
        START,           // 左スティック左
        SELECT,			 // 左スティック右

        MAX_BUTTOM_NUM,
    };

    public enum STICK_AXIS {
        LEFTSTICK_UP = 0,
        LEFTSTICK_RIGHT,
        LEFTSTICK_DOWN,
        LEFTSTICK_LEFT,
        RIGHTSTICK_UP,
        RIGHTSTICK_RIGHT,
        RIGHTSTICK_DOWN,
        RIGHTSTICK_LEFT,
    };
    [Header("空白にするとたぶんバグるので何かしら入れておいてください")]
    [Header("ボタン名はDUALSHOCK基準です")]
    [Header("各ボタンに合わせたいキーを入れてください。")]
    public string ButtonClossUp;    // 十字キー上
    public string ButtonClossDown;	// 十字キー下
    public string ButtonClossLeft;	// 十字キー左
    public string ButtonClossRight;	// 十字キー右
    public string ButtonCircle;		// ○ボタン(Aボタン)
    public string ButtonCancel;		// ×ボタン(Bボタン)
    public string ButtonTriangle;	// △ボタン(Xボタン)
    public string ButtonSquare;		// □ボタン(Yボタン)
    public string ButtonR1;			// R1ボタン
    public string ButtonR2;			// R2ボタン
    public string ButtonR3;			// R3ボタン(押し込み)
    public string ButtonL1;			// L1ボタン
    public string ButtonL2;			// L2ボタン
    public string ButtonL3;			// L3ボタン(押し込み)  
    public string StickR_Up;		// 右スティック上
    public string StickR_Down;		// 右スティック下
    public string StickR_Left;		// 右スティック左
    public string StickR_Right;		// 右スティック右 
    public string StickL_Up;		// 左スティック上
    public string StickL_Down;		// 左スティック下
    public string StickL_Left;      // 左スティック左
    public string StickL_Right;     // 左スティック右
    public string ButtonStart;      // スタート
    public string ButtonSelect;     // セレクト

    [SerializeField]
    private Dictionary<CONTROLLER_BUTTON, string> keyDictionary;
    private Dictionary<CONTROLLER_BUTTON, KeyCode> padDictionary;

    private bool[] BeforeClossButton;
    private bool[] BeforeStick;
    private bool[] NowStick;

    [SerializeField]
    private string KeyBoardDataFileName = "keyData";

    new void Awake() {
        ReadingKeyBoardData();    // データ読み込み
        AttachKeyBoard();
        // げーむぱっど(Joystick1Button19は対応するボタンがなかったので仮で入れてあるものです)
        padDictionary = new Dictionary<CONTROLLER_BUTTON, KeyCode>()
        {
            {CONTROLLER_BUTTON.CLOSS_UP,   KeyCode.Joystick1Button19},
            {CONTROLLER_BUTTON.CLOSS_DOWN, KeyCode.Joystick1Button19},
            {CONTROLLER_BUTTON.CLOSS_LEFT, KeyCode.Joystick1Button19},
            {CONTROLLER_BUTTON.CLOSS_RIGHT,KeyCode.Joystick1Button19},
            {CONTROLLER_BUTTON.CIRCLE,     KeyCode.Joystick1Button3},
            {CONTROLLER_BUTTON.CANCEL,     KeyCode.Joystick1Button2},
            {CONTROLLER_BUTTON.TRIANGLE,   KeyCode.Joystick1Button1},
            {CONTROLLER_BUTTON.SQUARE,     KeyCode.Joystick1Button0},
            {CONTROLLER_BUTTON.RIGHT_1,    KeyCode.Joystick1Button5},
            {CONTROLLER_BUTTON.RIGHT_2,    KeyCode.Joystick1Button7},
            {CONTROLLER_BUTTON.RIGHT_3,    KeyCode.Joystick1Button12},
            {CONTROLLER_BUTTON.LEFT_1,     KeyCode.Joystick1Button4},
            {CONTROLLER_BUTTON.LEFT_2,     KeyCode.Joystick1Button6},
            {CONTROLLER_BUTTON.LEFT_3,     KeyCode.Joystick1Button11},
            {CONTROLLER_BUTTON.RIGHTSTICK_UP,      KeyCode.Joystick1Button19},
            {CONTROLLER_BUTTON.RIGHTSTICK_DOWN,    KeyCode.Joystick1Button19},
            {CONTROLLER_BUTTON.RIGHTSTICK_LEFT,    KeyCode.Joystick1Button19},
            {CONTROLLER_BUTTON.RIGHTSTICK_RIGHT,   KeyCode.Joystick1Button19},
            {CONTROLLER_BUTTON.LEFTSTICK_UP,       KeyCode.Joystick1Button19},
            {CONTROLLER_BUTTON.LEFTSTICK_DOWN,     KeyCode.Joystick1Button19},
            {CONTROLLER_BUTTON.LEFTSTICK_LEFT,     KeyCode.Joystick1Button19},
            {CONTROLLER_BUTTON.LEFTSTICK_RIGHT,    KeyCode.Joystick1Button19},
            {CONTROLLER_BUTTON.START,              KeyCode.Joystick1Button10},
            {CONTROLLER_BUTTON.SELECT,             KeyCode.Joystick1Button9},
        };

        BeforeClossButton = new bool[4];
        BeforeStick = new bool[8];  // 0を左スティックの上方向として時計回りに1,2,3。4以降は右スティックで同じく時計回りに5,6,7。
        NowStick = new bool[8];
    }

    // Update is called once per frame   
    void Update() {
        NowStick[0] = GetLeftStickAxis().y > 0.8f ? true : false;
        NowStick[1] = GetLeftStickAxis().x > 0.8f ? true : false;
        NowStick[2] = GetLeftStickAxis().y < -0.8f ? true : false;
        NowStick[3] = GetLeftStickAxis().x < -0.8f ? true : false;
        NowStick[4] = GetRightStickAxis().y > 0.8f ? true : false;
        NowStick[5] = GetRightStickAxis().x > 0.8f ? true : false;
        NowStick[6] = GetRightStickAxis().y < -0.8f ? true : false;
        NowStick[7] = GetRightStickAxis().x < -0.8f ? true : false;
    }

    /// <summary>
    /// トリガー系の判定用
    /// </summary>
    void LateUpdate() {
        int i;
        // 現在フレームで入力されているかの確認(TriggerとRelease用)
        for(i = 0; i < BeforeClossButton.Length; i++) {
            BeforeClossButton[i] = GetPressButton((CONTROLLER_BUTTON)i);
        }
        for(i = 0; i < 8; i++) {
            BeforeStick[i] = NowStick[i];
        }
    }

    /// <summary>
    /// キーボードデータの取得
    /// 概要
    /// ファイルからキーデータを読み込む。ファイルが存在しない場合は生成する。
    /// </summary>
    private void ReadingKeyBoardData() {
        string FileName = "Data/"+KeyBoardDataFileName+".bin";
        if(File.Exists(FileName)) {
            FileInfo keyDataFile = new FileInfo("Data/" + KeyBoardDataFileName + ".bin");
            StreamReader streamReader = new StreamReader(keyDataFile.OpenRead());
            // キーの割り当て
            ButtonClossUp = streamReader.ReadLine();
            ButtonClossDown = streamReader.ReadLine();
            ButtonClossLeft = streamReader.ReadLine();
            ButtonClossRight = streamReader.ReadLine();
            ButtonCircle = streamReader.ReadLine();
            ButtonCancel = streamReader.ReadLine();
            ButtonTriangle = streamReader.ReadLine();
            ButtonSquare = streamReader.ReadLine();
            ButtonR1 = streamReader.ReadLine();
            ButtonR2 = streamReader.ReadLine();
            ButtonR3 = streamReader.ReadLine();
            ButtonL1 = streamReader.ReadLine();
            ButtonL2 = streamReader.ReadLine();
            ButtonL3 = streamReader.ReadLine();
            StickR_Up = streamReader.ReadLine();
            StickR_Down = streamReader.ReadLine();
            StickR_Left = streamReader.ReadLine();
            StickR_Right = streamReader.ReadLine();
            StickL_Up = streamReader.ReadLine();
            StickL_Down = streamReader.ReadLine();
            StickL_Left = streamReader.ReadLine();
            StickL_Right = streamReader.ReadLine();
            ButtonStart = streamReader.ReadLine();
            ButtonSelect = streamReader.ReadLine();
            streamReader.Close();
        }else {
            FileStream fileStream = File.Create("Data/"+KeyBoardDataFileName+".bin");
            fileStream.Close();
            AttachKeyBoard();
        }
    }

    /// <summary>
    /// キーボードデータの書き出し
    /// </summary>
    private void OutputKeyBoardData() {
        FileInfo keyDataFile = new FileInfo("Data/" + KeyBoardDataFileName + ".bin");
        StreamWriter streamWriter;
        streamWriter = keyDataFile.CreateText();
        int i;
        for(i = 0; i < (int)CONTROLLER_BUTTON.MAX_BUTTOM_NUM; i++) {
            streamWriter.WriteLine(keyDictionary[(CONTROLLER_BUTTON)i]);
        }
        streamWriter.Flush();
        streamWriter.Close();
    }

    /// <summary>
    /// キーボードの読み込み、更新
    /// </summary>
    private void UpdateKeyData() {
        // きーぼーど
        keyDictionary = new Dictionary<CONTROLLER_BUTTON, string>()
        {
            {CONTROLLER_BUTTON.CLOSS_UP,   ButtonClossUp},
            {CONTROLLER_BUTTON.CLOSS_DOWN, ButtonClossDown},
            {CONTROLLER_BUTTON.CLOSS_LEFT, ButtonClossLeft},
            {CONTROLLER_BUTTON.CLOSS_RIGHT,ButtonClossRight},
            {CONTROLLER_BUTTON.CIRCLE,     ButtonCircle},
            {CONTROLLER_BUTTON.CANCEL,     ButtonCancel},
            {CONTROLLER_BUTTON.TRIANGLE,   ButtonTriangle},
            {CONTROLLER_BUTTON.SQUARE,     ButtonSquare},
            {CONTROLLER_BUTTON.RIGHT_1,    ButtonR1},
            {CONTROLLER_BUTTON.RIGHT_2,    ButtonR2},
            {CONTROLLER_BUTTON.RIGHT_3,    ButtonR3},
            {CONTROLLER_BUTTON.LEFT_1,     ButtonL1},
            {CONTROLLER_BUTTON.LEFT_2,     ButtonL2},
            {CONTROLLER_BUTTON.LEFT_3,     ButtonL3},
            {CONTROLLER_BUTTON.RIGHTSTICK_UP,      StickR_Up},
            {CONTROLLER_BUTTON.RIGHTSTICK_DOWN,    StickR_Down},
            {CONTROLLER_BUTTON.RIGHTSTICK_LEFT,    StickR_Left},
            {CONTROLLER_BUTTON.RIGHTSTICK_RIGHT,   StickR_Right},
            {CONTROLLER_BUTTON.LEFTSTICK_UP,       StickL_Up},
            {CONTROLLER_BUTTON.LEFTSTICK_DOWN,     StickL_Down},
            {CONTROLLER_BUTTON.LEFTSTICK_LEFT,     StickL_Left},
            {CONTROLLER_BUTTON.LEFTSTICK_RIGHT,    StickL_Right},
            {CONTROLLER_BUTTON.START,              ButtonStart},
            {CONTROLLER_BUTTON.SELECT,             ButtonSelect},
        };
    }

    /// <summary>
    /// 実行中に変更を掛けたキーボードデータのアタッチメント
    /// </summary>
    [ContextMenu("KeyDataのアタッチメント")]
    private void AttachKeyBoard() {
        UpdateKeyData();
        OutputKeyBoardData();
    }

    // 以下コメントなし。

    public bool GetPressButton(CONTROLLER_BUTTON button) {
        if(button > CONTROLLER_BUTTON.CLOSS_RIGHT) {
            if(Input.GetKey(keyDictionary[button]) || Input.GetKey(padDictionary[button])) {
                return true;
            }
        } else {
            if(button == CONTROLLER_BUTTON.CLOSS_UP) {
                if(Input.GetKey(keyDictionary[button]) || Input.GetAxisRaw("GamePadVertical") == 1.0f) {
                    return true;
                }
            } else if(button == CONTROLLER_BUTTON.CLOSS_DOWN) {
                if(Input.GetKey(keyDictionary[button]) || Input.GetAxisRaw("GamePadVertical") == -1.0f) {
                    return true;
                }
            } else if(button == CONTROLLER_BUTTON.CLOSS_RIGHT) {
                if(Input.GetKey(keyDictionary[button]) || Input.GetAxisRaw("GamePadHorizontal") == 1.0f) {
                    return true;
                }
            } else {
                if(Input.GetKey(keyDictionary[button]) || Input.GetAxisRaw("GamePadHorizontal") == -1.0f) {
                    return true;
                }
            }
        }


        return false;
    }
    public bool GetTriggerButton(CONTROLLER_BUTTON button) {
        if(button > CONTROLLER_BUTTON.CLOSS_RIGHT) {
            if(Input.GetKeyDown(keyDictionary[button]) || Input.GetKeyDown(padDictionary[button])) {
                return true;
            }
        } else {
            if(button == CONTROLLER_BUTTON.CLOSS_UP) {
                if(Input.GetKey(keyDictionary[button]) || Input.GetAxisRaw("GamePadVertical") == 1.0f) {
                    if(!BeforeClossButton[0])
                        return true;
                }
            } else if(button == CONTROLLER_BUTTON.CLOSS_DOWN) {
                if(Input.GetKey(keyDictionary[button]) || Input.GetAxisRaw("GamePadVertical") == -1.0f) {
                    if(!BeforeClossButton[1])
                        return true;
                }
            } else if(button == CONTROLLER_BUTTON.CLOSS_RIGHT) {
                if(Input.GetKey(keyDictionary[button]) || Input.GetAxisRaw("GamePadHorizontal") == 1.0f) {
                    if(!BeforeClossButton[3])
                        return true;
                }
            } else {
                if(Input.GetKey(keyDictionary[button]) || Input.GetAxisRaw("GamePadHorizontal") == -1.0f) {
                    if(!BeforeClossButton[2])
                        return true;
                }
            }
        }

        return false;
    }
    public bool GetReleaseButton(CONTROLLER_BUTTON button) {
        if(Input.GetKeyUp(keyDictionary[button]) || Input.GetKeyUp(padDictionary[button])) {
            return true;
        }
        if(button > CONTROLLER_BUTTON.CLOSS_RIGHT) {
            if(Input.GetKey(keyDictionary[button]) || Input.GetKey(padDictionary[button])) {
                return true;
            }
        } else {
            if(button == CONTROLLER_BUTTON.CLOSS_UP) {
                if(!Input.GetKey(keyDictionary[button]) && !( Input.GetAxisRaw("GamePadVertical") == 1.0f )) {
                    if(BeforeClossButton[0])
                        return true;
                }
            } else if(button == CONTROLLER_BUTTON.CLOSS_DOWN) {
                if(!Input.GetKey(keyDictionary[button]) && !( Input.GetAxisRaw("GamePadVertical") == -1.0f )) {
                    if(BeforeClossButton[1])
                        return true;
                }
            } else if(button == CONTROLLER_BUTTON.CLOSS_RIGHT) {
                if(!Input.GetKey(keyDictionary[button]) && !( Input.GetAxisRaw("GamePadHorizontal") == 1.0f )) {
                    if(BeforeClossButton[3])
                        return true;
                }
            } else {
                if(!Input.GetKey(keyDictionary[button]) && !( Input.GetAxisRaw("GamePadHorizontal") == -1.0f )) {
                    if(BeforeClossButton[2])
                        return true;
                }
            }
        }

        return false;
    }
    public Vector2 GetLeftStickAxis() {
        Vector2 vec;
        if(GetPressButton(CONTROLLER_BUTTON.LEFTSTICK_RIGHT)) {
            vec.x = 1.0f;
        } else if(GetPressButton(CONTROLLER_BUTTON.LEFTSTICK_LEFT)) {
            vec.x = -1.0f;
        } else {
            vec.x = Input.GetAxisRaw("Horizontal");
        }
        if(GetPressButton(CONTROLLER_BUTTON.LEFTSTICK_UP)) {
            vec.y = 1.0f;
        } else if(GetPressButton(CONTROLLER_BUTTON.LEFTSTICK_DOWN)) {
            vec.y = -1.0f;
        } else {
            vec.y = Input.GetAxisRaw("Vertical");
        }
        return vec;
    }
    public Vector2 GetRightStickAxis() {
        Vector2 vec;
        if(GetPressButton(CONTROLLER_BUTTON.RIGHTSTICK_RIGHT)) {
            vec.x = 1.0f;
        } else if(GetPressButton(CONTROLLER_BUTTON.RIGHTSTICK_LEFT)) {
            vec.x = -1.0f;
        } else {
            vec.x = Input.GetAxisRaw("HorizontalRight");
        }
        if(GetPressButton(CONTROLLER_BUTTON.RIGHTSTICK_UP)) {
            vec.y = 1.0f;
        } else if(GetPressButton(CONTROLLER_BUTTON.RIGHTSTICK_DOWN)) {
            vec.y = -1.0f;
        } else {
            vec.y = Input.GetAxisRaw("VerticalRight");
        }

        return vec;
    }
    public bool GetTriggerStickAxis(STICK_AXIS stickAxis) {
        return !BeforeStick[(int)stickAxis] && NowStick[(int)stickAxis] ? true : false;
    }
}
