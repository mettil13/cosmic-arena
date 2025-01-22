using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour {

    #region Variables
    private AudioManager instance;
    public AudioManager Instance {
        get {
            instance ??= FindFirstObjectByType<AudioManager>();
            return instance;
        }
    }

    //List of AudioMixers
    [SerializeField] private AudioMixer master;
    [SerializeField] private AudioSource mainSoundtrackSource, secondarySoundtrackSource;
    private AudioSource sfxSource;
    private AudioSource voiceSource;

    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;

    //RENAME
    private AudioMixerGroup genericAudioMixerGroup;
    [SerializeField] private float minPitchChange, maxPitchChange;
    #endregion

    #region LifeCycle

    #endregion

    #region Base Play Methods
    //Play a soundtrack clip
    public void PlaySoundtrack(AudioClip soundtrackClip) {
        mainSoundtrackSource.PlayOneShot(soundtrackClip);
    }
    //Play an SFX clip
    public void PlaySFX(AudioClip sfxClip) {
        sfxSource.PlayOneShot(sfxClip);
    }
    //Play a voice clip
    public void PlayVoice(AudioClip voiceClip /*, string voiceId*/) {
        //search voiceline based on voiceId
        voiceSource.PlayOneShot(voiceClip);
    }
    #endregion

    #region Play Methods Overrides
    //Loop audio methods
    public void PlaySoundtrack(AudioClip soundtrackClip, bool isLooped) {
        if (mainSoundtrackSource.isPlaying) {
            mainSoundtrackSource.loop = isLooped;
            PlaySoundtrack(soundtrackClip);
            mainSoundtrackSource.loop = false;
        }
        else if (secondarySoundtrackSource.isPlaying) {
            mainSoundtrackSource.loop = isLooped;
            PlaySoundtrack(soundtrackClip);
            mainSoundtrackSource.loop = false;
        }
    }
    public void PlaySFX(AudioClip sfxClip, bool isLooped) {
        sfxSource.loop = isLooped;
        PlaySFX(sfxClip);
        sfxSource.loop = false;
    }
    public void PlayVoice(AudioClip voiceClip, bool isLooped) {
        voiceSource.loop = isLooped;
        PlayVoice(voiceClip);
        voiceSource.loop = false;
    }

    //Fade in and out methods
    public void PlayFadedSoundtrack(AudioClip soundtrackClip) {
        if (mainSoundtrackSource.isPlaying) {
            IEnumerator fadeOut = FadeOut(mainSoundtrackSource, fadeOutTime);
            IEnumerator fadeIn = FadeIn(secondarySoundtrackSource, soundtrackClip, fadeOutTime);
        }
        else {
            IEnumerator fadeOut = FadeOut(secondarySoundtrackSource, fadeOutTime);
            IEnumerator fadeIn = FadeIn(mainSoundtrackSource, soundtrackClip, fadeOutTime);
        }
    }

    //Random pitch methods
    public void PlaySFXVariablePitch(AudioClip sfxClip) {
        float pitchChange = Random.Range(minPitchChange, maxPitchChange);
        sfxSource.pitch += pitchChange;
        sfxSource.PlayOneShot(sfxClip);
        sfxSource.pitch = 1;    //set back to default
    }
    //AudioSource.pitch
    #endregion

    #region Utils
    private void SetAudioLoop(AudioSource source) {
        source.loop = true;
    }
    private void UnsetAudioLoop(AudioSource source) {
        source.loop = false;
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
        float startVolume = 0;
        audioSource.PlayOneShot(audioClip);

        while (audioSource.volume < 1) {    //Substitute with settings volume value
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
    }
    #endregion

    //REMEMBER: Make UI elemts register the events
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
                genericAudioMixerGroup.audioMixer.SetFloat("SFXVolume", 20 * Mathf.Log10((slider.value) / 100);

                break;
            case "Voice":
                genericAudioMixerGroup.audioMixer.SetFloat("VoiceVolume", 20 * Mathf.Log10((slider.value) / 100));

                break;
        }
    }

}
