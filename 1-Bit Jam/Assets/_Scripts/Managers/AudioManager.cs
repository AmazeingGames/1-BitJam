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
    [SerializeField] AudioSource baseMusicSource;
    [SerializeField] AudioSource devilishMusicSource;

    [SerializeField] AudioSource sfxSouce;

    public enum EventSounds { ColorSwap, UIClick, HeavenlyWalk, DevilishWalk, HeavenAmbience, DevilishAmbience }

    Dictionary<EventSounds, EventReference> SoundTypeToReference;

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
    }

    public void PlayAudioClip(EventSounds sound, Vector3 origin)
    {
        PlayAudioClip(SoundTypeToReference[sound], origin);
    }

    public void PlayAudioClip(EventReference sound, Vector3 origin)
    {
        RuntimeManager.PlayOneShot(sound, origin);
    }
}
