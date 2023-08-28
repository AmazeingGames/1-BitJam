using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [field: Header("SFX")]
    [field: SerializeField] public EventReference ColorSwapSound { get; private set; }
    [field: SerializeField] public EventReference HeavenlyWalkSound { get; private set; }
    [field: SerializeField] public EventReference DevilishWalkSound { get; private set; }
    [field: SerializeField] public EventReference UIClickSound { get; private set; }
    [field: SerializeField] public EventReference HeavenlyAmbience { get; private set; }
    [field: SerializeField] public EventReference DevilishAmbience { get; private set; }


    [Header("Music")]
    [SerializeField] float fadeInTime;
    [SerializeField] float fadeOutTime;

    [SerializeField] AudioSource heavenlyMusicSource;
    [SerializeField] AudioSource baseMusicSource;
    [SerializeField] AudioSource devilishMusicSource;

    [SerializeField] AudioSource sfxSouce;

    EventInstance heavenAmbience;
    EventInstance hellAmbience;

    public enum EventSounds { ColorSwap, UIClick, HeavenlyWalk, DevilishWalk, HeavenAmbience, DevilishAmbience }

    public enum EventInstances { HeavenAmbience,  DevilishAmbience }

    bool isFading = false;

    Dictionary<EventSounds, EventReference> SoundTypeToReference;
    Dictionary<EventInstances, EventInstance> SoundTypeToInstance;

    List<EventInstance> eventInstances = new();

    bool initializedAmbience;

    void OnEnable()
    {
        GameManager.OnStateEnter += HandleLevelStart;
    }

    void OnDisable()
    {
        GameManager.OnStateEnter -= HandleLevelStart;
    }

    void Start()
    {
        SoundTypeToReference = new()
        {
            { EventSounds.ColorSwap,        ColorSwapSound },
            { EventSounds.UIClick,          UIClickSound},
            { EventSounds.DevilishWalk,     DevilishWalkSound },
            { EventSounds.HeavenlyWalk,     HeavenlyWalkSound },
            { EventSounds.HeavenAmbience,   HeavenlyAmbience },
            { EventSounds.DevilishAmbience, DevilishAmbience },
        };

        SoundTypeToInstance = new()
        {
            { EventInstances.HeavenAmbience,    heavenAmbience },
            { EventInstances.DevilishAmbience,  hellAmbience },
        };
    }

    public void PlayAudioClip(EventSounds sound, Vector3 origin)
    {
        PlayAudioClip(SoundTypeToReference[sound], origin);
    }

    public void PlayAudioClip(EventReference sound, Vector3 origin)
    {
        RuntimeManager.PlayOneShot(sound, origin);
    }
 
    public void SetAmbienceParameter(EventInstances instance, string instanceParameterName, float value)
    {
        SetAmbienceParameter(SoundTypeToInstance[instance], instanceParameterName, value);
    }

    //Not sure why but this function doesn't work at all
    public void SetAmbienceParameter(EventInstance instance, string instanceParameterName, float value)
    {

        instance.setParameterByName(instanceParameterName, value);

        instance.getParameterByName(instanceParameterName, out float realValue);

        Debug.Log($"Set (instance) {instance} (parameter) {instanceParameterName} to (value) {value} | (real value) {realValue}");

    }

    public IEnumerator FadeTracks(bool playHeavenly, bool fadeSimultaneously)
    {
        AudioSource fadeInTrack = playHeavenly switch
        {
            true => heavenlyMusicSource,
            false => devilishMusicSource,
        };

        AudioSource fadeOutTrack = playHeavenly switch
        {
            true => devilishMusicSource,
            false => heavenlyMusicSource,
        };

        StartTrackFade(false, fadeOutTrack);

        if (fadeSimultaneously)
        {
            StartTrackFade(true, fadeInTrack);

            yield break;
        }

        while (true)
        {
            if (!isFading)
            {
                StartTrackFade(true, fadeInTrack);
                yield break;
            }

            yield return null;
        }
    }

    void HandleLevelStart(GameManager.GameState gameState)
    {
        if (gameState != GameManager.GameState.LevelStart)
            return;

        InitializeAmbience();
    }

    void InitializeAmbience()
    {
        if (initializedAmbience)
            return;

        Debug.Log("Initialized Audio");

        initializedAmbience = true;

        heavenAmbience = CreateEventInstance(EventSounds.HeavenAmbience);
        hellAmbience = CreateEventInstance(EventSounds.DevilishAmbience);

        heavenAmbience.start();
        hellAmbience.start();
    }

    void StartTrackFade(bool fadeTrackIn, AudioSource trackToFade)
    {
        float fadeTime = fadeTrackIn ? fadeInTime : fadeOutTime;
        float target = fadeTrackIn ? 1 : 0;

        StartCoroutine(TrackFade(target, fadeTime, trackToFade));
    }

    IEnumerator TrackFade(float target, float fadeTime, AudioSource trackToFade)
    {
        Mathf.Clamp(target, 0, 1);

        isFading = true;

        float start = trackToFade.volume;
        float timer = 0;
        float time;

        while (true)
        {
            timer += Time.deltaTime;

            time = timer / fadeTime;

            trackToFade.volume = Mathf.Lerp(start, target, time);

            if (trackToFade.volume == target)
            {
                Debug.Log("reachedTarget");
                isFading = false;
                yield break;
            }

            yield return null;
        }

    }

    void StartTrackFade(bool fadeIn, ColorSwap.Color trackTheme)
    {
        AudioSource trackToFade = trackTheme switch
        {
            ColorSwap.Color.White => heavenlyMusicSource,
            ColorSwap.Color.Black => devilishMusicSource,
            ColorSwap.Color.Neutral => baseMusicSource,
            ColorSwap.Color.Null => throw new System.NotImplementedException(),
            _ => throw new System.NotImplementedException(),
        };

        StartTrackFade(fadeIn, trackToFade);
    }

    public EventInstance CreateEventInstance(EventSounds sound) => CreateEventInstance(SoundTypeToReference[sound]);

    public EventInstance CreateEventInstance(EventReference sound)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);

        eventInstances.Add(eventInstance);

        return eventInstance;
    }

    void CleanUp()
    {
        Debug.Log("Cleanup");

        foreach (var eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    void OnDestroy()
    {
        CleanUp();
    }
}
