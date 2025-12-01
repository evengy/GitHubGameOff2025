using System;
using System.Collections;
using UnityEngine;

public class AmbientManager : Singleton<AmbientManager>, IResetable
{
    [SerializeField] AudioClip intro;
    [SerializeField] AudioClip ambience;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sourceA;
    [SerializeField] private AudioSource sourceB;
    [SerializeField] private AudioSource rainSource;

    [Header("Fade Settings")]
    [SerializeField] private float crossfadeDuration = 2f;
    [SerializeField] private float maxAmbientVolume = 1f;

    private Coroutine crossfadeCoroutine;
    private bool isAFadingToB;

    private void Start()
    {
        sourceA.volume = maxAmbientVolume;
        sourceB.volume = 0f;

        sourceA.Play();

        GameStateManager.Instance.StateUpdated += Instance_StateUpdated;
    }

    private void Instance_StateUpdated()
    {
        if (GameStateManager.Instance.GameState.Equals(GameState.InGame))
        {
            PlayIntroThenLoop(intro, ambience);
            rainSource.Stop();
            rainSource.Play();
        }
    }

    public void ResetBackToDefault()
    {
        sourceA.Stop();
        sourceB.Stop();
        sourceA.volume = 0;
        sourceB.volume = 0;
        StopAllCoroutines();
    }

    public void CrossfadeTo(AudioClip newClip)
    {
        StopAllCoroutines();

        AudioSource fadeOutSource = isAFadingToB ? sourceB : sourceA;
        AudioSource fadeInSource = isAFadingToB ? sourceA : sourceB;

        if (fadeOutSource.clip != newClip)
        {
            fadeInSource.clip = newClip;
            fadeInSource.volume = 0f;
            fadeInSource.loop = true;
            fadeOutSource.loop = false;
            if (!fadeInSource.isPlaying)
                fadeInSource.Play();

            crossfadeCoroutine = StartCoroutine(CrossfadeRoutine(fadeOutSource, fadeInSource, crossfadeDuration));

            isAFadingToB = !isAFadingToB;
        }
    }

    public void PlayIntroThenLoop(AudioClip introClip, AudioClip loopClip)
    {
        AudioSource introSource = isAFadingToB ? sourceA : sourceB;
        CrossfadeTo(introClip);
        StartCoroutine(PlayLoopAfterIntro(introSource, introClip, loopClip));
    }

    private IEnumerator CrossfadeRoutine(AudioSource fadeOut, AudioSource fadeIn, float duration)
    {
        float time = 0f;
        float startVolOut = fadeOut.volume;
        float startVolIn = fadeIn.volume;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            fadeOut.volume = Mathf.Lerp(startVolOut, 0f, t);
            fadeIn.volume = Mathf.Lerp(startVolIn, maxAmbientVolume, t);

            yield return null;
        }

        fadeOut.volume = 0f;
        fadeIn.volume = maxAmbientVolume;

        fadeOut.Stop();
    }

    private IEnumerator PlayLoopAfterIntro(AudioSource introSource, AudioClip introClip, AudioClip loopClip)
    {
        while (!introSource.isPlaying)
            yield return null;

        // Need sample check for webgl
        int lastSamples = 0;

        while (true)
        {
            int currentSamples = introSource.timeSamples;

            if (currentSamples > lastSamples)
            {
                lastSamples = currentSamples;

                if (currentSamples >= introClip.samples - 2048)
                    break;
            }

            yield return null;
        }

        CrossfadeTo(loopClip);
    }
}
