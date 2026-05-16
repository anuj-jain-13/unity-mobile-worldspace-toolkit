using UnityEngine;

public class AudioPlayer : MonoBehaviourSingleton<AudioPlayer>
{
    // ------------------------------
    // 🔊 Persistent Multipliers
    // ------------------------------
    public static float MusicVolume
    {
        get => PlayerPrefs.GetFloat("AudioPlayer.MusicVolume", 1f);   // user multiplier
        private set { PlayerPrefs.SetFloat("AudioPlayer.MusicVolume", Mathf.Clamp01(value)); PlayerPrefs.Save(); }
    }

    public static float SoundVolume
    {
        get => PlayerPrefs.GetFloat("AudioPlayer.SoundVolume", 1f);   // user multiplier
        private set { PlayerPrefs.SetFloat("AudioPlayer.SoundVolume", Mathf.Clamp01(value)); PlayerPrefs.Save(); }
    }

    public static bool IsVibrationOn
    {
        get => PlayerPrefs.GetInt("AudioPlayer.VibrationOnOff", 1) == 1;
        private set { PlayerPrefs.SetInt("AudioPlayer.VibrationOnOff", value ? 1 : 0); PlayerPrefs.Save(); }
    }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private AudioClip bgmClip;
    private AudioClip tapClip;

    // Keep reference volumes separately
    private float currentMusicBaseVolume = 1f;

    #region Overriden Methods
    protected override void SingletonAwakened()
    {
        addAudioSources();  //Add Audio Sources dynamically.
        //loadAudioData();
        setMyDontDestroyName();
    }
    #endregion

    private void addAudioSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    //private void loadAudioData()
    //{
    //    Debug.Log("AudioPlayer.LoadAudioData()...IsMusicOn : " + IsMusicOn + ", IsSoundOn : " + IsSoundOn);
    //}

    private void setMyDontDestroyName()
    {
        gameObject.name = "[" + Instance.GetType().ToString() + "]";
    }

    public static void StartAudioPlayer()
    {
        Debug.Log("AudioPlayer is Started..." + Instance);
        EventManager.triggerEvent(AppEventsId.AudioPlayerIsReady, Instance.gameObject);
    }

    public static void SetSoundClips(AudioClip music, AudioClip sfx)
    {
        if (music != null)
            Instance.bgmClip = music;
        if (sfx != null)
            Instance.tapClip = sfx;
    }

    public static void SetMusicVolume(float multiplier)
    {
        MusicVolume = multiplier;

        if (Instance.musicSource != null)
        {
            if (MusicVolume > 0f)
            {
                if (Instance.musicSource.clip != null)
                {
                    if (!Instance.musicSource.isPlaying)
                    {
                        if (Instance.musicSource.time > 0f)
                            Instance.musicSource.UnPause();
                        else
                            Instance.musicSource.Play();
                    }

                    applyMusicVolume();
                }
            }
            else
            {
                // slider is at 0 => mute music
                if (Instance.musicSource.isPlaying)
                    Instance.musicSource.Pause();
            }
        }
    }

    public static void PlayActiveMusic(float baseVolume = 1f) =>
        PlayMusicClip(Instance.bgmClip, baseVolume);


    public static void StopActiveMusic()
    {
        if (Instance.musicSource.isPlaying)
            Instance.musicSource.Stop();
    }

    public static void PlayMusicClip(AudioClip clip, float baseVolume = 1f)
    {
        if (clip == null) return;

        if (Instance.musicSource.isPlaying)
            Instance.musicSource.Stop();

        Instance.musicSource.clip = clip;
        Instance.currentMusicBaseVolume = baseVolume;

        if (MusicVolume > 0f)
        {
            applyMusicVolume();
            Instance.musicSource.Play();
        }
    }

    private static void applyMusicVolume()
    {
        if (Instance.musicSource.clip != null)
            Instance.musicSource.volume = Instance.currentMusicBaseVolume * MusicVolume;
    }

    public static void SetSoundVolume(float multiplier)
    {
        SoundVolume = multiplier;
        // applied during PlaySoundClip
    }

    public static void PlaySoundClip(AudioClip clip, float baseVolume = 1f)
    {
        if (clip == null) return;
        if (SoundVolume > 0f)
            Instance.sfxSource.PlayOneShot(clip, baseVolume * SoundVolume);
    }

    public static void PlayTapSound(float baseVolume = 1f) =>
        PlaySoundClip(Instance.tapClip, baseVolume);

    public static void ToggleVibration(bool isOn)
    {
        IsVibrationOn = isOn;
        EventManager.triggerEvent(AppEventsId.AudioPlayerToggleVibration, Instance.gameObject, IsVibrationOn);
    }

    public AudioSource GetMusicSource() => musicSource;
    public AudioSource GetSfxSource() => sfxSource;

    public static bool IsMusicOn => MusicVolume > 0f;
    public static bool IsSoundOn => SoundVolume > 0f;
}