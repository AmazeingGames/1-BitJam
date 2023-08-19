using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] float fadeInTime;
    [SerializeField] float fadeOutTime;

    [SerializeField] AudioSource heavenlyMusicSource;
    [SerializeField] AudioSource baseMusicSource;
    [SerializeField] AudioSource devilishMusicSource;

    [SerializeField] AudioSource sfxSouce;

    bool isFading = false;

    public void PlayAudioClip(AudioClip clip)
    {
        sfxSouce.PlayOneShot(clip);
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    void Start()
    {

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
}
