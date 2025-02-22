using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public string key;
    public void PlaySound()
    {
        AudioManager.Instance.PlaySound(key);
    }
}
[CustomEditor(typeof(AudioTest))]
public class AudioTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AudioTest audioTest = (AudioTest)target;
        if (GUILayout.Button("Play Sound") && Application.isPlaying)
        {
            audioTest.PlaySound();
        }
    }
}
