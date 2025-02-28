using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip musicClip;
    [SerializeField, Range(0.0f, 1.0f)] float sfxVolume;
    [SerializeField, Range(0.0f, 1.0f)] float musicVolume;
    [SerializeField] AudioClip uiSelectClip;
    [SerializeField] AudioClip uiBackClip;
    static AudioManager instance;

    public static AudioManager Get()
    {
        return instance;
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    void Start()
    {
        audioSource.volume = sfxVolume;
        musicSource.volume = musicVolume;
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 0.2f)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    public void PlayUISelect()
    {
        audioSource.PlayOneShot(uiSelectClip, 0.2f);
    }

    public void PlayUIBack()
    {
        audioSource.PlayOneShot(uiBackClip, 0.2f);
    }

    public void PauseMusic(bool pause)
    {
        if (pause)
        {
            musicSource.Pause();
        }
        else 
        {
            musicSource.UnPause();
        }
    }
}
