using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music Settings")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;

    [Header("Pool Settings")]
    [SerializeField] private int poolSize = 10;

    private Queue<AudioSource> availableSources = new Queue<AudioSource>();
    private List<AudioSource> allSources = new List<AudioSource>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializePool();

        DontDestroyOnLoad(gameObject);
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = new GameObject("PooledAudioSource");
            obj.transform.parent = transform;

            AudioSource source = obj.AddComponent<AudioSource>();
            source.playOnAwake = false;

            availableSources.Enqueue(source);
            allSources.Add(source);
        }
    }

    public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null)
            return;

        AudioSource source = GetSource();
        source.transform.position = position;

        source.clip = clip;
        source.volume = volume;
        source.Play();

        StartCoroutine(ReturnToPool(source, clip.length));
    }

    private AudioSource GetSource()
    {
        if (availableSources.Count > 0)
            return availableSources.Dequeue();

        GameObject obj = new GameObject("DynamicAudioSource");
        obj.transform.parent = transform;

        AudioSource source = obj.AddComponent<AudioSource>();
        source.playOnAwake = false;

        allSources.Add(source);

        return source;
    }

    private IEnumerator ReturnToPool(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);

        source.Stop();
        availableSources.Enqueue(source);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SwitchMusic(scene.name);
    }

    private void SwitchMusic(string sceneName)
    {
        AudioClip targetClip = null;

        if (sceneName == "MainMenu")
            targetClip = menuMusic;
        else
            targetClip = gameplayMusic;

        if (musicSource.clip == targetClip)
            return;

        StartCoroutine(FadeToMusic(targetClip, 1f));
    }

    private IEnumerator FadeToMusic(AudioClip newClip, float duration)
    {
        float startVolume = musicSource.volume;

        // Fade out
        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.unscaledDeltaTime / duration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.Play();

        // Fade in
        while (musicSource.volume < startVolume)
        {
            musicSource.volume += startVolume * Time.unscaledDeltaTime / duration;
            yield return null;
        }

        musicSource.volume = startVolume;
    }
}
