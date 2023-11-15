using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Audio[] sounds;
    public static AudioManager instance;

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
            sound.source.loop = sound.loop;
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

    // Start is called before the first frame update
    void Start()
    {
        playSound("GameTheme");
    }

    // Update is called once per frame
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
