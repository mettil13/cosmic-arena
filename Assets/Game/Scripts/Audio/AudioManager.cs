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

    //Mixers and audio sources
    [SerializeField] private AudioMixer master;
    [SerializeField] private AudioSource mainSoundtrackSource, secondarySoundtrackSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource dialogueSource;

    [SerializeField] private float fadeInTime, fadeOutTime;
    [SerializeField] private float minPitchChange, maxPitchChange;
    //List of default volumes for all audiosources, to set after balancing the audios
    [SerializeField] private float mainSoundtrackDefaultVolume, secondarySoundtrackDefaultVolume, sfxDefaultVolume, dialogueDefaultVolume;

    private List<AudioSource> audioSources = new List<AudioSource>();

    //RENAME
    private AudioMixerGroup genericAudioMixerGroup;

    public AudioMixer Master => master;
    public List<AudioSource> AudioSources => audioSources;
    #endregion

    #region LifeCycle
    private void Start() {
        FillAudioSources();
        SetDefaultVolumes();
    }
    #endregion

    #region Base Play Methods
    //Play a soundtrack clip
    public void PlaySoundtrack(AudioClip soundtrackClip) {
        StopAudioPlaying(mainSoundtrackSource);
        UnsetAudioLoop(mainSoundtrackSource);

        //mainSoundtrackSource.PlayOneShot(soundtrackClip);
        mainSoundtrackSource.resource = soundtrackClip;
        mainSoundtrackSource.Play();
    }
    //Play an SFX clip
    public void PlaySFX(AudioClip sfxClip) {
        StopAudioPlaying(sfxSource);
        UnsetAudioLoop(sfxSource);

        sfxSource.resource = sfxClip;
        sfxSource.Play();
    }
    //Play a voice clip
    public void PlayDialogue(AudioClip dialogueClip /*, string voiceId*/) {
        //search voiceline based on voiceId
        StopAudioPlaying(dialogueSource);
        UnsetAudioLoop(dialogueSource);

        dialogueSource.resource = dialogueClip;
        dialogueSource.Play();
    }
    #endregion

    #region Play Methods Overrides
    //Play soundtrack with audio source of choice
    public void PlaySoundtrack(AudioClip soundtrackClip, AudioSource audioSource) {
        StopAudioPlaying(audioSource);
        UnsetAudioLoop(audioSource);

        audioSource.resource = soundtrackClip;
        audioSource.Play();
    }

    //Loop audio methods
    public void PlayLoopedSoundtrack(AudioClip soundtrackClip) {
        if (mainSoundtrackSource.isPlaying) {
            mainSoundtrackSource.Stop();
            mainSoundtrackSource.resource = null;

            StopAudioPlaying(secondarySoundtrackSource);
            SetAudioLoop(secondarySoundtrackSource);

            secondarySoundtrackSource.resource = soundtrackClip;
            secondarySoundtrackSource.Play();
        }
        else if (secondarySoundtrackSource.isPlaying) {
            secondarySoundtrackSource.Stop();
            secondarySoundtrackSource.resource = null;

            StopAudioPlaying(mainSoundtrackSource);
            SetAudioLoop(mainSoundtrackSource);

            mainSoundtrackSource.resource = soundtrackClip;
            mainSoundtrackSource.Play();
        }
        else {
            StopAudioPlaying(mainSoundtrackSource);
            SetAudioLoop(mainSoundtrackSource);

            mainSoundtrackSource.resource = soundtrackClip;
            mainSoundtrackSource.Play();
        }
    }
    public void PlayLoopedSFX(AudioClip sfxClip) {
        StopAudioPlaying(sfxSource);
        SetAudioLoop(sfxSource);

        sfxSource.resource = sfxClip;
        sfxSource.Play();
    }
    public void PlayLoopedDialogue(AudioClip dialogueClip) {
        StopAudioPlaying(dialogueSource);
        SetAudioLoop(dialogueSource);

        dialogueSource.resource = dialogueClip;
        dialogueSource.Play();
    }

    //Fade in and out methods
    public void PlayFadedSoundtrack(AudioClip soundtrackClip) {
        if (mainSoundtrackSource.isPlaying) {
            StopAllCoroutines();
            SetDefaultVolumes();

            IEnumerator fadeOut = FadeOut(mainSoundtrackSource, fadeOutTime);
            IEnumerator fadeIn = FadeIn(secondarySoundtrackSource, soundtrackClip, fadeInTime);

            StartCoroutine(fadeOut);
            StartCoroutine(fadeIn);
        }
        else if (secondarySoundtrackSource.isPlaying) {
            StopAllCoroutines();
            SetDefaultVolumes();

            IEnumerator fadeOut = FadeOut(secondarySoundtrackSource, fadeOutTime);
            IEnumerator fadeIn = FadeIn(mainSoundtrackSource, soundtrackClip, fadeInTime);

            StartCoroutine(fadeOut);
            StartCoroutine(fadeIn);
        }
        else {
            StopAllCoroutines();
            SetDefaultVolumes();

            IEnumerator fadeIn = FadeIn(mainSoundtrackSource, soundtrackClip, fadeInTime);

            StartCoroutine(fadeIn);
        }
    }

    //Random pitch methods
    public void PlaySFXVariablePitch(AudioClip sfxClip) {
        StopAudioPlaying(sfxSource);

        float pitchChange = Random.Range(minPitchChange, maxPitchChange);
        sfxSource.pitch += pitchChange;
        sfxSource.resource = sfxClip;
        sfxSource.Play();
        sfxSource.pitch = 1;    //set back to default
    }

    #endregion

    #region Utils
    private void SetDefaultVolumes() {
        mainSoundtrackSource.volume = mainSoundtrackDefaultVolume;
        secondarySoundtrackSource.volume = secondarySoundtrackDefaultVolume;
        sfxSource.volume = sfxDefaultVolume;
        dialogueSource.volume = dialogueDefaultVolume;
    }
    private void FillAudioSources() {
        audioSources.Add(mainSoundtrackSource);
        audioSources.Add(secondarySoundtrackSource);
        audioSources.Add(sfxSource);
        audioSources.Add(dialogueSource);
    }
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
