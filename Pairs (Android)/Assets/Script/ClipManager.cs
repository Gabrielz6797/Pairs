using UnityEngine.Audio;
using System;
using UnityEngine;

public class ClipManager : MonoBehaviour
{
    public Audio[] sounds;
    public static ClipManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        foreach (Audio sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }
    }

    public void playSound(string name)
    {
        Audio audio = Array.Find(sounds, sound => sound.name == name);
        if (audio == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        audio.source.Play();
    }

    void Start() { }

    void Update()
    {
        if (GameSettings.Instance.isSoundEffectMutedPermanently())
        {
            AudioListener.pause = true;
        }
        else
        {
            AudioListener.pause = false;
        }
    }
}
