using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManagement : MonoBehaviour
{
    public AudioMixer generalM;

    public AudioMixerGroup masterM;
    public AudioMixerGroup musicM;
    public AudioMixerGroup soundsM;

    public Sound[] sounds;

    void Awake()
    {
        //ReloadSounds();
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            if (s.song == true)
            {
                s.source.outputAudioMixerGroup = musicM;
            }
            else
            {
                s.source.outputAudioMixerGroup = soundsM;
            }
        }
    }

    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
            s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

            s.source.Play();
        }
    }

    public void Play(string sound, float delay)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
            s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

            s.source.PlayDelayed(delay);
        }
    }

    public void PlayOneShot(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
            s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

            s.source.PlayOneShot(s.clip);
        }
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            s.source.Stop();
        }

    }


    public void TogglePause(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            if (s.source.isPlaying == true)
            {
                s.source.Pause();
            }
            else
            {
                s.source.UnPause();
            }
        }
    }

    public void TogglePause(string sound, bool d)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            if (d)
            {
                s.source.Pause();
            }
            else
            {
                s.source.UnPause();
            }
        }
    }

    public bool IsPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return false;
        }
        else
        {
            return s.source.isPlaying;
        }

    }

    public void ReloadSounds()
    {
        AudioSource[] _as = gameObject.GetComponentsInChildren<AudioSource>();

        if (_as.Length > 0)
        {
            foreach (AudioSource a in _as)
            {
                Destroy(a);
            }
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            if (s.song == true)
            {
                s.source.outputAudioMixerGroup = musicM;
            }
            else
            {
                s.source.outputAudioMixerGroup = soundsM;
            }
        }
    }

}
