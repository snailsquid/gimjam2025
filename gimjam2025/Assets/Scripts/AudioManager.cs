using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializedDictionary("Key", "Audio Clip")]
    public SerializedDictionary<string, AudioClip> audioClips;
    public void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void PlaySound(string key, Vector3? position = null)
    {
        AudioClip audioClip = audioClips[key];
        Vector3 clipPosition = position ?? Camera.main.transform.position;
        GameObject audioObject = new GameObject("AudioObject");
        audioObject.transform.position = clipPosition;
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
        Destroy(audioObject, audioClip.length);
    }
}
