using UnityEditor;

public class QuickStarter{
    [MenuItem("AdditionalItems/NameCreator/AudioNameCreate &s")]
    static void AudioNameCreate() {
        if(EditorApplication.isPlaying) {
            EditorApplication.isPlaying = false;
        }else {
            EditorApplication.isPlaying = true;
        }
    }
}
