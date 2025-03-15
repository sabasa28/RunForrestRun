using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip musicClipGameplay;
    [SerializeField] AudioClip musicClipMenu;
    [Range(0.0f, 1.0f)] public float sfxVolume;
    [Range(0.0f, 1.0f)] public float musicVolume;
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
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
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
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            musicSource.clip = musicClipMenu;
        }
        else
        { 
            musicSource.clip = musicClipGameplay;
        }
        musicSource.loop = true;
        musicSource.Play();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            musicSource.clip = musicClipMenu;
        }
        else
        {
            musicSource.clip = musicClipGameplay;
        }
        musicSource.loop = true;
        musicSource.Play();
    }

    public void UpdateMusicVolume(float newValue)
    {
        musicVolume = newValue;
        musicSource.volume = musicVolume;
    }

    public void PlaySFX(AudioClip clip, float volume = 0.2f)
    {
        audioSource.PlayOneShot(clip, volume * sfxVolume);
    }

    public void PlayUISelect()
    {
        audioSource.PlayOneShot(uiSelectClip, 0.2f * sfxVolume);
    }

    public void PlayUIBack()
    {
        audioSource.PlayOneShot(uiBackClip, 0.2f * sfxVolume);
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
