using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourcePool : MonoBehaviour{

    public List<AudioSource> sources = new List<AudioSource>();

    public float defaultVolume = 1, defaultPitch = 0;
    public bool defaultLoop = false, defaultPlayOnAwake = false;
    [SerializeField] private Transform audioSourcesParent;

    public AudioSource GenerateAudioSource() {
        AudioSource audioSource = InstantiateNewAudioSource(); ;
        return audioSource;
    }
    public void SetAudioSourceDefaultValues(AudioSource source) {
        source.volume = defaultVolume;
        source.pitch = defaultPitch;
        source.loop = defaultLoop;
        source.playOnAwake = defaultPlayOnAwake;
    }
    private AudioSource InstantiateNewAudioSource() {
        GameObject tempGameObject = new GameObject();
        tempGameObject.AddComponent<AudioSource>();
        tempGameObject.transform.parent = audioSourcesParent;
        return tempGameObject.GetComponent<AudioSource>();
    }
}
