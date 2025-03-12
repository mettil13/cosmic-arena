using UnityEngine;
using UnityEngine.Audio;

public class AudioMenuManager : MonoBehaviour
{
    //Oggetti AudioMixer
    [Header("AudioMixer")]
    [SerializeField] private AudioMixer MasterVolume;


    //Metodi per abbassare/alzare il volume dei Mixer
    public void SetGeneralVolume(float sliderValue)
    {
        MasterVolume.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetSoundtrackVolume(float sliderValue)
    {
        MasterVolume.SetFloat("SoundtrackVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetSFXVolume(float sliderValue)
    {
        MasterVolume.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetVoiceVolume(float sliderValue)
    {
        MasterVolume.SetFloat("VoiceVolume", Mathf.Log10(sliderValue) * 20);
    }
}
