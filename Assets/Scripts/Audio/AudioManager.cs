using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public enum SfxType
{
    Death,
    LevelUp,
    Pause,
    Resume,
    Walk,
    Dash,
    EnemyDeath,
    Button
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private int sfxPoolSize = 5;

    [Space]
    [SerializeField] private AudioClip backgroundMusic;

    [Space]
    [SerializeField] private AudioClip deathSfx;
    [SerializeField] private AudioClip levelUpSfx;
    [SerializeField] private AudioClip pauseSfx;
    [SerializeField] private AudioClip resumeSfx;
    [SerializeField] private AudioClip walkSfx;
    [SerializeField] private AudioClip dashSfx;
    [SerializeField] private AudioClip enemyDeathSfx;
    [SerializeField] private AudioClip buttonSfx;
    [SerializeField] private AudioClip[] abilitySfx = new AudioClip[4];

    [Space]
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.5f;
    [SerializeField, Range(0f, 1f)] private float sfxVolume = 0.5f;

    [Space]
    [SerializeField, Range(0.0f, 3.0f)] private float fadeDuration = 0.5f;

    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find an existing instance in the scene
                _instance = FindFirstObjectByType<AudioManager>();
                if (_instance == null)
                {
                    // Create new GameObject if none exists
                    var go = new GameObject("AudioManager");
                    _instance = go.AddComponent<AudioManager>();
                }
                _instance.EnsureAudioSources();
            }
            return _instance;
        }
        private set { _instance = value; }
    }

    public UnityEvent<SfxType> OnSfxPlayed = new UnityEvent<SfxType>();

    private PlayerStats playerStats;
    private Coroutine fadeCoroutine;
    private Dictionary<SfxType, AudioClip> sfxClips;
    private Queue<AudioSource> sfxPool;
    private int sfxPoolIndex = 0;
    private readonly object singletonLock = new object();

    private void Awake()
    {
        lock (singletonLock)
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
        EnsureAudioSources();
        InitSfxClips();
        InitSfxPool();
        SetInitialVolumes();
        PlayBackgroundMusic();
    }

    private void EnsureAudioSources()
    {
        // Always use AudioSources on this GameObject only
        var sources = GetComponents<AudioSource>();
        if (sources.Length == 0) {
            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
        } else if (sources.Length == 1) {
            musicSource = sources[0];
            sfxSource = gameObject.AddComponent<AudioSource>();
        } else {
            musicSource = sources[0];
            sfxSource = sources[1];
        }
        // Configure musicSource
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        // Configure sfxSource
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;
    }

    private void OnEnable()
    {
    SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    private void OnDisable()
    {
    SceneManager.sceneLoaded -= OnSceneLoaded; 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => FindAndSubscribePlayerStats();

    private void FindAndSubscribePlayerStats()
    {
        UnsubscribeFromPlayer();
        playerStats = FindFirstObjectByType<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.OnDeath += StopBackgroundMusic;
            playerStats.OnLevelUp += PlayLevelUpSfx;
        }
    }

    private void UnsubscribeFromPlayer()
    {
        if (playerStats != null)
        {
            playerStats.OnDeath -= StopBackgroundMusic;
            playerStats.OnLevelUp -= PlayLevelUpSfx;
            playerStats = null;
        }
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic == null || musicSource == null)
        {
            Debug.LogWarning("AudioManager Missing background music or music source");
            return;
        }
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        if (musicSource.isPlaying)
            StartFade(musicSource, 0f, fadeDuration, true, () => StartMusicCrossfade(backgroundMusic));
        else
            StartMusicCrossfade(backgroundMusic);
        FindAndSubscribePlayerStats();
    }

    private void StartMusicCrossfade(AudioClip newClip)
    {
        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.volume = 0f;
        musicSource.Play();
        StartFade(musicSource, musicVolume, fadeDuration);
    }

    private void StopBackgroundMusic()
    {
        if (musicSource != null)
            StartFade(musicSource, 0f, fadeDuration, true);
        PlaySfx(SfxType.Death);
    }

    public void PlaySfx(SfxType type, float? customVolume = null, float? customPitch = null)
    {
        if (sfxClips == null || !sfxClips.TryGetValue(type, out var clip) || clip == null)
        {
            Debug.LogWarning($"AudioManager SFX clip for {type} not assigned");
            return;
        }
        var source = GetNextSfxSource();
        source.volume = customVolume ?? sfxVolume;
        source.pitch = customPitch ?? 1f;
        source.PlayOneShot(clip);
        OnSfxPlayed.Invoke(type);
    }

    public void PlaySfxAtPosition(SfxType type, Vector3 position, float? customVolume = null, float? customPitch = null)
    {
        if (sfxClips == null || !sfxClips.TryGetValue(type, out var clip) || clip == null)
        {
            Debug.LogWarning($"AudioManager SFX clip for {type} not assigned");
            return;
        }
        var tempGO = new GameObject($"SFX_{type}");
        tempGO.transform.position = position;
        var tempSource = tempGO.AddComponent<AudioSource>();
        tempSource.spatialBlend = 1f;
        tempSource.volume = customVolume ?? sfxVolume;
        tempSource.pitch = customPitch ?? 1f;
        tempSource.PlayOneShot(clip);
        Destroy(tempGO, clip.length / tempSource.pitch);
        OnSfxPlayed.Invoke(type);
    }

    public static void PlayAbilitySfxStatic(int index, float? customVolume = null, float? customPitch = null)
    {
        if (Instance == null)
        {
            Debug.LogWarning("AudioManager instance not found when trying to play ability SFX");
            return;
        }
        Instance.PlayAbilitySfx(index, customVolume, customPitch);
    }

    public void PlayAbilitySfx(int index, float? customVolume = null, float? customPitch = null)
    {
        if (abilitySfx == null || index < 0 || index >= abilitySfx.Length)
        {
            Debug.LogWarning($"AudioManager Ability SFX index {index} is out of range");
            return;
        }
        var clip = abilitySfx[index];
        if (clip == null)
        {
            Debug.LogWarning($"AudioManager Ability SFX at index {index} not assigned");
            return;
        }
        var source = GetNextSfxSource();
        source.volume = customVolume ?? sfxVolume;
        source.pitch = customPitch ?? 1f;
        source.PlayOneShot(clip);
        OnSfxPlayed.Invoke(SfxType.Button); 
    }

    public AudioSource PlayLoopingSfx(SfxType type, float? customVolume = null, float? customPitch = null)
    {
        if (sfxClips == null || !sfxClips.TryGetValue(type, out var clip) || clip == null)
        {
            Debug.LogWarning($"AudioManager SFX clip for {type} not assigned");
            return null;
        }
        var source = GetNextSfxSource();
        source.clip = clip;
        source.loop = true;
        source.volume = customVolume ?? sfxVolume;
        source.pitch = customPitch ?? 1f;
        source.Play();
        return source;
    }

    public static void StopLoopingSfx(AudioSource source)
    {
        if (source != null)
        {
            source.Stop();
            source.loop = false;
            source.clip = null;
        }
    }

    private void PlayLevelUpSfx(int level)
    {
        PlaySfx(SfxType.LevelUp);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        foreach (var src in sfxPool)
            if (src != null)
                src.volume = sfxVolume;
    }

    public void PauseAll()
    {
        musicSource?.Pause();
        foreach (var src in sfxPool)
            src?.Pause();
    }

    public void ResumeAll()
    {
        musicSource?.UnPause();
        foreach (var src in sfxPool)
            src?.UnPause();
    }

    private void SetInitialVolumes()
    {
        if (musicSource != null)
            musicSource.volume = 0f;
        if (sfxPool != null)
            foreach (var src in sfxPool)
                if (src != null)
                    src.volume = sfxVolume;
    }

    private void InitSfxClips()
    {
        sfxClips = new Dictionary<SfxType, AudioClip>
        {
            { SfxType.Death, deathSfx },
            { SfxType.LevelUp, levelUpSfx },
            { SfxType.Pause, pauseSfx },
            { SfxType.Resume, resumeSfx },
            { SfxType.Walk, walkSfx },
            { SfxType.Dash, dashSfx },
            { SfxType.EnemyDeath, enemyDeathSfx },
            { SfxType.Button, buttonSfx }
        };
    }

    private void InitSfxPool()
    {
        sfxPool = new Queue<AudioSource>(sfxPoolSize);
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            sfxSource.loop = false;
            sfxSource.volume = sfxVolume;

            Debug.LogWarning("AudioManager sfxSource not assigned");
            return;
        }
        sfxPool.Enqueue(sfxSource);
        for (int i = 1; i < sfxPoolSize; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = false;
            src.volume = sfxVolume;
            sfxPool.Enqueue(src);
        }
    }

    private AudioSource GetNextSfxSource()
    {
        if (sfxPool == null || sfxPool.Count == 0)
            return null;
        var src = sfxPool.Dequeue();
        sfxPool.Enqueue(src);
        return src;
    }

    public void StartFade(AudioSource source, float targetVolume, float duration, bool stopAfterFade = false, Action onComplete = null)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeRoutine(source, targetVolume, duration, stopAfterFade, onComplete));
    }

    private IEnumerator FadeRoutine(AudioSource source, float targetVolume, float duration, bool stopAfterFade, Action onComplete)
    {
        if (source == null)
            yield break;
        float start = source.volume;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(start, targetVolume, Mathf.Clamp01(t / Mathf.Max(0.0001f, duration)));
            yield return null;
        }
        source.volume = targetVolume;
        if (stopAfterFade && source.isPlaying)
            source.Stop();
        fadeCoroutine = null;
        onComplete?.Invoke();
    }

#if UNITY_EDITOR
    /// <summary>
    /// Editor utility to preview SFX in the inspector
    /// </summary>
    [ContextMenu("Preview Button SFX")]
    private void PreviewButtonSfx()
    {
        PlayButtonSfx();
    }
#endif

    public void PlayButtonSfx() => PlaySfx(SfxType.Button);

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (musicSource == null)
            Debug.LogWarning("AudioManager musicSource not assigned", this);
        if (sfxSource == null)
            Debug.LogWarning("AudioManager sfxSource not assigned", this);
        if (backgroundMusic == null)
            Debug.LogWarning("AudioManager backgroundMusic not assigned", this);
        if (deathSfx == null)
            Debug.LogWarning("AudioManager deathSfx not assigned", this);
        if (levelUpSfx == null)
            Debug.LogWarning("AudioManager levelUpSfx not assigned", this);
        if (pauseSfx == null)
            Debug.LogWarning("AudioManager pauseSfx not assigned", this);
        if (resumeSfx == null)
            Debug.LogWarning("AudioManager resumeSfx not assigned", this);
        if (walkSfx == null)
            Debug.LogWarning("AudioManager walkSfx not assigned", this);
        if (dashSfx == null)
            Debug.LogWarning("AudioManager dashSfx not assigned", this);
        if (enemyDeathSfx == null)
            Debug.LogWarning("AudioManager enemyDeathSfx not assigned", this);
        if (buttonSfx == null)
            Debug.LogWarning("AudioManager buttonSfx not assigned", this);
        if (abilitySfx == null || abilitySfx.Length != 4)
            Debug.LogWarning("AudioManager abilitySfx array should have 4 slots", this);
        else
            for (int i = 0; i < abilitySfx.Length; i++)
                if (abilitySfx[i] == null)
                    Debug.LogWarning($"AudioManager abilitySfx[{i}] not assigned", this);
    }
#endif
}
