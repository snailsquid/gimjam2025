using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public string menuMusic, gameplayMusic;
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
    public void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene == SceneManager.GetSceneByName("MainMenu"))
        {
            PlaySound(menuMusic);
        }
        else
        {
            PlaySound(gameplayMusic);
        }
    }
    public void PlaySound(string key, Vector3? position = null, bool loop = false)
    {

        if (!audioClips.ContainsKey(key))
        {
            Debug.LogWarning("AudioManager: Audio key not found: " + key);
            return;
        }
        AudioClip audioClip = audioClips[key];
        Vector3 clipPosition = position ?? Camera.main.transform.position;
        GameObject audioObject = new GameObject("AudioObject");
        audioObject.transform.position = clipPosition;
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
        if (loop)
        {
            audioSource.loop = true;
        }
        else
        {
            Destroy(audioObject, audioClip.length);
        }
    }
}
