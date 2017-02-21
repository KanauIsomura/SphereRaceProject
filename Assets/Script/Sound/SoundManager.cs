using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : SingletonMonoBehaviour<SoundManager> {
    private const string BGM_PATH = "Sounds/BGM";
    private const string SE_PATH = "Sounds/SE";

    public float BGMVolume;
    public float SEVolume;

    public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
    public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
    private float bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

    private AudioSource BGMSource;
    private List<AudioSourceInfo> AudioSourceInfoList = new List<AudioSourceInfo>();
    private const int SE_SOURCE_NUM = 20;

    private Dictionary<string, AudioClip> BGMDictionary;
    private Dictionary<string, AudioClip> SEDictionary;

    private string nextBGMName;
    private string nextSEName;

    private bool isFadeOut = false;

    public class AudioSourceInfo {
        public AudioSource SESource;
        public GameObject ParentObj;
        public GameObject SourceObj;
        public string SEName;
        public bool isFadeOut;
        public float seFadeSpeedRate;
        public AudioSourceInfo() {
            SESource = null;
            ParentObj = null;
        }
    };


    protected override void Awake() {
        DontDestroyOnLoad(gameObject);
        base.Awake();
        for(int i = 0; i < SE_SOURCE_NUM + 1; i++) {
            AudioSourceInfo audioInfo = new AudioSourceInfo();
            GameObject soundObj = new GameObject("AudioObj" + i.ToString());
            audioInfo.ParentObj = null;
            audioInfo.SESource = soundObj.AddComponent<AudioSource>();
            audioInfo.SESource.playOnAwake = false;
            audioInfo.SourceObj = soundObj;
            soundObj.transform.SetParent(this.gameObject.transform);

            if(i == 0) {
                audioInfo.SESource.spatialBlend = 0.0f;
                audioInfo.SESource.loop = true;
                audioInfo.SESource.volume = SEVolume;
                BGMSource = audioInfo.SESource;
            } else {
                audioInfo.SESource.minDistance = 1.0f;
                audioInfo.SESource.spatialBlend = 1.0f;
                audioInfo.SESource.loop = false;
                audioInfo.SESource.volume = SEVolume;
                AudioSourceInfoList.Add(audioInfo);
            }
        }

        BGMDictionary = new Dictionary<string, AudioClip>();
        SEDictionary = new Dictionary<string, AudioClip>();

        object[] BGMList = Resources.LoadAll(BGM_PATH);
        object[] SEList = Resources.LoadAll(SE_PATH);

        foreach(AudioClip bgm in BGMList) {
            BGMDictionary[bgm.name] = bgm;
        }
        foreach(AudioClip se in SEList) {
            SEDictionary[se.name] = se;
        }
    }

    public void PlaySE(string seName, Vector3 position, float minDistance = 1.0f, GameObject parentObj = null, bool Loop = false) {
        if(!SEDictionary.ContainsKey(seName)) {
            Debug.Log(seName + "というSEが見つかりません");
            return;
        }

        foreach(AudioSourceInfo audioSourceInfo in AudioSourceInfoList) {
            if(!audioSourceInfo.SESource.isPlaying) {
                audioSourceInfo.SESource.minDistance = minDistance;
                audioSourceInfo.SESource.volume = SEVolume;
                audioSourceInfo.SourceObj.transform.position = position;
                audioSourceInfo.ParentObj = parentObj;
                audioSourceInfo.SESource.spatialBlend = 1.0f;
                audioSourceInfo.SESource.loop = Loop;
                audioSourceInfo.isFadeOut = false;
                audioSourceInfo.SEName = seName;
                if(Loop) {
                    audioSourceInfo.SESource.clip = SEDictionary[seName] as AudioClip;
                    audioSourceInfo.SESource.Play();
                } else {
                    audioSourceInfo.SESource.PlayOneShot(SEDictionary[seName] as AudioClip);
                }
                return;
            }
        }
    }

    public void PlaySE(string seName , bool Loop = false) {
        foreach(AudioSourceInfo audioSourceInfo in AudioSourceInfoList) {
            if(!audioSourceInfo.SESource.isPlaying) {
                audioSourceInfo.SESource.volume = SEVolume;
                audioSourceInfo.SESource.spatialBlend = 0.0f;
                audioSourceInfo.SESource.loop = Loop;
                audioSourceInfo.isFadeOut = false;
                audioSourceInfo.SEName = seName;
                if(Loop) {
                    audioSourceInfo.SESource.clip = SEDictionary[seName] as AudioClip;
                    audioSourceInfo.SESource.Play();
                } else {
                    audioSourceInfo.SESource.PlayOneShot(SEDictionary[seName] as AudioClip);
                }
                return;
            }
        }
    }

    public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH) {
        if(!BGMDictionary.ContainsKey(bgmName)) {
            Debug.Log(bgmName + "というBGMが見つかりません");
            return;
        }

        if(!BGMSource.isPlaying) {
            nextBGMName = "";
            BGMSource.spatialBlend = 0.0f;
            BGMSource.clip = BGMDictionary[bgmName] as AudioClip;
            if(fadeSpeedRate > 0) {
                BGMSource.volume = 1.0f;
            }else if(fadeSpeedRate < 0) {
                BGMSource.volume = 0.0f;
            }
            BGMSource.Play();
        }else if(BGMSource.clip.name != bgmName){
            nextBGMName = bgmName;
            FadeOutBGM(fadeSpeedRate);
        }
    }

    public void StopBGM() {
        BGMSource.Stop();
    }

    public void StopSE(string seName) {
        foreach(AudioSourceInfo audioSourceInfo in AudioSourceInfoList) {
            if(audioSourceInfo.SESource.isPlaying && audioSourceInfo.SEName == seName) {
                audioSourceInfo.SESource.Stop();
                return;
            }
        }
    }

    public void FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW) {
        bgmFadeSpeedRate = fadeSpeedRate;
        isFadeOut = true;
    }

    public void FadeOutSE(string seName,float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW) {
        foreach(AudioSourceInfo audioSourceInfo in AudioSourceInfoList) {
            if(audioSourceInfo.SESource.isPlaying && audioSourceInfo.SEName == seName) {
                audioSourceInfo.seFadeSpeedRate = fadeSpeedRate;
                audioSourceInfo.isFadeOut = true;
                return;
            }
        }
    }

    private void Update() {
        for(int i = 0; i < AudioSourceInfoList.Count; i++) {
            if(AudioSourceInfoList[i].SESource.isPlaying && AudioSourceInfoList[i].ParentObj != null) {
                AudioSourceInfoList[i].SourceObj.transform.position = AudioSourceInfoList[i].ParentObj.transform.position;
            }
            if(AudioSourceInfoList[i].SESource.isPlaying && AudioSourceInfoList[i].isFadeOut) {
                AudioSourceInfoList[i].SESource.volume -= Time.deltaTime * AudioSourceInfoList[i].seFadeSpeedRate;
                if(AudioSourceInfoList[i].SESource.volume <= 0) {
                    AudioSourceInfoList[i].SESource.Stop();
                    isFadeOut = false;
                }
            }
        }

        if(!isFadeOut) {
            return;
        }

        BGMSource.volume -= Time.deltaTime * bgmFadeSpeedRate;
        if(BGMSource.volume <= 0) {
            BGMSource.Stop();
            BGMSource.volume = BGMVolume;
            isFadeOut = false;

            if(string.IsNullOrEmpty(nextBGMName)) {
                PlayBGM(nextBGMName);
            }
        }
    }

    public void ChangeVolume(float _bgmVolume, float _seVolume) {
        if(!isFadeOut) {
            BGMSource.volume = _bgmVolume;
        }
        foreach(AudioSourceInfo audioSourceInfo in AudioSourceInfoList) {
            if(!audioSourceInfo.isFadeOut)
                audioSourceInfo.SESource.volume = _seVolume;
        }
        if(_bgmVolume > 0)
            BGMVolume = _bgmVolume;
        if(_seVolume > 0)
            SEVolume = _seVolume;
    }

    public void ChangeVolumeRate(float _bgmVolume, float _seVolume) {
        if(!isFadeOut) {
            BGMSource.volume *= _bgmVolume;
        }
        foreach(AudioSourceInfo audioSourceInfo in AudioSourceInfoList) {
            if(!audioSourceInfo.isFadeOut)
                audioSourceInfo.SESource.volume *= _seVolume;
        }
    }
}
