using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour {

    #region Variables
    private static AudioManager instance;
    public static AudioManager Instance {
        get {
            instance ??= FindFirstObjectByType<AudioManager>();
            return instance;
        }
    }

    [SerializeField] private AudioMixer master;

    [SerializeField] private float fadeInTime, fadeOutTime;
    [SerializeField] private float minPitchChange, maxPitchChange;
    //List of default volumes for all audiosources, to set after balancing the audios
    [SerializeField] private float mainSoundtrackDefaultVolume, secondarySoundtrackDefaultVolume, sfxDefaultVolume, dialogueDefaultVolume;

    private List<AudioSource> audioSources = new List<AudioSource>();

    private AudioSource genericAudioSource;

    [SerializeField] private AudioMixerGroup soundtrackOutput;
    [SerializeField] private AudioMixerGroup sfxOutput;
    [SerializeField] private AudioMixerGroup dialogueOutput;

    private Coroutine soundtrackCoroutine;
    private Coroutine sfxCoroutine;
    private Coroutine dialogueCoroutine;
    private enum TypeOfSound {
        Soundtrack,
        SFX,
        Dialogue
    }

    //RENAME
    private AudioMixerGroup genericAudioMixerGroup;

    public AudioMixer Master => master;
    public List<AudioSource> AudioSources => audioSources;
    #endregion

    #region LifeCycle
    private void Start() {
        DontDestroyOnLoad(this.gameObject);
        //FillAudioSources();
        //SetDefaultVolumes();
    }
    #endregion

    #region Base Play Methods
    //Play a soundtrack clip
    public void PlaySoundtrack(AudioClip soundtrackClip) {
        //If there's an audio source playing, stop it and the coroutine associated
        StopAudioSourcePlaying(soundtrackOutput);
        if(soundtrackCoroutine != null) {
            StopCoroutine(soundtrackCoroutine);
        }

        genericAudioSource = AudioSourcePoolManager.Instance.CheckPoolForAvailableAudioSource();
        genericAudioSource.outputAudioMixerGroup = soundtrackOutput;
        genericAudioSource.resource = soundtrackClip;
        genericAudioSource.Play();
        soundtrackCoroutine = StartCoroutine(AudioSourcePoolManager.Instance.StopWhenFinished(genericAudioSource));
    }
    //Play an SFX clip
    public void PlaySFX(AudioClip sfxClip) {
        StopAudioSourcePlaying(sfxOutput);
        if(sfxCoroutine != null) {
            StopCoroutine(sfxCoroutine);
        }

        genericAudioSource = AudioSourcePoolManager.Instance.CheckPoolForAvailableAudioSource();
        genericAudioSource.outputAudioMixerGroup = sfxOutput;
        genericAudioSource.resource = sfxClip;
        genericAudioSource.Play();
        sfxCoroutine = StartCoroutine(AudioSourcePoolManager.Instance.StopWhenFinished(genericAudioSource));
    }
    //Play a voice clip
    public void PlayDialogue(AudioClip dialogueClip /*, string voiceId*/) {
        StopAudioSourcePlaying(dialogueOutput);
        if(dialogueCoroutine != null) {
            StopCoroutine(dialogueCoroutine);
        }

        genericAudioSource = AudioSourcePoolManager.Instance.CheckPoolForAvailableAudioSource();
        genericAudioSource.outputAudioMixerGroup = dialogueOutput;
        genericAudioSource.resource = dialogueClip;
        genericAudioSource.Play();
        dialogueCoroutine = StartCoroutine(AudioSourcePoolManager.Instance.StopWhenFinished(genericAudioSource));
    }
    #endregion

    #region Play Methods Overrides

    ////Loop audio methods
    public void PlayLoopedSoundtrack(AudioClip soundtrackClip) {
        StopAudioSourcePlaying(soundtrackOutput);
        if(soundtrackCoroutine != null) {
            StopCoroutine(soundtrackCoroutine);
        }

        genericAudioSource = AudioSourcePoolManager.Instance.CheckPoolForAvailableAudioSource();
        genericAudioSource.outputAudioMixerGroup = soundtrackOutput;
        genericAudioSource.resource = soundtrackClip;
        SetAudioLoop(genericAudioSource);
        genericAudioSource.Play();
        soundtrackCoroutine = StartCoroutine(AudioSourcePoolManager.Instance.StopWhenFinished(genericAudioSource));
    }

    ////Fade in and out methods
    //public void PlayFadedSoundtrack(AudioClip soundtrackClip) {
    //    if (mainSoundtrackSource.isPlaying) {
    //        StopAllCoroutines();
    //        SetDefaultVolumes();

    //        IEnumerator fadeOut = FadeOut(mainSoundtrackSource, fadeOutTime);
    //        IEnumerator fadeIn = FadeIn(secondarySoundtrackSource, soundtrackClip, fadeInTime);

    //        StartCoroutine(fadeOut);
    //        StartCoroutine(fadeIn);
    //    }
    //    else if (secondarySoundtrackSource.isPlaying) {
    //        StopAllCoroutines();
    //        SetDefaultVolumes();

    //        IEnumerator fadeOut = FadeOut(secondarySoundtrackSource, fadeOutTime);
    //        IEnumerator fadeIn = FadeIn(mainSoundtrackSource, soundtrackClip, fadeInTime);

    //        StartCoroutine(fadeOut);
    //        StartCoroutine(fadeIn);
    //    }
    //    else {
    //        StopAllCoroutines();
    //        SetDefaultVolumes();

    //        IEnumerator fadeIn = FadeIn(mainSoundtrackSource, soundtrackClip, fadeInTime);

    //        StartCoroutine(fadeIn);
    //    }
    //}

    ////Random pitch methods
    //public void PlaySFXVariablePitch(AudioClip sfxClip) {
    //    StopAudioPlaying(sfxSource);

    //    float pitchChange = Random.Range(minPitchChange, maxPitchChange);
    //    sfxSource.pitch += pitchChange;
    //    sfxSource.resource = sfxClip;
    //    sfxSource.Play();
    //    sfxSource.pitch = 1;    //set back to default
    //}

    #endregion

    #region Utils
    //private void SetDefaultVolumes() {
    //    mainSoundtrackSource.volume = mainSoundtrackDefaultVolume;
    //    secondarySoundtrackSource.volume = secondarySoundtrackDefaultVolume;
    //    sfxSource.volume = sfxDefaultVolume;
    //    dialogueSource.volume = dialogueDefaultVolume;
    //}
    //private void FillAudioSources() {
    //    audioSources.Add(mainSoundtrackSource);
    //    audioSources.Add(secondarySoundtrackSource);
    //    audioSources.Add(sfxSource);
    //    audioSources.Add(dialogueSource);
    //}
    private void SetAudioLoop(AudioSource source) {
        source.loop = true;
    }
    private void UnsetAudioLoop(AudioSource source) {
        source.loop = false;
    }
    private void StopAudioPlaying(AudioSource audioSource) {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, AudioClip audioClip, float FadeTime) {
        //Reduce or increase this value to slow down or speed up the fade in effect
        float timeDiluation = 1;
        audioSource.volume = 0;
        audioSource.resource = audioClip;
        audioSource.Play();

        while (audioSource.volume < 1) {    //Substitute with settings volume value
            audioSource.volume += timeDiluation * Time.deltaTime / FadeTime;

            yield return null;
        }
    }
    private bool StopAudioSourcePlaying(AudioMixerGroup audioMixerGroup) {
        foreach (AudioSource audioSource in AudioSourcePoolManager.Instance.activePool.sources) {
            if (audioSource.outputAudioMixerGroup == audioMixerGroup) {
                if (audioSource.isPlaying) {
                    audioSource.Stop();
                    AudioSourcePoolManager.Instance.activePool.sources.Remove(audioSource);
                    AudioSourcePoolManager.Instance.inactivePool.sources.Add(audioSource);
                    audioSource.enabled = false;
                    return true;
                }
            }
        }
        return false;
    }
    #endregion

    //REMEMBER: Make UI elements register the event
    private void VolumeChange(ChangeEvent<int> evt, SliderInt slider) {

        genericAudioMixerGroup = master.FindMatchingGroups(slider.name)[0];

        //Inserire formula per la gestione del volume
        switch (slider.name) {
            case "Master":
                genericAudioMixerGroup.audioMixer.SetFloat("MasterVolume", 20 * Mathf.Log10((slider.value) / 100));

                break;
            case "Soundtrack":
                genericAudioMixerGroup.audioMixer.SetFloat("SoundtrackVolume", 20 * Mathf.Log10((slider.value) / 100));

                break;
            case "SFX":
                genericAudioMixerGroup.audioMixer.SetFloat("SFXVolume", 20 * Mathf.Log10((slider.value) / 100));

                break;
            case "Voice":
                genericAudioMixerGroup.audioMixer.SetFloat("VoiceVolume", 20 * Mathf.Log10((slider.value) / 100));

                break;
        }
    }
}
