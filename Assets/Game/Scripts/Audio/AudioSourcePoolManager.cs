using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioSourcePoolManager : MonoBehaviour {

    private static AudioSourcePoolManager instance;
    public static AudioSourcePoolManager Instance {
        get {
            instance ??= FindFirstObjectByType<AudioSourcePoolManager>(); 
            return instance;
        }
    }

    public AudioSourcePool activePool;
    public AudioSourcePool inactivePool;

    //pool management
    public AudioSource CheckPoolForAvailableAudioSource() {
        AudioSource source = null;
        bool sourceRemoved = false;
        foreach (AudioSource tempSource in inactivePool.sources) {
            if (tempSource != null) {
                source = tempSource;
                source.enabled = true;
                sourceRemoved = true;
            }
        }

        if (sourceRemoved) {
            inactivePool.sources.Remove(source);
        }

        if (source == null) {
            source = activePool.GenerateAudioSource();
        }
        activePool.SetAudioSourceDefaultValues(source);
        activePool.sources.Add(source);

        return source;
    }

    //Basic play method
    //public void Play(AudioClip audioClip) {
    //    AudioSource audioSource = CheckPoolForAvailableAudioSource();
    //    if (audioSource == null) {
    //        audioSource = GenerateAudioSource();
    //        Instantiate(audioSource, audioSourcesParent);
    //    }
    //    else
    //        audioSource.enabled = true;

    //    audioSource.resource = audioClip;
    //    audioSource.Play();
    //    StartCoroutine(StopWhenFinished(audioSource));
    //}
    ////loop play
    //public void Play(AudioClip audioClip, bool loop) {
    //    AudioSource audioSource = CheckPoolForAvailableAudioSource();
    //    if (audioSource == null)
    //        audioSource = GenerateAudioSource();
    //    else
    //        audioSource.enabled = true;

    //    audioSource.resource = audioClip;
    //    audioSource.loop = loop;
    //    audioSource.Play();
    //    StartCoroutine(StopWhenFinished(audioSource));
    //}
    ////loop and play on awake play
    //public void Play(AudioClip audioClip, bool loop, bool playOnAwake) {
    //    AudioSource audioSource = CheckPoolForAvailableAudioSource();
    //    if (audioSource == null)
    //        audioSource = GenerateAudioSource();
    //    else
    //        audioSource.enabled = true;

    //    audioSource.resource = audioClip;
    //    audioSource.loop = loop;
    //    audioSource.playOnAwake = playOnAwake;
    //    audioSource.Play();
    //    StartCoroutine(StopWhenFinished(audioSource));
    //}

    public IEnumerator StopWhenFinished(AudioSource audioSource) {
        while (audioSource.isPlaying) {
            yield return new WaitForEndOfFrame();
        }
        DeactivateAudioSource(audioSource);
    }
      public void DeactivateAudioSource(AudioSource audioSource) {
        inactivePool.sources.Add(audioSource);
        activePool.sources.Remove(audioSource);
        audioSource.enabled = false;
    }
}
