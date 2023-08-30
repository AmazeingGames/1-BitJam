using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [field: Header("SFX")]
    [field: SerializeField] public EventReference SwapToHeavenSound { get; private set; }
    [field: SerializeField] public EventReference SwapToHellSound { get; private set; }
    [field: SerializeField] public EventReference HeavenlyWalkSound { get; private set; }
    [field: SerializeField] public EventReference DevilishWalkSound { get; private set; }
    [field: SerializeField] public EventReference UIClickSound { get; private set; }
    [field: SerializeField] public EventReference HeavenlyAmbience { get; private set; }
    [field: SerializeField] public EventReference DevilishAmbience { get; private set; }


    [Header("Music")]
    [SerializeField] float fadeInTime;
    [SerializeField] float fadeOutTime;

    [SerializeField] AudioSource heavenlyMusicSource;

    [SerializeField] AudioSource sfxSouce;

    public enum EventSounds { SwapToHell, SwapToHeaven, UIClick, HeavenlyWalk, DevilishWalk, HeavenAmbience, DevilishAmbience }

    Dictionary<EventSounds, EventReference> SoundTypeToReference;

    public EventInstance HeavenAmbienceInstance { get; private set; }
    public EventInstance DevilishAmbienceInstance { get; private set; }

    readonly List<EventInstance> EventInstances = new();

    void Start()
    {
        SoundTypeToReference = new()
        {
            { EventSounds.SwapToHell,       SwapToHellSound },
            { EventSounds.SwapToHeaven,     SwapToHeavenSound },
            { EventSounds.UIClick,          UIClickSound},
            { EventSounds.DevilishWalk,     DevilishWalkSound },
            { EventSounds.HeavenlyWalk,     HeavenlyWalkSound },
            { EventSounds.HeavenAmbience,   HeavenlyAmbience },
            { EventSounds.DevilishAmbience, DevilishAmbience },
        };

        //Creates the instance
        HeavenAmbienceInstance = CreateEventInstance(EventSounds.HeavenAmbience);
        DevilishAmbienceInstance = CreateEventInstance(EventSounds.DevilishAmbience);
    }

    void OnDestroy()
    {
        CleanUp();    
    }

    public void PlayAudioClip(EventSounds sound, Vector3 origin)
    {
        PlayAudioClip(SoundTypeToReference[sound], origin);
    }

    public void PlayAudioClip(EventReference sound, Vector3 origin)
    {
        RuntimeManager.PlayOneShot(sound, origin);
    }

    //Creates the instance
    EventInstance CreateEventInstance(EventSounds eventSound) => CreateEventInstance(SoundTypeToReference[eventSound]);

    EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        
        EventInstances.Add(eventInstance);

        return eventInstance;
    }

    void CleanUp()
    {
        foreach (var eventInstance in EventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }
}
